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

        public DateTime GetDate()
        {
            string[] fullDateFromPdf = FileName.Substring(FileName.IndexOf("от ") + 3, FileName.IndexOf(" года") - (FileName.IndexOf("от ") + 3)).Split(' ');
            int day = int.Parse(fullDateFromPdf[0]);
            int month = GetMonthNumber(fullDateFromPdf[1]);
            int year = int.Parse(fullDateFromPdf[2]);
            DateTime date = new DateTime(year, month, day);

            return date;
        }

        private string GetLawNumber()
        {
            string number = FileName.Substring(FileName.IndexOf("№ ") + 2, FileName.IndexOf(" (") - FileName.IndexOf("№ ") + 2);

            return number;
        }
        private int GetMonthNumber(string month) =>
            month switch
            {
                "Январь" => 1,
                "Февраль" => 2,
                "Март" => 3,
                "Апрель" => 4,
                "Май" => 5,
                "Июнь" => 6,
                "Июль" => 7,
                "Август" => 8,
                "Сентябрь" => 9,
                "Октябрь" => 10,
                "Ноябрь" => 11,
                "Декарь" => 12,
                _ => throw new ArgumentException(message: "Нет такого месяца", paramName: month),
            };
    }
}
