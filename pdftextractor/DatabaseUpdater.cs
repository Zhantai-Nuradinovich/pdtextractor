using pdftextractor.Data.Models;
using pdftextractor.Decorators;
using pdftextractor.Implementations;
using pdftextractor.Interfaces;
using pdftextractor.Operators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor
{
    public class DatabaseUpdater
    {
        public IOperationStrategy Strategy { get; set; }
        public IPdfBuilder Builder { get; set; }

        const string baseUrl = "http://kenesh.kg/ru/article/list/11?page="; //baseUrl->pages->articles->files
        const string JK = "Жогорку Кеңеш";
        const string government = "Правительство";
        string directory = "../../../Downloaded Files";

        //нужно сохранить адрес последней загруженной страницы
        public DatabaseUpdater(IOperationStrategy strategy, IPdfBuilder builder)
        {
            Strategy = strategy;
            Builder = builder;
        }
        public void UpdateDatabase()
        {
            int curPageIndex = 1;
            ResultExtractor resultExtractor;
            VoteOperator voteOperator;
            LawOperator lawOperator;
            DeputyOperator deputyOperator;

            while(true)
            {
                string pageUrl = baseUrl + curPageIndex;
                List<string> articlesOnPage = Builder.GetArticlesUrls(pageUrl);

                ConsoleExtensions.WriteInConsole("Checking articles on " + pageUrl, ConsoleColor.Yellow);

                if (articlesOnPage.Count == 0)
                {
                    ConsoleExtensions.WriteInConsole("Page empty, ending update", ConsoleColor.Blue);
                    break;
                }

                foreach(string articleUrl in articlesOnPage)
                {
                    Builder.DownloadFilesInArticle(articleUrl);

                    List<VotesPdf> votesPdfs;

                    using (FileStream fs = new FileStream(directory + "/pdf files info.xml", FileMode.OpenOrCreate))
                    {
                        var xs = new System.Xml.Serialization.XmlSerializer(typeof(List<VotesPdf>));
                        votesPdfs = xs.Deserialize(fs) as List<VotesPdf>;
                    }

                    foreach (var votesPdf in votesPdfs)
                    {
                        //deputyOperator = new DeputyOperator(votesPdf.Initiators);
                        lawOperator = new LawOperator();
                        int id = lawOperator.AddToDb(votesPdf.LawName);
                        resultExtractor = new ResultExtractor(directory + "/" + votesPdf.FileName + ".pdf", votesPdf.Initiators, id);
                        voteOperator = new VoteOperator(resultExtractor);
                        bool isAdded = voteOperator.AddToDb(id);
                        if(!isAdded)
                        {
                            ConsoleExtensions.WriteInConsole($"Failed to add {votesPdf.FileName}.pdf, terminating update", ConsoleColor.Red);
                            return;
                        }
                    }
                }
                curPageIndex++;
            }

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
