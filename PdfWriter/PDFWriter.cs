using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace pdfWriter
{
  internal class PDFWriter
  {
    private readonly iTextSharp.text.Document _document;
    private readonly string _docFilename;
    private PdfWriter _wrPdf;
    private PdfContentByte _canvas;
    private BaseFont _font;

    public PDFWriter(string filename)
    {
      _document = new Document();
      _docFilename = filename;
      _wrPdf = PdfWriter.GetInstance(_document, new FileStream(_docFilename, FileMode.Create));
      _document.Open();
      _canvas = _wrPdf.DirectContent;
      _font = ITEXT_HLP.font_sys_get("arial");
    }

    //функция добавления изображения
    public void AddImage(string imageFilename)
    {
      AddImage(File.ReadAllBytes(imageFilename));
    }

    public void AddImage(byte[] data)
    {
      iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(data);

      pic.Alignment = Element.ALIGN_CENTER;
      if (pic.Height > pic.Width)
      {
        //Maximum height is 800 pixels.
        float percentage = 0.0f;
        percentage = 700 / pic.Height;
        pic.ScalePercent(percentage * 100);
      }
      else
      {
        //Maximum width is 600 pixels.
        float percentage = 0.0f;
        percentage = 540 / pic.Width;
        pic.ScalePercent(percentage * 100);
      }
      //pic.ScaleAbsolute(pic.Width, pic.Height);
      pic.SpacingAfter = 100;
      _document.Add(pic);
    }

    //функция добавления абзаца
    public void AddParagraph(string parText, int spaceBefore, int spaceAfter)
    {
      Font font = new Font(_font);
      iTextSharp.text.Paragraph plotText = new iTextSharp.text.Paragraph(parText, font);
      plotText.SpacingBefore = spaceBefore;
      plotText.SpacingAfter = spaceAfter;
      _document.Add(plotText);
    }


    //функция добавления таблицы
    public void AddTable(double[,] values, string[] headers)
    {
      PdfPTable table = new PdfPTable(headers.Length);
      //iTextSharp.text.Font font = new iTextSharp.text.Font();
      float[] widths = new float[headers.Length];
      for (int i = 0; i < headers.Length; i++)
      {
        PdfPCell cell = new PdfPCell(new Phrase(headers[i], new Font(_font)));
        cell.BackgroundColor = new BaseColor(System.Drawing.Color.BlanchedAlmond);
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(cell);
      }
      int k = values.GetUpperBound(0), m = values.GetUpperBound(1);
      for (int i = 0; i <= k; i++)
      {
        for (int j = 0; j <= m; j++)
        {
          table.AddCell(Math.Round(values[i, j],2).ToString());
        }
      }
      _document.Add(table);
    }

    public void CloseDocument()
    {
      _document.Close();
    }
  }


}
