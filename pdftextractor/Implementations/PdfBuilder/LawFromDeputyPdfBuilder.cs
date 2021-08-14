using pdftextractor.Data.Models;
using pdftextractor.Decorators;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Implementations
{
    public class LawFromDeputyPdfBuilder : IPdfBuilder
    {
        const string baseUrl = "http://kenesh.kg/ru/article/list/11?page="; //baseUrl->pages->articles->files
        const string JK = "Жогорку Кеңеш";
        const string government = "Правительство";
        string directory = "../../../Downloaded Files";
        public List<string> GetArticlesUrls(string pageUrl)
        {
            var articles = new List<string>();
            string htmlCode;
            using (var myWebClient = new WebClient())
            {
                htmlCode = myWebClient.DownloadString(pageUrl);
            }

            int lastIndex = 0;
            while (true)
            {
                int curIndex = htmlCode.IndexOf("more-link", lastIndex); //more-link - стили <a>, чтобы получить ссылку
                if (curIndex == -1)
                    break;

                int urlStartIndex = htmlCode.IndexOf('/', curIndex);
                int urlEndIndex = htmlCode.IndexOf('\"', urlStartIndex);
                string articleUrl = "http://kenesh.kg" + htmlCode.Substring(urlStartIndex, urlEndIndex - urlStartIndex);

                articles.Add(articleUrl);

                lastIndex = curIndex + 20;
            }

            return articles;
        }

        public void DownloadFilesInArticle(string articleUrl)
        {
            string htmlCode;

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);

            using (var myWebClient = new WebClient())
            {
                htmlCode = myWebClient.DownloadString(articleUrl);

                var pdfFiles = new List<VotesPdf>();

                int lastIndex = 0;
                int lastName = 0;

                while (true)
                {
                    int curIndex = htmlCode.IndexOf("p><a", lastIndex);
                    if (curIndex == -1)
                        break;

                    int urlStartIndex = curIndex + 11;
                    int urlEndIndex = htmlCode.IndexOf("\"", urlStartIndex);
                    string url = "http://kenesh.kg" + htmlCode.Substring(urlStartIndex, urlEndIndex - urlStartIndex);

                    int rawLawStringStartIndex = htmlCode.IndexOf("</strong>", urlEndIndex);
                    int rawLawStringEndIndex = htmlCode.IndexOf("p>", urlEndIndex);
                    string rawLawString = htmlCode.Substring(rawLawStringStartIndex, rawLawStringEndIndex - rawLawStringStartIndex + 2);

                    lastName++;
                    VotesPdf fileInfo = GetFormattedVoteInfo(rawLawString, lastName.ToString("D2"));

                    pdfFiles.Add(fileInfo);

                    myWebClient.DownloadFile(url, directory + $"/{fileInfo.FileName}.pdf");

                    lastIndex = curIndex + 20;
                }

                ConsoleExtensions.WriteInConsole("Downloaded pdfs from " + articleUrl);

                using (FileStream fs = new FileStream(directory + "/pdf files info.xml", FileMode.OpenOrCreate))
                {
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(List<VotesPdf>));
                    xs.Serialize(fs, pdfFiles);
                }
            }
        }


        public VotesPdf GetFormattedVoteInfo(string rawHtml, string fileName)
        {
            string lawName = "";
            var initiators = new List<string>();
            int curIndex = 0;
            int lawNameEndIndex;

            rawHtml = WebUtility.HtmlDecode(rawHtml);

            while (curIndex < rawHtml.Length)
            {
                if (rawHtml[curIndex] == '<')
                {
                    if (rawHtml[curIndex + 1] == 'e')
                    {
                        lawNameEndIndex = curIndex;
                        lawName = RemoveHtmlCodes(rawHtml.Substring(0, lawNameEndIndex));

                        int initiatorStringStartIndex = rawHtml.IndexOf("депутат", lawNameEndIndex);

                        if (initiatorStringStartIndex == -1)
                        {
                            bool isGovernment = rawHtml.Substring(lawNameEndIndex, 50).Contains(government);
                            if (isGovernment)
                                initiators.Add(government);
                            else
                                initiators.Add(JK);
                        }
                        else
                        {
                            int initiatorsStartIndex = rawHtml.IndexOf(' ', initiatorStringStartIndex);
                            int initiatorsEndIndex = rawHtml.IndexOf(")", initiatorsStartIndex);
                            string initiatorString = RemoveHtmlCodes(rawHtml.Substring(initiatorsStartIndex, initiatorsEndIndex - initiatorsStartIndex));
                            initiators.AddRange(initiatorString.Split(','));
                        }
                        break;
                    }
                }

                curIndex++;
            }

            rawHtml = rawHtml.Trim();
            lawName = lawName.Trim();

            var formattedVoteInfo = new VotesPdf()
            {
                LawName = lawName,
                Initiators = initiators,
                FileName = fileName,
            };

            return formattedVoteInfo;
        }

        public string RemoveHtmlCodes(string htmlString)
        {
            htmlString = WebUtility.HtmlDecode(htmlString);
            int curIndex = 0;

            while (curIndex < htmlString.Length)
            {
                if (htmlString[curIndex] == '<')
                {
                    int leftBracketIndex = htmlString.IndexOf('>', curIndex);
                    htmlString = htmlString.Remove(curIndex, leftBracketIndex - curIndex + 1);
                }
                else
                    curIndex++;
            }
            htmlString.Trim();

            return htmlString;
        }
    }
}
