using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using pdftextractor.Data.Models;
using Syncfusion.Pdf.Parsing;

namespace pdftextractor
{
    public class ResultExtractor
    {
        string filePath { get; set; }
        public Vote[] Votes { get; set; } // Изменил на свойство, теперь получать голоса будет только раз, когда объект инициализируется
        public List<string> Initiators { get; set; }
        public int LawId { get; set; }
        public ResultExtractor(string _filePath, List<string> initiators, int lawId)
        {
            filePath = _filePath;
            Initiators = initiators;
            Votes = ParseFile();
            LawId = lawId;
        }
        public Vote[] ParseFile()
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileStream);
            string[] extracted = loadedDocument.Pages[0].ExtractText(true).Split('\n');

            loadedDocument.Close(true);

            Vote[] votes = new Vote[extracted.Length - 18];

            for(int i = 16; i < extracted.Length - 2; i++)
            {
                string currentLine = extracted[i];

                Vote vote = GetVoteResults(currentLine);

                var L_Error = vote.Deputy.Name.IndexOf("Л ");
                if(L_Error  > 0)
                {
                    vote.Deputy = new Deputy()
                    {
                        Name = vote.Deputy.Name.Remove(L_Error, 1)
                    };
                }
                vote.LawId = LawId;

                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    vote.DeputyId = db.Deputies.Where(d => d.Name == vote.Deputy.Name).FirstOrDefault()?.Id 
                        ?? db.Deputies.Add(vote.Deputy).Entity.Id;
                }


                votes[i - 16] = vote;
            }

            return votes;
        }



        public string GetString()
        {
            string res = "";
            foreach(Vote vote in Votes)
            {
                res += $"{vote.Deputy.Name} {vote.GetDecisionString()}\n";
            }
            res = res.Substring(0, res.Length - 1);
            return res;
        }

        private Vote GetVoteResults(string currentLine)
        {
            var vote = new Vote();

            switch (currentLine.Substring(currentLine.Length - 3, 1))
            {
                case "л":
                    {
                        vote.Decision = Decision.Absent;

                        if (currentLine[currentLine.Length - 15] == 'н')
                            vote.Deputy = new Deputy()
                            {
                                Name = currentLine.Substring(0, currentLine.Length - 15)
                            };

                        else
                            vote.Deputy = new Deputy()
                            {
                                Name = currentLine.Substring(0, currentLine.Length - 14)
                            };
                    }
                    break;

                case "а":
                    {
                        vote.Decision = Decision.Absent;

                        if (currentLine[currentLine.Length - 14] == 'н')
                            vote.Deputy = new Deputy()
                            {
                                Name = currentLine.Substring(0, currentLine.Length - 14)
                            };
                        else
                            vote.Deputy = new Deputy()
                            {
                                Name = currentLine.Substring(0, currentLine.Length - 13)
                            };
                    }
                    break;

                case "з":
                    {
                        vote.Decision = Decision.Agreed;
                        vote.Deputy = new Deputy()
                        {
                            Name = currentLine.Substring(0, currentLine.Length - 3)
                        };
                    }
                    break;

                case "и":
                    {
                        vote.Decision = Decision.Rejected;
                        vote.Deputy = new Deputy()
                        {
                            Name = currentLine.Substring(0, currentLine.Length - 7)
                        };
                    }
                    break;
            }

            if (Initiators.Contains(vote.Deputy.Name))
                vote.Decision = Decision.Initiator;

            return vote;
        }
    }
}
