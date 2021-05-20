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

        public DeputyStrategy(int deputyId, string deputyName)
        {
            DeputyId = deputyId;
            DeputyName = deputyName;
        }

        public bool AddToDb()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext()) // юсинг для освобождения ресурсов после использования контекста
                {
                    if (!db.Deputies.Where(x => x.Name == DeputyName).Any())
                    {
                        DeputyId = db.Deputies.Add(new Deputy() { Name = DeputyName }).Entity.Id;
                    }

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
