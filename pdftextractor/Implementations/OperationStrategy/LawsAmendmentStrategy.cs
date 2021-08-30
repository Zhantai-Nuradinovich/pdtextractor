using pdftextractor.Data.Models;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Implementations
{
    public class LawsAmendmentStrategy : IOperationStrategy
    {
        int LawsAmendmentId { get; set; }
        public int LawId { get; set; }
        public DateTime AmendmentDate { get; set; }
        public string LinkToLaw { get; set; } 
        public string LinkToVotes { get; set; }
        public int Id
        {
            get => LawsAmendmentId;
        }

        public string AuxLawNumber { get; set; }
        public bool AddToDb()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (db.Amendments.Any(x => x.LinkToVotes == LinkToVotes))//Проверка на то, чтобы не дублировались поправки
                    {
                        LawsAmendmentId = db.Amendments.Where(x => x.LinkToVotes == LinkToVotes).FirstOrDefault().Id;
                        return true;
                    }
                    TLawsAmendment lawsAmendment = new TLawsAmendment() 
                    { 
                        LawId = LawId,
                        AmendmentDate = AmendmentDate,
                        LinkToLaw = LinkToLaw,
                        LinkToVotes = LinkToVotes
                    };
                    if (AuxLawNumber != "Нет номера")
                        lawsAmendment.LinkToLaw = GetLinkToLawFromMinjust(lawsAmendment.LawId, lawsAmendment.AmendmentDate, db);
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

        private string GetLinkToLawFromMinjust(int lawId, DateTime amendmentDate, ApplicationDbContext db)
        {
            //TLaw law = db.Laws.Where(x => x.Id == lawId).FirstOrDefault();
            //string lawName = law.LawName;
            //using (var client = new WebClient())
            //{
            //    client.BaseAddress = "http://cbd.minjust.gov.kg/OpenData/GetDocumentListByQuery?Name=";

            //}
            string code = "";
            
            Random random = new Random();
            const string chars = "0123456789";
            code = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "http://cbd.minjust.gov.kg/act/view/ru-ru/" + code;
        }
    }
}
