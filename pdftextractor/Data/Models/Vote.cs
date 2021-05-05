using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Data.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }
        public int LawId { get; set; }
        public Law Law { get; set; }
        public int DeputyId { get; set; }
        public Deputy Deputy { get; set; }
        public Decision Decision { get; set; }

        public string GetDecisionString()
        {
            return Decision switch
            {
                Decision.Absent => "Отсутствовал",
                Decision.Agreed => "За",
                Decision.Rejected => "Против",
                _ => "Инициатор"
            };
        }
    }
}