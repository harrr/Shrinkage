using System;

namespace ShrinkageExplorer.Core.Interfaces
{
  public interface IFilm : IEquatable<IFilm>
  {
    float Width { get; set; }

    float Thickness { get; set; }

    float Temperature { get; set; }

    IMaterial Material { get; set; }

    IFilm FilmAfterRolls(IWorkingRoll currentRoll, IWorkingRoll nextRoll);
  }
}