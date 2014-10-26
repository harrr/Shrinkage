using System;
using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.ExtensionMethods;

namespace ShrinkageExplorer.Core.DataClasses
{
  /// <summary>
  ///   Класс привода валка
  /// </summary>
  public class RollDrive : IEquatable<RollDrive>
  {
    private IEnumerable<GeometryRoll> _geometryRolls;

    public RollDrive()
    {
      GeometryRolls = new GeometryRoll[] { };
    }


    public RollDrive(IEnumerable<GeometryRoll> rolls)
    {
      WorkingRolls = rolls.Select(roll => new WorkingRoll(roll, this)).ToList();
    }

    public RollDrive(RollDrive drive)
    {
      Temperature = drive.Temperature;
      Velocity = drive.Velocity;
      GeometryRolls = drive.GeometryRolls.Select(roll => new GeometryRoll(roll));

      MinTemperature = drive.MinTemperature;
      MaxTemperature = drive.MaxTemperature;
      MinVelocity = drive.MinVelocity;
      MaxVelocity = drive.MaxVelocity;

      Number = drive.Number;
    }

    public int Number { get; set; }

    public float Temperature { get; set; }

    public float Velocity { get; set; }

    public float MinVelocity { get; set; }

    public float MaxVelocity { get; set; }

    public float MinTemperature { get; set; }

    public float MaxTemperature { get; set; }

    public IEnumerable<GeometryRoll> GeometryRolls
    {
      get { return _geometryRolls; }
      set
      {
        _geometryRolls = value;
        WorkingRolls = value == null
          ? new List<WorkingRoll>()
          : value.Select(roll => new WorkingRoll(roll, this)).ToList();
      }
    }

    public IList<WorkingRoll> WorkingRolls { get; private set; }

    

    

    #region Члены IEquatable<RollDrive>

    public bool Equals(RollDrive other)
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