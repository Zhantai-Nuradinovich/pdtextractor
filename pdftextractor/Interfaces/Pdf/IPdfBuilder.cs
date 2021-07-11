using pdftextractor.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor.Interfaces
{
    public interface IPdfBuilder
    {
        List<string> GetArticlesUrls(string pageUrl);
        void DownloadFilesInArticle(string articleUrl);
        string RemoveHtmlCodes(string htmlString);
        VotesPdf GetFormattedVoteInfo(string rawHtml, string fileName);
    }
}
