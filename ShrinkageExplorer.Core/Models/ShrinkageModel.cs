using System;
using System.Collections.Generic;
using ShrinkageExplorer.Core.ExtensionMethods;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Models
{
  [Serializable]
  public class ModelException : Exception
  {
    public ModelException(string message)
      : base(message)
    { }
    public ModelException()
      : base()
    { }
  }
  public struct ShrinkageResult
  {
    public double Sl { get; set; }
    public double Sw { get; set; }
    public double Sh { get; set; }

    public ShrinkageResult(double sl, double sw, double sh)
      : this()
    {
      Sl = sl;
      Sw = sw;
      Sh = sh;
    }
    public override string ToString()
    {
      return String.Format("Length shrinkage={0:P}\nWidth shrinkage={1:P}\nHeight shrinkage={2:P}", Sl, Sw, Sh);

    }
  }
  public abstract class ShrinkageModel
  {
    protected struct Length
    {
      public double l;
      public double L;
    }

    public IFilm IFilm { get; protected set; }

    public IRollLine Line { get; protected set; }

    public List<IWorkingRoll> Rolls { get; protected set; }

    protected ShrinkageModel(){}

    public abstract ShrinkageResult[] Calculate(IRollLine line, IFilm film);

    public abstract string ModelName { get; }

    protected IFilm[] Films()
    {
      var result = new IFilm[Rolls.Count];
      result[0] = IFilm;
      var currentFilm = IFilm;
      for (var i = 0; i < result.Length - 1; ++i)
        currentFilm = result[i + 1] = currentFilm.FilmAfterRolls(Rolls[i], Rolls[i + 1]);
      return result;
    }
    protected double[] Es()
    {
      var result = new double[Rolls.Count * 2];
      var k = 0;
      for (var i = 3; i < Rolls.Count - 1; ++i)
      {
        result[k++] = Rolls[i + 1].Velocity / Rolls[i].Velocity;
        result[k++] = 1;
      }
      //result[result.Length - 1] = 1;
      return result;
    }

    /*protected double[] Ls()
    {
        double[] result = new double[line.Count - 1];
        IFilm[] films = Films();
        for (int i = 0; i < result.Length; ++i)
        {
            ModelRoll current_roll = line[i];
            ModelRoll next_roll = line[i+1];

            IFilm current_film = films[i];
            IFilm next_film = films[i+1];
            double S = current_roll.GetDistanceBetweenCentersWith(next_roll);
            double f1 = Math.Atan(Math.Sqrt(Math.Pow(S / (current_roll.Radius + next_roll.Radius - current_film.Thickness/2 - next_film.Thickness ), 2)-1));
            double f2 = Math.Atan((next_roll.Y - current_roll.Y) / (next_roll.X - current_roll.X));
            double f = Math.PI / 2 - f1 + f2;
            double deltaX = current_roll.X + (current_roll.Radius + current_film.Thickness/2) * Math.Sin(f) - next_roll.X;
            //double deltaX = (current_roll.Radius + current_film.Thickness) * Math.Sin(f) - next_roll.X;
            double deltaY = current_roll.Y - (current_roll.Radius + current_film.Thickness/2 ) * Math.Cos(f) - next_roll.Y;
            //double deltaY = current_roll.Radius + current_film.Thickness / 2 - (current_roll.Radius + current_film.Thickness / 2) * Math.Cos(f) - next_roll.Y;
            result[i] = Math.Sqrt(deltaX * deltaX + deltaY * deltaY - Math.Pow(next_roll.Radius + next_film.Thickness, 2));
        }
        return result;
    }*/

    protected Length[] Ls()
    {
      double Alpha, Beta;
      Length[] lengths = new Length[Rolls.Count - 1];
      double[] F_Angle = new double[Rolls.Count];
      double[] G_Angle = new double[Rolls.Count];

      double C0_C1, C1_C2, C0_C2;
      double C1_D0, C0_D0;

      double A1_D0, B0_D0;

      for (int i = 0; i < Rolls.Count - 1; i++)
      {
        var current_roll = Rolls[i];
        var next_roll = Rolls[i + 1];
        C0_C1 = current_roll.GetDistanceBetweenCentersWith(next_roll);

        Alpha = current_roll.Radius / next_roll.Radius;

        C1_D0 = C0_C1 / (1 + Alpha);
        C0_D0 = C0_C1 - C1_D0;

        B0_D0 = Math.Pow(Math.Abs(C0_D0 * C0_D0 - current_roll.Radius * current_roll.Radius), 0.5);
        A1_D0 = Math.Pow(Math.Abs(C1_D0 * C1_D0 - next_roll.Radius * next_roll.Radius), 0.5);

        lengths[i].l = B0_D0 + A1_D0;

        F_Angle[i] = Math.Atan(B0_D0 / current_roll.Radius);
        //if (current_roll.Clockwise == next_roll.Clockwise)
        {
          lengths[i].l = C0_C1;
          F_Angle[i] = Math.PI / 2;
        }
      }

      for (int i = 0; i < Rolls.Count - 2; i++)
      {
        C0_C1 = Rolls[i].GetDistanceBetweenCentersWith(Rolls[i + 1]);
        C1_C2 = Rolls[i + 1].GetDistanceBetweenCentersWith(Rolls[i + 2]);
        C0_C2 = Rolls[i].GetDistanceBetweenCentersWith(Rolls[i + 2]);
        G_Angle[i + 1] = Math.Acos(Math.Round((C0_C1 * C0_C1 + C1_C2 * C1_C2 - C0_C2 * C0_C2) / 2 / C0_C1 / C1_C2, 6));
        Beta = 2 * Math.PI - F_Angle[i] - F_Angle[i + 1] - G_Angle[i + 1];
        lengths[i + 1].L = Rolls[i + 1].Radius * Beta;
        Beta = 1;
      }
      return lengths;
    }

    protected double[] Times()
    {
      var result = new List<double>();
      Length[] ls = Ls();
      double Average_Velocity;
      if (Rolls.Count >= 5)
      {
        Average_Velocity = (Rolls[3].Velocity + Rolls[4].Velocity) / 2;
        result.Add(ls[3].l / Average_Velocity);
      }

      for (int i = 4; i < Rolls.Count - 1; i++)
      {
        result.Add(ls[i].L / Rolls[i].Velocity);
        Average_Velocity = (Rolls[i].Velocity + Rolls[i + 1].Velocity) / 2;
        result.Add(ls[i].l / Average_Velocity);
      }
      return result.ToArray();
    }
  }
}
