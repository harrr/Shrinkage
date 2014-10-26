using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ShrinkageExplorer.Wpf.ViewModels.Common;

namespace ShrinkageExplorer.Wpf.Views.Common
{
  /// <summary>
  /// Description for RollLineTabledView.
  /// </summary>
  public partial class RollLineTabledView
  {
    private ICollectionView _collectionView;

    public bool IsReadOnly
    {
      get { return (bool)GetValue(IsReadOnlyProperty); }
      set { SetValue(IsReadOnlyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RollLineTabledView), new PropertyMetadata(false));


    public RollLineTabledView()
    {
      InitializeComponent();
    }

    private void RollLineTabledView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      var dataContext = e.NewValue as RollLineViewModel;
      if (dataContext == null)
        return;
      _collectionView = CollectionViewSource.GetDefaultView(dataContext.Rolls);
      _collectionView.GroupDescriptions.Clear();
      _collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Drive"));
    }

  }
}