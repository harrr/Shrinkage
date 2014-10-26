using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.ExtensionMethods;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Data.PartialClasses
{
  public class Film : IFilm
  {
    #region Члены IFilm

    public float Width { get; set; }

    public float Thickness { get; set; }

    public float Temperature { get; set; }

    public IMaterial Material { get; set; }

    public IFilm FilmAfterRolls(IWorkingRoll currentRoll, IWorkingRoll nextRoll)
    {
      return new Film
      {
        Width = Width,
        Thickness = (Thickness * currentRoll.GetVelocityRatioWith(nextRoll)),
        Temperature = Temperature,
        Material = Material
      };
    }

    #endregion

    #region Члены IEquatable<IFilm>

    public bool Equals(IFilm other)
    {
      return Material.Equals(other.Material)
             && Width == other.Width
             && Thickness == other.Thickness
             && Temperature == other.Temperature;
    }

    #endregion
  }
}
