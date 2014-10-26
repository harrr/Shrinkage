using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.ExtensionMethods;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Wpf.Messages;
using ShrinkageExplorer.Wpf.Utilities;

namespace ShrinkageExplorer.Wpf.ViewModels.Common
{

  public class RollDriveViewModel : EditableViewModel
  {
    private readonly ObservableCollection<RollViewModel> _rolls;

    public RollDriveViewModel(IRollDrive rollDrive, RollLineViewModel lineViewModel)
    {
      LineViewModel = lineViewModel;
      ModelDrive = rollDrive;
      _rolls = new ObservableCollection<RollViewModel>(ModelDrive
        .WorkingRolls
        .Select(r =>
        {
          var roll = new RollViewModel(r, this);
          roll.PropertyChanged += RefreshWhenChanged;
          return roll;
        }));
      //Rolls = new ReadOnlyObservableCollection<RollViewModel>(_rolls);

      _rolls.CollectionChanged += RefreshWhenChanged;
      _rolls.CollectionChanged += RollsCollectionChanged;

      EditPropertiesCommand = new RelayCommand(() =>
        MessengerInstance.Send(
          new EditViewModelPropertiesMessage { ViewModel = this },
          MessageTokens.EditViewModelProperties));

      AttachRollCommand = new RelayCommand(() =>
        MessengerInstance.Send(
          new AttachNewRollMessage { Drive = this },
          MessageTokens.AttachNewRoll));

      RemoveRollCommand = new RelayCommand<RollViewModel>(RemoveRoll);
    }

    private void RollsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          foreach (RollViewModel roll in e.NewItems)
          {
            roll.PropertyChanged += RefreshWhenChanged;
            ModelDrive.AddRoll(roll.ModelRoll);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          foreach (RollViewModel roll in e.OldItems)
          {
            roll.PropertyChanged -= RefreshWhenChanged;
            ModelDrive.RemoveRoll(roll.ModelRoll);
          }
          break;
      }
    }

    private void RefreshWhenChanged(object sender, EventArgs e)
    {
      Refresh();
    }

    public IRollDrive ModelDrive { get; private set; }

    public RollLineViewModel LineViewModel { get; private set; }

    public int Number
    {
      get { return ModelDrive.Number; }
      set
      {
        if (value == ModelDrive.Number)
          return;
        RaisePropertyChanging("Number");
        ModelDrive.Number = value;
        RaisePropertyChanged("Number");
      }
    }

    public float Temperature
    {
      get { return ModelDrive.Temperature; }
      set
      {
        if (value == ModelDrive.Temperature)
          return;
        ModelDrive.Temperature = value;
        RaisePropertyChanged(() => Temperature);

        foreach (var roll in _rolls)
          roll.Refresh();
      }
    }

    public float MinTemperature
    {
      get { return ModelDrive.MinTemperature; }
      set
      {
        if (value == ModelDrive.MinTemperature)
          return;
        ModelDrive.MinTemperature = value;
        RaisePropertyChanged(() => MinTemperature);
      }
    }

    public float MaxTemperature
    {
      get { return ModelDrive.MaxTemperature; }
      set
      {
        if (value == ModelDrive.MaxTemperature)
          return;
        ModelDrive.MaxTemperature = value;
        RaisePropertyChanged(() => MaxTemperature);
      }
    }

    public float Velocity
    {
      get { return ModelDrive.Velocity * 60; }
      set
      {
        if (value == ModelDrive.Velocity)
          return;
        ModelDrive.Velocity = value / 60;
        RaisePropertyChanged(() => Velocity);

        foreach (var roll in _rolls)
          roll.Refresh();
      }
    }

    public float MinVelocity
    {
      get { return ModelDrive.MinVelocity * 60; }
      set
      {
        if (value == ModelDrive.MinVelocity)
          return;
        ModelDrive.MinVelocity = value / 60;
        RaisePropertyChanged(() => MinVelocity);
      }
    }

    public float MaxVelocity
    {
      get { return ModelDrive.MaxVelocity * 60; }
      set
      {
        if (value == ModelDrive.MaxVelocity)
          return;
        ModelDrive.MaxVelocity = value / 60;
        RaisePropertyChanged(() => MaxVelocity);
      }
    }

    public float Left
    {
      get
      {
        if (_rolls.Count == 0)
          return 0;
        return _rolls.Min(r => r.X) - 3;
      }
    }

    public float Right
    {
      get
      {
        if (_rolls.Count == 0)
          return 0;
        return _rolls.Max(r => r.X) + 3;
      }
    }

    public ObservableCollection<RollViewModel> Rolls { get { return _rolls; } }

    public void Refresh()
    {
      RaisePropertyChanged(() => Left);
      RaisePropertyChanged(() => Right);
    }

    public void AddRoll(RollViewModel roll)
    {
      roll.Drive = this;
      _rolls.Add(roll);
    }

    public void RemoveRoll(RollViewModel roll)
    {
      _rolls.Remove(roll);
    }

    public override IEditableObject EditableObject
    {
      get
      {
        return new Caretaker<RollDriveViewModel>(this);
      }
    }

    public RelayCommand AttachRollCommand { get; private set; }

    public RelayCommand EditPropertiesCommand { get; private set; }

    public RelayCommand<RollViewModel> RemoveRollCommand { get; private set; }


    public override string ToString()
    {
      return String.Format("{0} {1} {2}", Number, Velocity, Temperature);
    }
  }
}