using System;

namespace ShrinkageExplorer.Core.DataClasses
{
  public class Film : IEquatable<Film>
  {
    public float Width { get; set; }
    public float Thickness { get; set; }
    public float Temperature { get; set; }
    public Material Material { get; set; }

    public Film(float width, float thickness, float temperature, Material material)
    {
      Width = width;
      Thickness = thickness;
      Temperature = temperature;
      Material = material;
    }

    public Film(Film film)
      : this(film.Width, film.Thickness, film.Temperature, film.Material) { }

    public Film() { }

    public Film FilmAfterRolls(WorkingRoll currentRoll, WorkingRoll nextRoll)
    {
      return new Film(Width, (Thickness * currentRoll.GetVelocityRatioWith(nextRoll)), Temperature, Material);
    }

    public override string ToString()
    {
      return String.Format("{0} {1}x{2}", Material.Name, Width, Thickness);
    }

    public bool Equals(Film other)
    {
      return Material.Equals(other.Material)
             && Width == other.Width
             && Thickness == other.Thickness
             && Temperature == other.Temperature;
    }
  }
}