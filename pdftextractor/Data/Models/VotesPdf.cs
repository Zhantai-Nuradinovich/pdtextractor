using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Data.Models
{
    public class VotesPdf
    {
        public string LawName { get; set; }
        public List<string> Initiators { get; set; }
        public string FileName { get; set; }

    }
}
