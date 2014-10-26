using System.ComponentModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using ShrinkageExplorer.Core;
using ShrinkageExplorer.Wpf.Messages;
using ShrinkageExplorer.Wpf.Utilities;
using ShrinkageExplorer.Wpf.ViewModels.Common;
using ShrinkageExplorer.Wpf.Views;

namespace ShrinkageExplorer.Wpf
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App
  {
    private readonly IDialogService _dialogService;

    public App()
    {
      RegisterMessages();
      _dialogService = ViewModelLocator.DialogService;
    }



    static App()
    {
      DispatcherHelper.Initialize();
    }

    private void RegisterMessages()
    {
      Messenger.Default.Register<EditViewModelPropertiesMessage>(
        this, MessageTokens.EditViewModelProperties, EditViewModelMessageReceived);

      Messenger.Default.Register<AttachNewRollMessage>(
        this, MessageTokens.AttachNewRoll, AttachNewRollReceived);

      Messenger.Default.Register<AttachNewDriveMessage>(
        this, MessageTokens.AttachNewDrive, AttachNewDriveReceived);
    }

    private void AttachNewRollReceived(AttachNewRollMessage message)
    {
      var drive = message.Drive;
      var newRoll = new RollViewModel(drive.ModelDrive.CreateWorkingRoll(), drive);
      if (EditObject(newRoll))
        drive.AddRoll(newRoll);
    }

    private void AttachNewDriveReceived(AttachNewDriveMessage message)
    {
      var line = message.Line;
      var newNumber = 1;

      var lastDrive = line.Drives.LastOrDefault();
      if (lastDrive != null)
        newNumber = lastDrive.Number + 1;
      var newDrive = new RollDriveViewModel(line.ModelLine.CreateDrive(newNumber), line);
      line.AddDrive(newDrive);
      if (!(EditObject(newDrive) && newDrive.Rolls.Count > 0))
        line.RemoveDrive(newDrive);
    }

    private void EditViewModelMessageReceived(EditViewModelPropertiesMessage message)
    {
      EditObject(message.ViewModel);
    }

    private bool EditObject(EditableViewModel viewModel)
    {
      var editableObject = viewModel.EditableObject;
      editableObject.BeginEdit();
      bool isEditingCompleted = false;
      try
      {
        isEditingCompleted = _dialogService.ShowDialog(viewModel) == true;
      }
      catch (ShrinkageExplorerException exc)
      {
        MessageBox.Show(exc.Message);
      }
      if (isEditingCompleted)
        editableObject.EndEdit();
      else
        editableObject.CancelEdit();
      return isEditingCompleted;
    }

  }
}
