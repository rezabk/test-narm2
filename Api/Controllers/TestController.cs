using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using System.Text;
using SixLabors.Fonts;

namespace Api.Controllers;

public class TestController : BaseController
{
    [HttpGet("[action]")]
    public IActionResult GeneratePdfController()
    {

        string studentId = "123456";
        string studentName = "نام و نام خانوادگی";
        string assignmentName = "نام تمرین";
        List<string> answers = new List<string> { "پاسخ اول", "پاسخ دوم", "پاسخ سوم" };

        // Create the PDF document in memory
        var pdfDocument = CreatePdfDocument(studentId, studentName, assignmentName, answers);

        // Render the PDF document to a memory stream
        using var stream = new MemoryStream();
        pdfDocument.Save(stream, false);

        // Return the PDF as a downloadable file
        stream.Position = 0; // Reset stream position
        return File(stream.ToArray(), "application/pdf", "PersianPdfExample.pdf");
    }
    
     private PdfDocument CreatePdfDocument(string studentId, string studentName, string assignmentName, List<string> answers)
    {
        // Initialize a new MigraDoc document
        Document document = new Document();
        Section section = document.AddSection();

        // Define the font path and load it with SixLabors.Fonts
        string fontPath = "C:\\Users\\Reza\\Desktop\\Final_Project-main\\Common\\B-NAZANIN.TTF"; // Update to actual font path
        var fontCollection = new FontCollection();
        var persianFontFamily = fontCollection.Add(fontPath);
        
        // Set RTL text direction for the document section
        section.PageSetup.RightMargin = Unit.FromCentimeter(1);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Portrait;

        // Helper method to set paragraph font
        void SetParagraphFont(Paragraph paragraph, bool isBold = false)
        {
            paragraph.Format.Font.Name = persianFontFamily.Name;
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Bold = isBold;
            paragraph.Format.Alignment = ParagraphAlignment.Right;
        }

        // Add student ID
        var studentIdParagraph = section.AddParagraph();
        studentIdParagraph.AddFormattedText("شماره دانشجویی: " + studentId);
        SetParagraphFont(studentIdParagraph, isBold: true);

        // Add student name
        var studentNameParagraph = section.AddParagraph();
        studentNameParagraph.AddFormattedText("نام و نام خانوادگی: " + studentName);
        SetParagraphFont(studentNameParagraph, isBold: true);

        // Add assignment name
        var assignmentNameParagraph = section.AddParagraph();
        assignmentNameParagraph.AddFormattedText("نام تمرین: " + assignmentName);
        SetParagraphFont(assignmentNameParagraph, isBold: true);

        // Add answers list with space before it
        section.AddParagraph("\n");
        int questionNumber = 1;

        foreach (var answer in answers)
        {
            // Question number
            var questionParagraph = section.AddParagraph();
            questionParagraph.AddFormattedText($"{questionNumber}- ", TextFormat.Bold);
            SetParagraphFont(questionParagraph);

            // Answer text
            var answerParagraph = section.AddParagraph();
            answerParagraph.AddFormattedText(answer);
            SetParagraphFont(answerParagraph);

            questionNumber++;
        }

        // Render document to PDF
        PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true)
        {
            Document = document
        };
        pdfRenderer.RenderDocument();
        
        return pdfRenderer.PdfDocument;
    }
}