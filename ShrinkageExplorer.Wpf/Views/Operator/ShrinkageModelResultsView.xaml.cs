using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ShrinkageExplorer.Wpf.ViewModels.Operator;

namespace ShrinkageExplorer.Wpf.Views.Operator
{
  /// <summary>
  /// Description for ShrinkageModelResultsView.
  /// </summary>
  public partial class ShrinkageModelResultsView
  {
    /// <summary>
    /// Initializes a new instance of the ShrinkageModelResultsView class.
    /// </summary>
    public ShrinkageModelResultsView()
    {
      InitializeComponent();
    }

    private void RollNumberComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Chart.ItemsSource = (DataContext as ShrinkageModelViewModel)
        .ShrinkageOfRoll[Convert.ToInt32(e.AddedItems[0])];
    }

    public byte[] ChartImage
    {
      get
      {
        if (LineSeries.ActualWidth == 0)
          return null;
        var targetBitmap = new RenderTargetBitmap((int)chart.ActualWidth,
                           (int)chart.ActualHeight,
                           96d, 96d,
                           PixelFormats.Default);
        targetBitmap.Render(chart);

        // add the RenderTargetBitmap to a Bitmapencoder
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

        var ms = new MemoryStream();
        encoder.Save(ms);
        return ms.ToArray();
      }
    }


  }
}