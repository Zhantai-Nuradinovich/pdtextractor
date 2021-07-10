using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Data.Models
{
    public class TLawsAmendment: BaseModel
    {
        public int LawId { get; set; }
        public TLaw Law { get; set; }
        public DateTime AmendmentDate { get; set; }
        public string LinkLaw { get; set; } //какой закон правили
        public string LinkVotes { get; set; } //как голосовали
        public ICollection<TVote> Votes { get; set; }
    }
}
