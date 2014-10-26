
using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace ShrinkageExplorer.Wpf.Utilities
{
  public abstract class EditableViewModel : ViewModelBase
  {
    public abstract IEditableObject EditableObject { get; }

  }
}
