using pdftextractor.Data.Models;
using pdftextractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor
{
    public class PdfProcessor
    {
        //Вместо интерфейса добавить можно реализацию конкретную
        //Если, например, закон издан правительством, то просто отнаследуйся от IPdfBuilder и создай другую реализацию БЕЗ изменения кода, только расширение
        IPdfBuilder pdfBuilder { get; set; }

        public PdfProcessor(IPdfBuilder pdfBuilder)
        {
            this.pdfBuilder = pdfBuilder;
        }

        public List<VotesPdf> ConstructPdf()
        {
            //Здесь конечный результат
            //Просто вызвать методы из builderа
            //Пример:
            //      pdfBuilder.GetArticlesUrls("pathurl");
            //      pdfBuilder.DownloadFilesInArticle("articleUrl");
            //      pdfBuilder.RemoveHtmlCodes("htmlString");
            //      return pdfBuilder.GetFormattedVoteInfo("rawHtml", "fileName");
            //Вся логика будет в конкретных реализациях
            return null;
        } 
    }
}
