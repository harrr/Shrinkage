using System;
using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Models
{
  public class ThreeLinkedModel : ShrinkageModel
  {

    private double _mu20, _G, _b2, _T0, _n, _mu10, _b1; //model parameters
    private double _h0;//material parameter
    private double[] _y;//result shrinkage
    public override ShrinkageResult[] Calculate(IRollLine line, IFilm film)
    {
      Line = line;
      IFilm = film;
      Rolls = line.WorkingRolls.ToList();
      var results = new List<ShrinkageResult>();

      var L = Ls().Select(x => x.l).ToArray();
      var films = Films();
      var result = new List<double>();
      var rolls = Line.WorkingRolls.ToList();

      double xPrev = -0.1 * rolls[0].Radius,
        //x coordinate where material leaves the prevous roll
                 yPrev = _h0 / 2,
        //y coordinate where material leaves the prevous  roll
                 xNext = xPrev,
        //x coordinate where material touches the next roll
                 yNext = yPrev,
        //y coordinate where material touches the next roll
                 fiPrev = 1.786e+2,
        //with this angle material leaves prev roll
                 fiNext = 0,
        //with this angle material touches the next roll
                 distance = 0; //
      double moveTime = 0, eps2, eps2point = 0, t = rolls[0].Temperature, epsPoint = (rolls[1].Velocity / rolls[0].Velocity - 1) / moveTime;
      //timespan between roles
      //eps2=kalander's eps2, eps2point is countable, t=t of role
      eps2 = 0.1;
      distance = rolls[0].Radius * (fiPrev);
      epsPoint = 0;
      int first = 0;
      /* for (int ind = 0; ind < rolls.Count-1; ind++)
       {
            
           if (rolls[ind].Temperature < rolls[ind + 1].Temperature || rolls[ind].Temperature > 200 ||
               (rolls[ind].Temperature <= 76 && first!=0))
               rolls.Remove(rolls[ind]);
           if (rolls[ind].Temperature <= 76 && first == 0)
               first = ind;
           ind++;
       }*/
      _y = new double[rolls.Count];
      for (int i = 0; i < rolls.Count - 1; i++)
      {
        var r1 = rolls[i];
        var r2 = rolls[i + 1];
        Cicle(xPrev, yPrev, r1.X, r1.Y, r2.X, r2.Y, r1.Radius, r2.Radius, out xPrev, out yPrev, out xNext, out yNext, out distance, out fiPrev, out fiNext, r1.Clockwise, r1.X, r1.Y);
        t = r1.Temperature;
        moveTime = (Math.Pow(r2.Velocity, 2) - Math.Pow(r1.Velocity, 2)) / (2 * distance);
        epsPoint = moveTime / rolls[0].Velocity;
        eps2point = Chorda(eps2, t, epsPoint);
        //for (double dt = 0; dt < 0.11; dt+=0.01)
        eps2 += eps2point * moveTime * 0.01;
        _y[i] = eps2 / (1 + (r2.Velocity / rolls[0].Velocity - 1))/100;
        results.Add(new ShrinkageResult(-_y[i], 0, 0));
      }
      var r = rolls[rolls.Count - 1];
      Cicle(xPrev, yPrev, r.X, r.Y, r.X, r.Y, r.Radius, r.Radius, out xPrev, out yPrev, out xNext, out yNext, out distance, out fiPrev, out fiNext, r.Clockwise, r.X, r.Y);
      t = r.Temperature;
      moveTime = (Math.Pow(r.Velocity, 2) - Math.Pow(r.Velocity, 2)) / (2 * distance);
      epsPoint = moveTime / rolls[0].Velocity;
      eps2point = Chorda(eps2, t, epsPoint);
      for (double dt = 0; dt < 11; dt += 1)
        eps2 += eps2point * dt;
      _y[rolls.Count - 1] = eps2 / (1 + (r.Velocity / rolls[0].Velocity - 1))/50;
      results.Add(new ShrinkageResult(-_y[rolls.Count - 1], 0, 0));

      return results.ToArray();
    }

    private void Cicle(double xPrev, double yPrev, double xNextStart, double yNextStart, double xNextFinish, double yNextFinish,
        double rPrev, double rNext, out double touchXPrev, out double touchYPrev, out double touchXNext, out double touchYNext,
        out double ds0, out double fiA, out double fiB, bool up, double xCenter, double yCenter)
    {
      double dx = xNextStart - xPrev;
      double dy = yNextStart - yPrev;
      double l = dx * dx + dy * dy;
      ds0 = Math.Sqrt(Math.Abs(l - rPrev * rPrev)); //distance material flows between rolls
      double alfa = Math.Atan(dy / dx);
      double fi = Math.Atan(rPrev / ds0);
      int i = (up ? 1 : -1);
      fiA = alfa + i * (fi + Math.PI / 2); //with this angle material leaves prev roll
      touchXPrev = xNextStart + rPrev * Math.Cos(fiA);
      touchYPrev = yNextStart + rPrev * Math.Sin(fiA);
      dx = xNextFinish - xNextStart;
      dy = yNextFinish - yNextStart;
      double teta = Math.Atan(Math.Sqrt((dx * dx + dy * dy) / Math.Pow(rNext + rPrev, 2) - 1));
      fiB = i * teta * Math.Atan(dy / dx); //with this angle material touches the next roll
      touchXNext = xCenter + rPrev * Math.Cos(fiB);
      touchYNext = yCenter + rPrev * Math.Sin(fiB);
    }

    private double Chorda(double eps2, double T, double epsPoint)
    {
      double x0 = eps2, x1 = eps2;
      while (F(eps2, T, epsPoint, x0) * F(eps2, T, epsPoint, x1) >= 0)
      {
        x0--;
        x1++;
      }
      double fa = F(eps2, T, epsPoint, x0), fb = F(eps2, T, epsPoint, x1), fc = F(eps2, T, epsPoint, (x0 + x1) / 2);
      while (Math.Abs(F(eps2, T, epsPoint, x0) - F(eps2, T, epsPoint, x1)) > 0.001 && Math.Abs(x0 - x1) >= 1e-3)
      {
        fa = F(eps2, T, epsPoint, x0);
        fb = F(eps2, T, epsPoint, x1);
        fc = F(eps2, T, epsPoint, (x0 + x1) / 2);
        if (F(eps2, T, epsPoint, x0) * F(eps2, T, epsPoint, (x0 + x1) / 2) < 0)
          x1 = (x0 + x1) / 2;
        else
          x0 = (x0 + x1) / 2;
      }
      return (x0 + x1) / 2;
    }

    private double F(double eps2, double T, double epsPoint, double eps2Point)
    {
      return eps2 + _mu20 * Math.Exp(-_b2 * (T - _T0)) * eps2Point / _G - _mu10 * Math.Exp(-_b1 * (T - _T0)) * Math.Pow(Math.Abs(epsPoint - eps2Point), _n) * Math.Sign(epsPoint - eps2Point) / _G;
    }
    public override string ModelName
    {
      get
      {
        return "ThreeLinked model";
      }
    }
  }
}
