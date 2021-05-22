
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Data.Models
{
    public class Law
    {
        [Key]
        public int Id { get; set; }
        public string LawName { get; set; }
        public DateTime OfferDate { get; set; }
    }
}