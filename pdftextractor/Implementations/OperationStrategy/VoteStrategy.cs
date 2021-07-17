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
        int LawsAmendmentId { get; set; }
        public int Id 
        { 
            get => LawsAmendmentId; 
        }

        public VoteStrategy(ResultExtractor extractor, int lawsAmendmentId)
        {
            this.extractor = extractor;
            LawsAmendmentId = lawsAmendmentId;
        }

        public bool AddToDb()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    List<TVote> votesToDb = new List<TVote>();
                    foreach (var vote in extractor.Votes)
                    {
                        TVote newVote = new TVote();
                        newVote.LawsAmendmentId = LawsAmendmentId;
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
