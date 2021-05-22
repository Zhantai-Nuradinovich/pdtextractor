using pdftextractor.Data.Models;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Implementations
{
    public class VoteStrategy : IOperationStrategy
    {
        public ResultExtractor extractor { get; set; }
        int LawId { get; set; }
        public int Id 
        { 
            get => LawId; 
        }

        public VoteStrategy(ResultExtractor extractor, int lawId)
        {
            this.extractor = extractor;
            LawId = lawId;
        }

        public bool AddToDb()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    List<Vote> votesToDb = new List<Vote>();
                    foreach (var vote in extractor.Votes)
                    {
                        Vote newVote = new Vote();
                        newVote.LawId = LawId;

                        newVote.Decision = vote.Decision;

                        var depName = vote.Deputy?.Name;

                        if (db.Deputies.Where(d => d.Name == depName).Any())
                            newVote.DeputyId = db.Deputies.Where(d => d.Name == depName).FirstOrDefault().Id;
                        else
                            newVote.Deputy = vote.Deputy;
                        

                        votesToDb.Add(newVote);
                    }

                    db.Votes.AddRange(votesToDb);

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
                return false;
            }
        }
    }
}
