using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using SyncFusionPDFBurning.Model;
using System.IO;

namespace SyncFusionPDFBurning.Controllers
{
    [ApiController]
    public class BurnController : ControllerBase
    {
        private readonly ILogger<BurnController> _logger;

        public BurnController(ILogger<BurnController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/burn")]
        public IActionResult Post([FromForm] DeficiencyData deficiencydata)
        {
            Metadata metadata = new Metadata()
            {
                fontColour = deficiencydata.fontColour,
                fontFamily = deficiencydata.fontFamily,
                fontSize = deficiencydata.fontSize,
                x = deficiencydata.x,
                y = deficiencydata.y,
                height = deficiencydata.height,
                width = deficiencydata.width,
                pageNumber = deficiencydata.pageNumber,
                text = deficiencydata.text
            };
            return textOnExistingPDF(deficiencydata.file, metadata);
            //return textOnBlankPdf();

        }

        private FileStreamResult textOnExistingPDF(IFormFile pdf, Metadata metadata)
        {

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15f);
            PdfBrush brush = new PdfSolidBrush(Color.Black);

            //Creates a copy PDF document to edit.
            var memoryStream = new MemoryStream();
            pdf.CopyTo(memoryStream);
            PdfLoadedDocument document = new PdfLoadedDocument(memoryStream);
            PdfDocument doc = new PdfDocument();
            doc.ImportPageRange(document, 0, document.Pages.Count - 1);

            RectangleF defrectangle = new RectangleF(metadata.x, metadata.y, metadata.height, metadata.width);

            if (metadata.pageNumber > 0)
            {
                doc.Pages[metadata.pageNumber - 1].Graphics.DrawString(metadata.text, font, brush, defrectangle);//draw on the pagenumber specified
            }
            
            
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            ms.Position = 0;
            doc.Close(true);

            //Download the PDF document in the browser.
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
            fileStreamResult.FileDownloadName = "Annotation.pdf";
            return fileStreamResult;
        }



        private FileStreamResult textOnBlankPdf()
        {
            //Creates a new PDF document.
            PdfDocument document = new PdfDocument();

            //Creates a new page 
            PdfPage page = document.Pages.Add();
            document.PageSettings.SetMargins(0);

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10f);
            PdfBrush brush = new PdfSolidBrush(Color.Black);
            /*
                        RectangleF freetextrect = new RectangleF(405, 645, 80, 30);
                        PdfFreeTextAnnotation freeText = new PdfFreeTextAnnotation(freetextrect);

                        freeText.MarkupText = "Electronically signed by \n Tchowdhury\n ";
                        freeText.TextMarkupColor = new PdfColor(Color.GreenYellow);

                        freeText.Font = new PdfStandardFont(PdfFontFamily.Courier, 117f);
                        freeText.BorderColor = new PdfColor(Color.OrangeRed);
                        freeText.Border = new PdfAnnotationBorder(0f);
                        freeText.AnnotationFlags = PdfAnnotationFlags.Locked;
                        freeText.Text = "Electronically signed by Tulika";
                        freeText.Color = new PdfColor(Color.Transparent);
                        PointF[] Freetextpoints = { new PointF(365, 700), new PointF(379, 654), new PointF(405, 654) };
                        freeText.CalloutLines = Freetextpoints;*/

            string deficiencyText = "Electronically Signed by \n TChowdhury \n today";
            RectangleF defrectangle = new RectangleF(405, 645, 80, 30);
            page.Graphics.DrawString(deficiencyText, font, brush, defrectangle);
           /* page.Annotations.Add(freeText);*/

            MemoryStream SourceStream = new MemoryStream();
            document.Save(SourceStream);
            document.Close(true);

            //Creates a new Loaded document.
            PdfLoadedDocument lDoc = new PdfLoadedDocument(SourceStream);
            /*PdfLoadedPage lpage1 = lDoc.Pages[0] as PdfLoadedPage;*/


            //Save the PDF to the MemoryStream
            MemoryStream ms = new MemoryStream();

            lDoc.Save(ms);

            //If the position is not set to '0' then the PDF will be empty.
            ms.Position = 0;

            lDoc.Close(true);

            //Download the PDF document in the browser.
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
            fileStreamResult.FileDownloadName = "Annotation.pdf";
            return fileStreamResult;
        }

    }
}
