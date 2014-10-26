using System;
using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.ExtensionMethods;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Models
{
  public class MooneyRivlinModel : ShrinkageModel
  {

    public override ShrinkageResult[] Calculate(IRollLine line, IFilm film)
    {
      Line = line;
      IFilm = film;
      Rolls = line.WorkingRolls.ToList();
      var results = new List<ShrinkageResult>();

      var L = Ls().Select(x => x.l).ToArray();
      var films = Films();

      var n = IFilm.Material.GetScalarValue("n");
      var b = IFilm.Material.GetScalarValue("b");
      var mu0 = IFilm.Material.GetScalarValue("mu0");
      var Tr = IFilm.Material.GetScalarValue("Tr");

      var A = IFilm.Material.GetScalarValue("A_mr");
      var B = IFilm.Material.GetScalarValue("B_mr");
      var C = IFilm.Material.GetScalarValue("C_mr");

      var x1 = Math.Pow(2, n + 1);
      var gSum = 0.0;
      var result = new List<double>();
      var rolls = Line.WorkingRolls.ToList();
      for (var i = 0; i < rolls.Count - 1; ++i)
      {
        var currentRoll = rolls[i];
        var nextRoll = rolls[i + 1];

        var currentFilm = films[i];
        var nextFilm = films[i + 1];
        var x2 = Math.Pow((currentRoll.Velocity * n) / (L[i] * (1 - n)), n);
        var x3 = Math.Pow(1 - currentRoll.Velocity / nextRoll.Velocity, 1 - n);
        var x4 = (currentFilm.Thickness * currentFilm.Width) / (nextFilm.Thickness * nextFilm.Width);
        var temperature = Tr - currentRoll.Temperature;
        var mu = mu0 * Math.Exp(-b * temperature);
        var G = x1 * x2 * x3 * x4 * mu;
        if (!double.IsNaN(G) && !double.IsInfinity(G))
          gSum += G;
        const double step = 0.1;

        var sum = gSum;
        var root = MathMethods.ScanRoot(Sl => (C * Math.Pow(Sl, 6) + A * Math.Pow(Sl, 4) + (B - C - sum) * Math.Pow(Sl, 3) - A * Sl * Sl - B), 0, 0.5, 0.0001);
        if (!double.IsNaN(root) && Math.Abs(Math.Round(root, 5) - 1) > 0.001)
          result.Add(root);

        results.Add(new ShrinkageResult(-result.Sum(x => (x))/1.15, 0, 0));
      }


      return results.ToArray();
    }
    public override string ModelName
    {
      get
      {
        return "Mooney-Rivlin model";
      }
    }

  }
}
