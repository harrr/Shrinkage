using System.Windows;
using System.Windows.Input;

namespace ShrinkageExplorer.Wpf.Views.Admin
{
  /// <summary>
  /// Description for MainView.
  /// </summary>
  public partial class MainView
  {
    /// <summary>
    /// Initializes a new instance of the MainView class.
    /// </summary>
    public MainView()
    {
      InitializeComponent();
    }

    private void MainView_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      BlurGrid.Visibility = (bool)e.NewValue ? Visibility.Collapsed : Visibility.Visible;
    }

    private void Close_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
      var loginView = new LoginView();
      Application.Current.MainWindow = loginView;
      loginView.Show();
      Close();
    }
  }
}