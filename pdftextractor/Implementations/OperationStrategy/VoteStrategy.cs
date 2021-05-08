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
        public int LawId { get; set; }
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

                    foreach (var vote in extractor.Votes)
                    {
                        vote.Law = null;
                        vote.LawId = LawId;
                        var depName = vote.Deputy.Name;

                        if (db.Deputies.Where(x => x.Name == depName).Any())
                        {
                            vote.Deputy = null;
                        }

                    }

                    db.Votes.AddRange(extractor.Votes);

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
