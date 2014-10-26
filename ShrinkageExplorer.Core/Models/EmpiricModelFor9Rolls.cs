using System;
using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.ExtensionMethods;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Models
{
  public class EmpiricModelFor9Rolls : EmpiricModel
  {

    public override ShrinkageResult[] Calculate(IRollLine line, IFilm film)
    {
      Line = line;
      IFilm = film;
      Rolls = line.WorkingRolls.ToList();
      var results = new List<ShrinkageResult>();
      var a = IFilm.Material.GetScalarValue("a_emp29");
      var b = IFilm.Material.GetScalarValue("b_emp29");
      var e = IFilm.Material.GetScalarValue("e_emp29");
      var f = IFilm.Material.GetScalarValue("f_emp29");
      var C = IFilm.Material.GetVectorValue("C_emp29");
      var D = IFilm.Material.GetVectorValue("D_emp29");
      var G = IFilm.Material.GetVectorValue("G_emp29");
      var Q = IFilm.Material.GetVectorValue("Q_emp29");

      var H0 = Films().Average(x => x.Thickness) * Math.Pow(10, 6);
      var cT = 0.0;
      var gT = 0.0;
      var dV = 0.0;
      var qV = 0.0;
      for (var i = 0; i < 27; ++i)
      {
        dV += D[i] * Rolls[i].Velocity;
        qV += Q[i] * Rolls[i].Velocity;
        if (i > 20) continue;
        cT += C[i] * Rolls[i].Temperature;
        gT += G[i] * Rolls[i].Temperature;

        var sl = (a + b * H0 + cT + dV) / 100;
        var sd = (e + f * H0 + gT + qV) / 100;

        results.Add(new ShrinkageResult(sl, sd, 0));
      }


      return results.ToArray();
    }
    public override string ModelName
    {
      get
      {
        return "Empiric model";
      }
    }


    public override bool Check(IRollLine line, IFilm film)
    {
      return film.Thickness >= 250 * Math.Pow(10, -6)
          && film.Thickness <= 600 * Math.Pow(10, -6)
          && line.WorkingRolls.Count() == 9;
    }
  }
}
