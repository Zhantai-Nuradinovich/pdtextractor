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
                    Law law = new Law() { Name = LawName, DateTime = DateTime.Now };
                    law = db.Laws.Add(law).Entity;

                    db.SaveChanges();

                    LawId = law.Id;
                    return true;
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
