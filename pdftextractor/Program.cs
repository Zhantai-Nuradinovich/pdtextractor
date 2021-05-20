using pdftextractor.Implementations;
using pdftextractor.Interfaces;
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

            IPdfBuilder builder = new LawFromDeputyPdfBuilder();
            var test = new DatabaseUpdater(builder);

            test.UpdateDatabase();
        }
    }
}
