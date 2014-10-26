using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using pdfWriter;
using ShrinkageExplorer.Data;
using ShrinkageExplorer.Wpf.ViewModels.Common;
using ShrinkageExplorer.Wpf.ViewModels.Operator;

namespace ShrinkageExplorer.Wpf.Views.Operator
{
  /// <summary>
  /// This application's main window.
  /// </summary>
  public partial class MainView
  {
    private int _loadedLineCounter;
    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// </summary>
    public MainView()
    {
      InitializeComponent();
      Closing += (s, e) => ViewModelLocator.Cleanup();
    }

    private void MainView_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      BlurGrid.Visibility = (bool)e.NewValue ? Visibility.Collapsed : Visibility.Visible;
    }

    private void Close_OnExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private bool PrepareForReport(OperatorUiViewModel dataContext)
    {
      if (dataContext.Model.CalculationResults == null)
      {
        MessageBox.Show("You have to evaluate model to save report");
        return false;
      }
      var image = ResultsView.ChartImage;
      if (image == null)
      {
        MessageBox.Show("You have to open the chart tab");
        return false;
      }
      return true;
    }

    private void PdfReport_Click(object sender, RoutedEventArgs e)
    {
      var dataContext = DataContext as OperatorUiViewModel;
      if (!PrepareForReport(dataContext))
        return;
      var sfd = new SaveFileDialog
      {
        AddExtension = true,
        DefaultExt = "pdf",
        Filter = "Pdf files|*.pdf"
      };

      if (sfd.ShowDialog() != true) return;

      var shrinkages = dataContext.Model.ResultsToChart.Values.ToArray();
      var rolls = dataContext.Line.Drives.SelectMany(d => d.Rolls).ToArray();
      PdfGenerator.GenerateReport(sfd.FileName,
        rolls.Select(r => r.Temperature).ToArray(),
        rolls.Select(r => (float)Math.Round(r.Velocity)).ToArray(),
        shrinkages,
        ResultsView.ChartImage);
      MessageBox.Show("Report saved successfully");
    }

    private void WordReport_Click(object sender, RoutedEventArgs e)
    {
      var dataContext = DataContext as OperatorUiViewModel;
      if (!PrepareForReport(dataContext))
        return;

      var shrinkages = dataContext.Model.ResultsToChart.Values.ToArray();
      var rolls = dataContext.Line.Drives.SelectMany(d => d.Rolls).ToArray();
      WordReportGenerator.GenerateReport(
        rolls.Select(r => r.Temperature).ToArray(),
        rolls.Select(r => (float)Math.Round(r.Velocity)).ToArray(),
        shrinkages,
        ResultsView.ChartImage);
      MessageBox.Show("Report saved successfully");
    }

    private void ExcelReport_Click(object sender, RoutedEventArgs e)
    {
      var dataContext = DataContext as OperatorUiViewModel;
        
      if (!PrepareForReport(dataContext))
        return;

      var shrinkages = dataContext.Model.ResultsToChart.Values.ToArray();
      var rolls = dataContext.Line.Drives.SelectMany(d => d.Rolls).ToArray();
      ExcelReportGenerator.GenerateReport(
        rolls.Select(r => r.Temperature).ToArray(),
        rolls.Select(r => (float)Math.Round(r.Velocity)).ToArray(),
        shrinkages,
        ResultsView.ChartImage);
      MessageBox.Show("Report saved successfully");
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
      var loginView = new LoginView();
      Application.Current.MainWindow = loginView;
      loginView.Show();
      Close();
    }

    private void LoadLine_Click(object sender, RoutedEventArgs e)
    {
      var dataContext = DataContext as OperatorUiViewModel;
      var ofd = new OpenFileDialog
      {
        CheckFileExists = true,
        DefaultExt = "sel",
        AddExtension = true,
        Filter = "Shrinkage Explorer line file|*.sel"
      };
      if (ofd.ShowDialog() != true) return;
      var line = LineLoader.Load(ofd.FileName);
      if (line == null)
      {
          MessageBox.Show("Unable to load line from file!");
      }
      else
      {
          line.Name = "Loaded line #" + (++_loadedLineCounter);
          dataContext.Lines.Add(new RollLineViewModel(line));
      }
    }

    private void SaveLine_Click(object sender, RoutedEventArgs e)
    {
      var dataContext = DataContext as OperatorUiViewModel;
      var sfd = new SaveFileDialog
      {
        DefaultExt = "sel",
        AddExtension = true,
        Filter = "Shrinkage Explorer line file|*.sel"
      };
      if (sfd.ShowDialog() != true) return;
      var saver = new LineSaver(dataContext.Line.ModelLine, sfd.FileName);
      saver.Save();
    }
  }
}