using pdftextractor.Data.Models;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Implementations
{
    public class LawStrategy : IOperationStrategy
    {
        public string LawName { get; set; }
        public string LawNumber { get; set; }
        public LawCategory Category { get; set; }
        public string AddInfo { get; set; }
        public DateTime LawDate { get; set; }
        public int LawId { get; set; }
        public int Id
        {
            get => LawId;
        }
        public bool AddToDb()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    TLaw law = new TLaw() 
                    { 
                        LawName = LawName,
                        LawNumber = LawNumber,
                        Category = Category,
                        AddInfo = AddInfo,
                        LawDate = LawDate
                    };

                    if(db.Laws.Where(l => l.LawName == LawName).Any())
                    {
                        LawId = db.Laws.Where(l => l.LawName == LawName).FirstOrDefault().Id;
                        return false;
                    }

                    law = db.Add(law).Entity;
                    db.SaveChanges();

                    LawId = law.Id;

                    return db.Amendments.Any(x => x.LawId == LawId); //Есть поправки - голоса пройдут, нет - нет.
                }
            }
            catch (Exception)
            {
                LawId = -1;
                return false;
            }
        }
    }
}
