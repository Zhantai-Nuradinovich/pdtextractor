using pdftextractor.Operators;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace pdftextractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WindowWidth = 160;
            Console.WindowHeight = 40;
            string filePath = "http://kenesh.kg/uploads/media/default/0001/63/010421110053_31-03-2021%2002-06%20%D0%93%D0%BE%D0%BB%D0%BE%D1%81%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%2014.pdf";
            string pageUrl = "http://kenesh.kg/ru/article/list/11?page=1";
            //var extractor = new ResultExtractor(filePath); 

            //var VoteOper = new VoteOperator(extractor); //Будет отвечать за операции над голосами

            string articleUrl = "http://kenesh.kg/ru/article/show/7913/zakonoproekti-vnesennie-na-golosovanie-21-aprelya-2021-goda";
            var test = new DatabaseUpdater();
            //var urls = test.GetArticlesUrls(pageUrl);

            test.UpdateDatabase();

            //Console.WriteLine(VoteOper.AddToDb());
            //Console.Write(extractor.GetString());
        }
    }
}
