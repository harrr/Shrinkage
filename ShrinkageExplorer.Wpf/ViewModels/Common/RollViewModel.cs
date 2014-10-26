using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Wpf.Messages;
using ShrinkageExplorer.Wpf.Utilities;

namespace ShrinkageExplorer.Wpf.ViewModels.Common
{

  public class RollViewModel : EditableViewModel
  {

    public IWorkingRoll ModelRoll { get; private set; }

    public float X
    {
      get { return ModelRoll.X * 100; }
      set
      {
        if (value == X)
          return;
        ModelRoll.X = value / 100;
        RaisePropertyChanged(() => X);
      }
    }

    public float Y
    {
      get { return ModelRoll.Y * 100; }
      set
      {
        if (value == Y)
          return;
        ModelRoll.Y = value / 100;
        RaisePropertyChanged(() => Y);
      }
    }

    public float Radius
    {
      get { return ModelRoll.Radius * 100; }
      set
      {
        if (value == Radius)
          return;
        ModelRoll.Radius = value / 100;
        RaisePropertyChanged(() => Y);
      }
    }

    public float Velocity
    {
      get { return Drive.Velocity; }
      set
      {
        if (value == Drive.Velocity)
          return;
        Drive.Velocity = value;
        RaisePropertyChanged(() => Velocity);
      }
    }

    public float Temperature
    {
      get { return Drive.Temperature; }
      set
      {
        if (value == Drive.Temperature)
          return;
        Drive.Temperature = value;
        RaisePropertyChanged(() => Temperature);
      }
    }

    public bool ClockwiseRotation
    {
      get { return ModelRoll.Clockwise; }
      set
      {
        if (value == ModelRoll.Clockwise)
          return;
        ModelRoll.Clockwise = value;
        RaisePropertyChanged(() => ClockwiseRotation);
      }
    }

    public RollViewModel(RollViewModel roll)
      : this(roll.ModelRoll, roll.Drive) { }

    public RollViewModel(IWorkingRoll roll, RollDriveViewModel drive)
    {
      ModelRoll = roll;
      Drive = drive;
      EditPropertiesCommand = new RelayCommand(() =>
        MessengerInstance.Send(
          new EditViewModelPropertiesMessage { ViewModel = this },
          MessageTokens.EditViewModelProperties));
    }

    public RollDriveViewModel Drive { get; set; }

    public RelayCommand EditPropertiesCommand { get; private set; }

    public void Refresh()
    {
      RaisePropertyChanged(() => X);
      RaisePropertyChanged(() => Y);
      RaisePropertyChanged(() => Radius);
      RaisePropertyChanged(() => Velocity);
      RaisePropertyChanged(() => Temperature);
      RaisePropertyChanged(() => ClockwiseRotation);
    }

    public override string ToString()
    {
      return ModelRoll.ToString();
    }


    public override IEditableObject EditableObject
    {
      get { return new Caretaker<RollViewModel>(this); }
    }
  }
}
