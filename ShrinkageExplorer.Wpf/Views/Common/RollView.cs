using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Threading;
using ShrinkageExplorer.Wpf.Utilities;
using ShrinkageExplorer.Wpf.ViewModels.Common;

namespace ShrinkageExplorer.Wpf.Views.Common
{
  public class RollView : Control
  {
    private bool _captured;
    private Vector _delta;
    private float _xMoving;
    private float _yMoving;
    //private const float UnitsInMeter = 100; // 100 for cm, 10 for dm, 1 for m
    private const float Step = 5;



    public RollViewModel Roll
    {
      get { return (RollViewModel)GetValue(RollProperty); }
      set { SetValue(RollProperty, value); }
    }

    public static readonly DependencyProperty RollProperty =
      DependencyProperty.Register("Roll", typeof(RollViewModel), typeof(RollView),
        new FrameworkPropertyMetadata(RollChanged));

    private static void RollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (e.NewValue as RollViewModel).PropertyChanged += (sender, args) => (d as FrameworkElement).InvalidateVisual();
    }




    static RollView()
    {
      WallBrushProperty = DependencyProperty.Register("WallBrush", typeof(Brush), typeof(RollLineGraphicalView),
        new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.AffectsRender));

    }

    public RollView()
    {
    }

    public RollView(RollViewModel roll)
    {
      Roll = roll;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (e.ClickCount > 1)
        return;
      base.OnMouseLeftButtonDown(e);
      var canvas = VisualParent as Canvas;
      if (canvas == null) return;
      WallBrush = Brushes.Red;
      var startPoint = new Point
      {
        X = e.GetPosition(canvas).X,
        Y = e.GetPosition(canvas).Y
      };

      _delta = (startPoint - (new Point(Roll.X, Roll.Y)));
      Mouse.Capture(this);
      _captured = true;
      _xMoving = Roll.X;
      _yMoving = Roll.Y;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      var canvas = VisualParent as Canvas;
      if (canvas == null) return;
      base.OnMouseLeftButtonUp(e);
      WallBrush = Brushes.LightGray;
      Mouse.Capture(null);
      _captured = false;
      InvalidateVisual();
      Roll.X = _xMoving;
      Roll.Y = _yMoving;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      var canvas = VisualParent as Canvas;
      if (canvas == null) return;
      var curPoint = new Point();
      if (!_captured) return;
      Mouse.Capture(this);
      curPoint.X = e.GetPosition(canvas).X;
      curPoint.Y = e.GetPosition(canvas).Y;

      _xMoving = (float)(Math.Round((curPoint.X + _delta.X) / Step) * Step);
      _yMoving = (float)(Math.Round((curPoint.Y + _delta.Y) / Step) * Step);

      InvalidateVisual();
    }

    public Brush WallBrush
    {
      get { return (Brush)GetValue(WallBrushProperty); }
      set { SetValue(WallBrushProperty, value); }
    }

    public static readonly DependencyProperty WallBrushProperty;

    private static readonly Pen ArrowPen = new Pen(Brushes.Black, 1);
    private static readonly Pen NormalPen = new Pen(Brushes.Black, 1);
    private static readonly Pen DashPen = new Pen(Brushes.Red, 1) { DashStyle = DashStyles.DashDot };

    protected override void OnRender(DrawingContext drawingContext)
    {
      var pen = NormalPen;
      double x = _captured ? _xMoving : Roll.X;
      double y = _captured ? _yMoving : Roll.Y;
      double r = Roll.Radius;
      base.OnRender(drawingContext);
      drawingContext.DrawEllipse(WallBrush, pen, new Point(x, y), r, r);
      drawingContext.DrawEllipse(Brushes.White, pen, new Point(x, y), r - r * 0.2, r - r * 0.2);

      var theta = 0; 
      if (!Roll.ClockwiseRotation)
      {
        drawingContext.DrawArc(ArrowPen, null,
          new Rect(new Point(x - r / 2, y - r / 2),
            new Point(x + r / 2, y + r / 2)), 0, 180);
        double x1 = x - r * 0.5 * Math.Cos(theta);
        double y1 = y - r * 0.5 * Math.Sin(theta);
        double x2 = x - r * 0.25 * Math.Cos(theta);
        double y2 = y - r * 0.25 * Math.Sin(theta);
        double x3 = x - r * 0.75 * Math.Cos(-Math.PI / 6 + theta);
        double y3 = y - r * 0.75 * Math.Sin(-Math.PI / 6 + theta);
        drawingContext.DrawLine(ArrowPen, new Point(x1, y1), new Point(x2, y2));
        drawingContext.DrawLine(ArrowPen, new Point(x1, y1), new Point(x3, y3));
      }
      else
      {
        drawingContext.DrawArc(ArrowPen, null,
          new Rect(new Point(x - r / 2, y - r / 2),
            new Point(x + r / 2, y + r / 2)), 0, 180);

        double x1 = x + r * 0.5 * Math.Cos(theta);
        double y1 = y + r * 0.5 * Math.Sin(-theta);
        double x2 = x + r * 0.25 * Math.Cos(theta);
        double y2 = y + r * 0.25 * Math.Sin(-theta);
        double x3 = x + r * 0.75 * Math.Sin(Math.PI / 3 + theta);
        double y3 = y + r * 0.75 * Math.Cos(Math.PI / 3 + theta);
        drawingContext.DrawLine(ArrowPen, new Point(x1, y1), new Point(x2, y2));
        drawingContext.DrawLine(ArrowPen, new Point(x1, y1), new Point(x3, y3));
      }
      if (!_captured) return;
      const double tripleStep = Step * 3;
      drawingContext.DrawLine(DashPen, new Point(x - r - tripleStep, y), new Point(x + r + tripleStep, y));
      drawingContext.DrawLine(DashPen, new Point(x, y - r - tripleStep), new Point(x, y + r + tripleStep));
    }
  }
}
