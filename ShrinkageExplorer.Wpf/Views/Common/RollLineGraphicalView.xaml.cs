using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShrinkageExplorer.Wpf.Views.Common
{
  /// <summary>
  /// Description for GraphicalRollLineView.
  /// </summary>
  public partial class RollLineGraphicalView
  {
    #region MiddlePointProperty

    public Point MiddlePoint
    {
      get { return (Point)GetValue(MiddlePointProperty); }
      set { SetValue(MiddlePointProperty, value); }
    }

    public static readonly DependencyProperty MiddlePointProperty =
      DependencyProperty.Register("MiddlePoint", typeof(Point), typeof(RollLineGraphicalView), new PropertyMetadata());

    #endregion

    protected override Size ArrangeOverride(Size arrangeSize)
    {
      MiddlePoint = new Point(0, arrangeSize.Height);
      double x = 0.0;
      double y = 0.0;
      foreach (var element in InternalChildren.Cast<UIElement>().Where(element => element != null))
      {
        element.RenderTransform = new MatrixTransform(1, 0, 0, -1, 0, 0);
        element.Arrange(new Rect(new Point(MiddlePoint.X + x, MiddlePoint.Y + y), element.DesiredSize));
      }
      return arrangeSize;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
      foreach (var element in InternalChildren.Cast<UIElement>().Where(element => element != null))
      {
        element.Measure(availableSize);
      }
      return new Size();
    }

    public void SaveImage(string path)
    {
      var rtb = new RenderTargetBitmap((int)RenderSize.Width,
        (int)RenderSize.Height, 96d, 96d, PixelFormats.Default);
      rtb.Render(this);

      var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)RenderSize.Width, (int)RenderSize.Height));

      var pngEncoder = new PngBitmapEncoder();
      pngEncoder.Frames.Add(BitmapFrame.Create(crop));

      using (var fs = System.IO.File.OpenWrite(path + ".png"))
      {
        pngEncoder.Save(fs);
      }
    }

    #region Static fields

    private static readonly Pen GridPen = new Pen(Brushes.LightGray, 1);
    private static readonly Pen ArrowPen = new Pen(Brushes.Black, 3);
    private static double _height;
    private static double _width;
    private const double Step = 20;

    private static readonly Typeface Tf = new Typeface(new FontFamily("Times New Roman"), FontStyles.Normal,
      FontWeights.Normal, FontStretches.Normal);

    #endregion

    protected override void OnRender(DrawingContext dc)
    {
      base.OnRender(dc);

      _height = ActualHeight;
      _width = ActualWidth;
      dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, _width, _height));

      for (double x = 0; x < _width; x += Step)
        dc.DrawLine(GridPen, new Point(x, 0), new Point(x, _height));
      for (double y = 0; y < _height; y += Step)
        dc.DrawLine(GridPen, new Point(0, y), new Point(_width, y));


      //Горизонтальная ось
      var axisTop = MiddlePoint.Y;
      dc.DrawLine(ArrowPen, new Point(0, axisTop), new Point(_width, axisTop));
      dc.DrawLine(ArrowPen, new Point(_width, axisTop), new Point(_width - 10, axisTop - 10));
      dc.DrawLine(ArrowPen, new Point(_width, axisTop), new Point(_width - 10, axisTop + 10));
      dc.DrawText(new FormattedText("X, cm",
        CultureInfo.CurrentCulture,
        FlowDirection.LeftToRight,
        Tf, 20, Brushes.Black),
        new Point(_width - 50, axisTop - 30));
      //Вертикальная ось
      var axisLeft = MiddlePoint.X;
      dc.DrawLine(ArrowPen, new Point(axisLeft, 0), new Point(axisLeft, _height));
      dc.DrawLine(ArrowPen, new Point(axisLeft, 0), new Point(axisLeft + 10, 10));
      dc.DrawLine(ArrowPen, new Point(axisLeft, 0), new Point(axisLeft - 10, 10));
      dc.DrawText(new FormattedText("Y, cm",
        CultureInfo.CurrentCulture,
        FlowDirection.LeftToRight,
        Tf, 20, Brushes.Black),
        new Point(axisLeft + 10, 10));
    }

    public RollLineGraphicalView()
    {
      InitializeComponent();
    }


    private void Drive_MouseDown(object sender, MouseButtonEventArgs e)
    {
      /*if (e.ClickCount != 2)
        return;
      var viewModel = (sender as FrameworkElement).DataContext as RollDriveViewModel;
      var editableObject = viewModel.EditableObject;

      editableObject.BeginEdit();
      var view = new DrivePropertiesView
      {
        Owner = Window.GetWindow(this),
        DataContext = viewModel
      };
      view.Owner.IsEnabled = false;
      if (view.ShowDialog() == true)
        editableObject.EndEdit();
      else
        editableObject.CancelEdit();
      view.Owner.IsEnabled = true;*/

    }

    private void Roll_DoubleClick(object sender, MouseButtonEventArgs e)
    {
      /*var viewModel = (sender as FrameworkElement).DataContext as RollViewModel;
      var editableObject = viewModel.EditableObject;

      editableObject.BeginEdit();
      var view = new RollPropertiesView()
      {
        Owner = Window.GetWindow(this),
        DataContext = viewModel
      };
      view.Owner.IsEnabled = false;
      if (view.ShowDialog() == true)
        editableObject.EndEdit();
      else
        editableObject.CancelEdit();
      view.Owner.IsEnabled = true;*/
    }

    private void Roll_DoubleClick(object sender, RoutedEventArgs e)
    {

    }

  }
}