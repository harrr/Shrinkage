using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data
{
  public class WorkingRoll : Roll, IWorkingRoll
  {

    public WorkingRoll(IGeometryRoll roll, IRollDrive drive)
    {
      X = roll.X;
      Y = roll.Y;
      Radius = roll.Radius;
      Clockwise = roll.Clockwise;
      RollDrive = drive;
    }

    public WorkingRoll(IWorkingRoll roll)
      : this(roll, roll.RollDrive) { }

    public WorkingRoll()
    {
      
    }


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


    public IRollDrive RollDrive { get; private set; }

    //public

    public override string ToString()
    {
      return String.Format("{0}, V = {1}, T = {2}", base.ToString(), Velocity, Temperature);
    }

    #region Члены IEquatable<WorkingRoll>

    public bool Equals(IWorkingRoll other)
    {
      return base.Equals(other) && RollDrive.Equals(other.RollDrive);
    }

    #endregion
  }
}
