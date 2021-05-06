using pdftextractor.Data.Models;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Implementations
{
    public class LawFromDeputyPdfBuilder : IPdfBuilder
    {
        public void DownloadFilesInArticle(string articleUrl)
        {
            throw new NotImplementedException();
        }

        public List<string> GetArticlesUrls(string pageUrl)
        {
            throw new NotImplementedException();
        }

        public VotesPdf GetFormattedVoteInfo(string rawHtml, string fileName)
        {
            throw new NotImplementedException();
        }

        public string RemoveHtmlCodes(string htmlString)
        {
            throw new NotImplementedException();
        }
    }
}
