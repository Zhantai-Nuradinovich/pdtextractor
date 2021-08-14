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

        List<string> month = new List<string>
            {
                "января","февраля","марта",
                "апреля", "мая", "июня",
                "июля", "августа", "сентября",
                "октября", "ноября","декабря"
            };

        public DateTime GetDate()
        {
            List<string> words = FileName.Split(new char[] { ' ', ',', '!', ')', '(', '?' }).ToList();
            for (int i = 0; i < words.Count; i++)
            {
                try
                {
                    DateTime answer = DateTime.Parse(words[i]);
                    return answer;
                }
                catch
                {
                    if (i == words.Count - 1 || i == words.Count - 2 || i == words.Count - 3)
                    {
                        break;
                    }
                    else if (CheckOnNum(words[i]) && month.Contains(words[i + 1]) && CheckOnNum(words[i + 2]) && words[i + 3] == "года")
                    {
                        int numMonth = month.IndexOf(words[i + 1]);
                        return new DateTime(int.Parse(words[i + 2]), numMonth, int.Parse(words[i]));
                    }
                }
            }
            return new DateTime();
        }

        public string GetLawNumber()
        {
            try
            {
                string str = FileName.Substring(FileName.IndexOf('№') + 2);

                int index1 = 0;
                int index2 = str.IndexOf(')');
                string strin = str.Substring(index1, index2);
                string[] NumAndDate = strin.Split(new string[] { " от " }, StringSplitOptions.RemoveEmptyEntries);

                return NumAndDate[0];
            }
            catch
            {
                return "Нет номера";
            }
        }
        public static bool CheckOnNum(string obj)
        {
            try
            {
                Convert.ToInt32(obj);
                return true;
            }
            catch { return false; }
        }
    }
}
