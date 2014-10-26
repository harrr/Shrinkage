using System.Windows;
using System.Windows.Controls;
namespace ShrinkageExplorer.Wpf.Views.Operator
{
  /// <summary>
  /// Description for OptimizationView.
  /// </summary>
  public partial class OptimizationView
  {
    /// <summary>
    /// Initializes a new instance of the OptimizationView class.
    /// </summary>
    public OptimizationView()
    {
      InitializeComponent();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      var textBox = sender as TextBox;
      textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
    }
  }
}