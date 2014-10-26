using System;

namespace ShrinkageExplorer.Core.Interfaces
{
  /// <summary>
  ///   Класс валка
  /// </summary>
  public interface IGeometryRoll : IEquatable<IGeometryRoll>
  {
    float X { get; set; }

    float Y { get; set; }

    float Radius { get; set; }

    bool Clockwise { get; set; }

    IGeometryRoll Clone();
  }
}