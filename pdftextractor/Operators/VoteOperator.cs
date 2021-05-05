using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Operators
{
    public class VoteOperator
    {
        public ResultExtractor extractor { get; set; }

        public VoteOperator(ResultExtractor extractor)
        {
            this.extractor = extractor;
        }

        public bool AddToDb(int lawId)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {

                    foreach (var vote in extractor.Votes)
                    {
                        vote.Law = null;
                        vote.LawId = lawId;
                        var depName = vote.Deputy.Name;

                        if(db.Deputies.Where(x => x.Name == depName).Any())
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
