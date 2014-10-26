using System;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.DataClasses
{
  public class WorkingRoll : GeometryRoll, IEquatable<WorkingRoll>
  {

    public WorkingRoll(GeometryRoll roll, RollDrive drive)
      : base(roll)
    {
      RollDrive = drive;
    }

    public WorkingRoll(WorkingRoll roll)
      : this(roll, roll.RollDrive) { }


    public float Temperature
    {
      get { return RollDrive.Temperature; }
      set { RollDrive.Temperature = value; }
    }

    public float Velocity
    {
      get { return RollDrive.Velocity; }
      set { RollDrive.Velocity = value; }
    }


    public RollDrive RollDrive { get; private set; }

    //public

    public override string ToString()
    {
      return String.Format("{0}, V = {1}, T = {2}", base.ToString(), Velocity, Temperature);
    }

    #region Члены IEquatable<WorkingRoll>

    public bool Equals(WorkingRoll other)
    {
      return base.Equals(other) && RollDrive.Equals(other.RollDrive);
    }

    #endregion
  }
}