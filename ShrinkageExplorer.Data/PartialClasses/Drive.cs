
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Data;
namespace ShrinkageExplorer.Data
{
  public partial class Drive : IRollDrive
  {

    #region Члены IRollDrive

    public ICollection<IGeometryRoll> GeometryRolls
    {
      get
      {
        return Rolls;
      }
    }

    public IWorkingRoll CreateWorkingRoll(IGeometryRoll roll)
    {
      return new WorkingRoll(roll, this);
    }

    public IWorkingRoll CreateWorkingRoll()
    {
      return new WorkingRoll();
    }

    public IList<IWorkingRoll> WorkingRolls
    {
      get
      {
        var collection = new ObservableCollection<IWorkingRoll>(GeometryRolls.Select(CreateWorkingRoll));
        collection.CollectionChanged += collection_CollectionChanged;
        return collection;
      }
    }

    void collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          e.NewItems.Cast<IWorkingRoll>().ToList().ForEach(roll => GeometryRolls.Add(
          new Roll
          {
            Drive = this,
            Radius = roll.Radius,
            X = roll.X,
            Y = roll.Y,
            Clockwise =roll.Clockwise,
          }
        ));
          break;
        case NotifyCollectionChangedAction.Remove:
          e.OldItems.Cast<IWorkingRoll>().ToList().ForEach(roll => 
            GeometryRolls.Remove(GeometryRolls.First(r =>r.X == roll.X && r.Y == roll.Y)));
          break;
      }
    }

    #endregion

    public IRollDrive Clone()
    {
      return new Drive
      {
        Temperature = Temperature,
        Velocity = Velocity,
        Rolls = GeometryRolls.Select(roll => roll.Clone()).ToList(),

        MinTemperature = MinTemperature,
        MaxTemperature = MaxTemperature,
        MinVelocity = MinVelocity,
        MaxVelocity = MaxVelocity,

        Number = Number
      };
    }

    #region Члены IEquatable<IRollDrive>

    public bool Equals(IRollDrive other)
    {
      if (Velocity != other.Velocity)
        return false;
      if (Temperature != other.Temperature)
        return false;
      return !GeometryRolls.Where((t, i) => !t.Equals(other.GeometryRolls.ElementAt(i))).Any();
    }

    #endregion


  }
}
