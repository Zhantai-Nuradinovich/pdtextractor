using pdftextractor.Data.Models;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Implementations
{
    public class LawsAmendmentStrategy : IOperationStrategy
    {
        int LawsAmendmentId { get; set; }
        public int LawId { get; set; }
        public DateTime AmendmentDate { get; set; }
        public string LinkLaw { get; set; } 
        public string LinkVotes { get; set; }
        public int Id
        {
            get => LawsAmendmentId;
        }
        public bool AddToDb()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    TLawsAmendment lawsAmendment = new TLawsAmendment() 
                    { 
                        LawId = LawId,
                        AmendmentDate = AmendmentDate,
                        LinkLaw = LinkLaw,
                        LinkVotes = LinkVotes
                    };

                    lawsAmendment = db.Add(lawsAmendment).Entity;
                    db.SaveChanges();

                    LawsAmendmentId = lawsAmendment.Id;

                    return true;
                }
            }
            catch (Exception)
            {
                LawsAmendmentId = -1;
                return false;
            }
        }
    }
}
