
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Data.Models
{
    public class TLaw: BaseModel
    {
        public string LawName { get; set; }
        public string LawNumber { get; set; }//ПОПРАВИТЬ В АПИ
        public DateTime LawDate { get; set; }
        public LawCategory Category { get; set; }
        public string AddInfo { get; set; }
        public ICollection<TLawsAmendment> Amendments { get; set; }
    }
}