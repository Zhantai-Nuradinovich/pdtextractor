using pdftextractor.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Operators
{
    public class LawOperator
    {
        public int AddToDb(string lawName)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    TLaw law = new TLaw() { LawName = lawName };
                    law = db.Laws.Add(law).Entity;

                    db.SaveChanges();

                    return law.Id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
