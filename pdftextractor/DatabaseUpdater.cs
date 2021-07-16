using pdftextractor.Data.Models;
using pdftextractor.Decorators;
using pdftextractor.Implementations;
using pdftextractor.Interfaces;
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
        public DatabaseUpdater(IPdfBuilder builder)
        {
            Builder = builder;
        }
        public void UpdateDatabase()
        {
            int curPageIndex = 1;
            ResultExtractor resultExtractor;

            while(true) //one page
            {
                string pageUrl = baseUrl + curPageIndex;
                List<string> articlesOnPage = Builder.GetArticlesUrls(pageUrl);
                ConsoleExtensions.WriteInConsole("Checking articles on " + pageUrl, ConsoleColor.Yellow);

                if (articlesOnPage.Count == 0)
                {
                    ConsoleExtensions.WriteInConsole("Page empty, ending update", ConsoleColor.Blue);//probably, it stops the application (Ask Aman to run the old version of the app)
                    break; //continue;
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

                        Strategy = new LawStrategy() { LawName = votesPdf.LawName };
                        int id = UpdateStrategysTable(Strategy);

                        resultExtractor = new ResultExtractor(directory + "/" + votesPdf.FileName + ".pdf", votesPdf.Initiators, id);

                        Strategy = new VoteStrategy(resultExtractor, id);
                        bool isAdded = UpdateStrategysTable(Strategy) == id;

                        if (!isAdded)
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

        int UpdateStrategysTable(IOperationStrategy strategy)
        {
            if (strategy.AddToDb())
                return strategy.Id;
            
            return -1;
        }
    }
}
