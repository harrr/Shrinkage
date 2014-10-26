using System;

namespace ShrinkageExplorer.Core.DataClasses
{
  /// <summary>
  ///   Класс валка
  /// </summary>
  public class GeometryRoll : IEquatable<GeometryRoll>
  {
    public GeometryRoll()
    {
      
    }

    public GeometryRoll(float x, float y, float radius, bool clockwise = true)
    {
      X = x;
      Y = y;
      Radius = radius;
      ClockwiseRotation = clockwise;
    }

    public GeometryRoll(GeometryRoll roll)
      : this(roll.X, roll.Y, roll.Radius, roll.ClockwiseRotation) { }

    public float X { get; set; }

    public float Y { get; set; }

    public float Radius { get; set; }

    public bool ClockwiseRotation { get; set; }

    

    public override string ToString()
    {
      return String.Format("X = {0}, Y = {1}, Radius = {2}, Rotation -  {3}",
        X, Y, Radius, ClockwiseRotation ? "Clockwise" : "CounterClockwise");
    }

    #region Члены IEquatable<GeometryRoll>

    public bool Equals(GeometryRoll other)
    {
      return X == other.X 
        && Y == other.Y 
        && Radius == other.Radius 
        && ClockwiseRotation == other.ClockwiseRotation;
    }

    #endregion
  }
}