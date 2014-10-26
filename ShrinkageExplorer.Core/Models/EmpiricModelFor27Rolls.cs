using System;
using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.ExtensionMethods;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Models
{
  public class EmpiricModelFor27Rolls : EmpiricModel
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

      var H0 = Math.Round(IFilm.Thickness, 6) * Math.Pow(10, 6);
      var cT = 0.0;
      var gT = 0.0;
      var dV = 0.0;
      var qV = 0.0;
      for (var i = 0; i < 27; ++i)
      {
        var roll = Rolls[i];
        dV += D[i] * roll.Velocity * 60;
        qV += Q[i] * roll.Velocity * 60;
        if (i < 20)
        {
          cT += C[i] * roll.Temperature;
          gT += G[i] * roll.Temperature;
        }
        var sl = (a + b * H0 + cT + dV) / 100;
        var sd = (e + f * H0 + gT + qV) / 100;

        results.Add(new ShrinkageResult(sl*1.5, sd/12f, 0));
      }


      return results.ToArray();
    }

    public override string ModelName
    {
      get
      {
        return "Empiric model for 29 rolls line";
      }
    }

    public override bool Check(IRollLine line, IFilm film)
    {
      return film.Thickness >= 250 * Math.Pow(10, -6) && film.Thickness <= 600 * Math.Pow(10, -6) &&
                line.WorkingRolls.Count() == 27;
    }
  }
}
