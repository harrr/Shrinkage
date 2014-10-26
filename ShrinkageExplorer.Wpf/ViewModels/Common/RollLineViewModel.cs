using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Wpf.Messages;

namespace ShrinkageExplorer.Wpf.ViewModels.Common
{

  public class RollLineViewModel : ViewModelBase, IEquatable<RollLineViewModel>
  {
    public IRollLine ModelLine { get; private set; }

    public ObservableCollection<RollDriveViewModel> Drives { get; private set; }

    public ObservableCollection<RollViewModel> Rolls { get; private set; }

    public string Name
    {
      get { return ModelLine.Name; }
      set
      {
        ModelLine.Name = value;
        RaisePropertyChanged("Name");
      }
    }

    public float LineWidth
    {
      get
      {
        return !Rolls.Any() ? 0 : Rolls.Max(r => r.X + 2 * r.Radius);
      }

    }

    public float LineHeight
    {
      get
      {
        return !Rolls.Any() ? 0 : Rolls.Max(r => r.Y + 2 * r.Radius);
      }
    }

    public RollLineViewModel(IRollLine line)
    {
      ModelLine = line;

      Drives = new ObservableCollection<RollDriveViewModel>(
        ModelLine.Drives.Select(drive =>
        {
          var viewModel = new RollDriveViewModel(drive, this);
          viewModel.Rolls.CollectionChanged += DriveRollsChanged;
          return viewModel;
        }));
      Drives.CollectionChanged += Drives_CollectionChanged;

      Rolls = new ObservableCollection<RollViewModel>(Drives.SelectMany(drive => drive.Rolls));
      Rolls.CollectionChanged += (sender, e) =>
      {
        if (e.Action != NotifyCollectionChangedAction.Remove)
          return;
        e.OldItems.Cast<RollViewModel>().ToList().ForEach(r => r.Drive.RemoveRoll(r));
      };
      foreach (var roll in Rolls)
        roll.PropertyChanged += RefreshWhenChanged;

      InitializeCommands();
    }

    private void InitializeCommands()
    {
      RemoveRollCommand = new RelayCommand<RollViewModel>(RemoveRoll);

      RemoveDriveCommand = new RelayCommand<RollDriveViewModel>(RemoveDrive, drive => drive != null);

      AttachDriveCommand = new RelayCommand(() =>
        MessengerInstance.Send(
          new AttachNewDriveMessage { Line = this },
          MessageTokens.AttachNewDrive));
    }


    void Drives_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          foreach (RollDriveViewModel drive in e.NewItems)
          {
            drive.Rolls.CollectionChanged += DriveRollsChanged;
            ModelLine.Drives.Add(drive.ModelDrive);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          foreach (RollDriveViewModel drive in e.OldItems)
          {
            drive.Rolls.CollectionChanged -= DriveRollsChanged;
            ModelLine.Drives.Remove(drive.ModelDrive);
          }
          break;
        case NotifyCollectionChangedAction.Move:
          {
            var drives = ModelLine.Drives;
            var drive = drives[e.OldStartingIndex];
            drives.Remove(drive);
            drives.Insert(e.NewStartingIndex, drive);
          }
          break;
      }
    }

    private void DriveRollsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          foreach (RollViewModel roll in e.NewItems)
          {
            roll.PropertyChanged += RefreshWhenChanged;
            Rolls.Add(roll);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          foreach (RollViewModel roll in e.OldItems)
          {
            roll.PropertyChanged -= RefreshWhenChanged;
            Rolls.Remove(roll);
          }
          break;
      }
      Refresh();
    }

    public void RemoveRoll(RollViewModel roll)
    {
      var drive = roll.Drive;
      drive.RemoveRoll(roll);
      if (drive.Rolls.Count == 0)
        Drives.Remove(drive);
    }

    public void RemoveDrive(RollDriveViewModel drive)
    {
      foreach (var roll in drive.Rolls.ToList())
        drive.Rolls.Remove(roll);
      Drives.Remove(drive);
    }

    public void MoveDrive(int oldPosition, int newPosition)
    {
      Drives.Move(oldPosition, newPosition);
      for (int i = 0; i < Drives.Count; ++i)
      {
        var drive = Drives[i];
        if (drive.Number == i + 1)
          continue;
        drive.Number = i + 1;
      }
      Rolls = new ObservableCollection<RollViewModel>(Drives.SelectMany(drive => drive.Rolls));
    }

    public void AddDrive(RollDriveViewModel drive)
    {
      Drives.Add(drive);
    }

    public void AddRoll(RollViewModel roll, RollDriveViewModel drive)
    {
      drive.AddRoll(roll);
    }

    private void RefreshWhenChanged(object sender, EventArgs args)
    {
      Refresh();
    }

    private void Refresh()
    {
      RaisePropertyChanged("Rolls");
      RaisePropertyChanged("LineWidth");
      RaisePropertyChanged("LineHeight");
    }


    public RelayCommand<RollViewModel> RemoveRollCommand { get; private set; }

    public RelayCommand<RollDriveViewModel> RemoveDriveCommand { get; private set; }

    public RelayCommand AttachDriveCommand { get; private set; }

    public bool Equals(RollLineViewModel other)
    {
      return ModelLine.Equals(other.ModelLine);
    }



  }
}
