using pdftextractor.Data.Models;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Implementations
{
    public class DeputyStrategy : IOperationStrategy
    {
        int DeputyId { get; set; }
        string DeputyName { get; set; }
        public int Id
        {
            get => DeputyId;
        }

        public DeputyStrategy(string deputyName)
        {
            DeputyName = deputyName;
        }

        public bool AddToDb()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    var deputy = new Deputy() { Name = DeputyName };

                    if (db.Deputies.Where(d => d.Name == DeputyName).Any())
                    {
                        DeputyId = db.Deputies.Where(d => d.Name == DeputyName).FirstOrDefault().Id;
                        return true;
                    }

                    deputy = db.Add(deputy).Entity;
                    db.SaveChanges();

                    DeputyId = deputy.Id;

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
