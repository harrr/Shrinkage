using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace pdfWriter
{
  public class PdfGenerator
  {
    public static void GenerateReport(string fileName, float[] temperatures, float[] speeds, float[] shrinkages, byte[] graphBytes)
    {
      //класс PDFWriter используется для добавления 
      //элементов в файл *pdf
      //классы METRIC, ITEXT_HLP для того, чтобы отображалась кирилица
      string exePath = AppDomain.CurrentDomain.BaseDirectory;
      string pdfFilename = exePath + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_"
              + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_"
              + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".pdf";
      var writer = new PDFWriter(fileName);

      try
      {
        //добавление логотипа
        string image = exePath + "Logo.jpg";
        writer.AddImage(image);
        //добавление текста
        writer.AddParagraph("Experiment data", 2, 2);
        writer.AddImage(graphBytes);
        string[] tableHeaders = new string[4];
        tableHeaders[0] = "#";
        //tableHeaders[1] = "Материал";
        tableHeaders[1] = "Roll temperature, ℃";
        tableHeaders[2] = "Roll velocity, m/min";
        tableHeaders[3] = "Length shrinkage, %";

        var rowCount = temperatures.Length;
        double[,] vDoubles = new double[rowCount-3, 4];
        for (int i = 3; i < rowCount; i++)
        {
          var j = i - 3;
          vDoubles[j, 0] = i + 1;
          vDoubles[j, 1] = temperatures[i];
          vDoubles[j, 2] = speeds[i];
          vDoubles[j, 3] = Math.Round(shrinkages[j], 2);
        }
        //Добавление таблицы
        writer.AddTable(vDoubles, tableHeaders);

        writer.CloseDocument();
      }
      catch (IOException ioe)
      {
        Debug.WriteLine(ioe.Message);
        File.Delete(pdfFilename);
        //MessageBox.Show(ioe.Message, "Unable to create file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
