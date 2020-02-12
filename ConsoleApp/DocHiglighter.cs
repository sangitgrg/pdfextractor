using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace ConsoleApp
{
   public class DocHiglighter
    {
        string pdfPath = "D:\\Documents\\Tag Numbering Specification.pdf";
        void SearchAndHighlight(string text)
        {
            // initialize pdf writer
            PdfWriter pdfWriter = new PdfWriter(pdfPath);
            // initialize pdf document
            PdfDocument pdfDoc = new PdfDocument(pdfWriter);
            // initialize document
            Document pdfDocument = new Document(pdfDoc);

            
        }

        public virtual void SetTextAnnotaion(String dest)
        {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            //Initialize document
            Document document = new Document(pdf);
            Paragraph p = new Paragraph("The example of text markup annotation.");
            document.ShowTextAligned(p, 20, 795, 1, TextAlignment.LEFT, VerticalAlignment.MIDDLE, 0);
            //Create text markup annotation
            PdfAnnotation ann = PdfTextMarkupAnnotation.CreateHighLight(new Rectangle(105, 790, 64, 10), new float[] {
                169, 790, 105, 790, 169, 800, 105, 800 }).SetColor(ColorConstants.YELLOW).SetTitle(new PdfString("Hello!")).SetContents
                (new PdfString("I'm a popup.")).SetTitle(new PdfString("iText")).SetRectangle(new PdfArray
                (new float[] { 100, 600, 200, 100 }));
            pdf.GetFirstPage().AddAnnotation(ann);
            //Close document
            document.Close();
        }

        public List<int> ReadPdfFile(string fileName, String searthText)
        {
            //PdfDocument reader = new PdfDocument(new PdfReader(fileName));
            //FilteredEventListener listener = new FilteredEventListener();
            //var strat = listener.AttachEventListener(new TextLocationStrategy());
            //PdfCanvasProcessor processor = new PdfCanvasProcessor(listener);
            //processor.ProcessPageContent(reader.GetPage(1));
            //Console.WriteLine("=== FINDING COORDINATES ===");
            //string curtPageText = PdfTextExtractor.GetTextFromPage(reader.GetPage(page), strategy);

            //foreach (var d in strat.objectResult)
            //{
            //    Console.WriteLine("Char >" + d.Text + " X >" + d.Rect.GetX() + " font >" + d.FontFamily + " font size >" + d.FontSize.ToString() + " space >" + d.SpaceWidth);

            //}


            List<int> pages = new List<int>();
            if (File.Exists(fileName))
            {
                using (PdfReader pdfReader = new PdfReader(fileName))
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    FilteredEventListener listener = new FilteredEventListener();
                    var strat = listener.AttachEventListener(new TextLocationStrategy());
                    PdfCanvasProcessor processor = new PdfCanvasProcessor(listener);
                    for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                    {
                        ITextExtractionStrategy strategy = new TextLocationStrategy();
                        
                        processor.ProcessPageContent(pdfDocument.GetPage(page));
                        string currentPageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy);

                        if (currentPageText.ToLower().Contains(searthText.ToLower()))
                        {
                            foreach (var d in strat.objectResult)
                            {
                                Console.WriteLine("Char >" + d.Text + " X >" + d.Rect.GetX() + " font >" + d.FontFamily + " font size >" + d.FontSize.ToString() + " space >" + d.SpaceWidth);

                            }
                            pages.Add(page);
                        }
                    }
                }
            }
            return pages;
        }
      
              public List<int> ReadPdfFileTest(string fileName, String searthText)
        {
            List<int> pages = new List<int>();
            if (File.Exists(fileName))
            {
                using (PdfReader pdfReader = new PdfReader(fileName))
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                        string currentPageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy);
                        if (currentPageText.Contains(searthText))
                        {
                            FilteredEventListener listener = new FilteredEventListener();
                            var strat = listener.AttachEventListener(new TextLocationStrategy());
                            PdfCanvasProcessor processor = new PdfCanvasProcessor(listener);

                            pages.Add(page);
                        }
                    }
                    pdfReader.Close();
                }                
            }
            return pages;
        }

    }
}
