using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Models;

namespace ShrinkageExplorer.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  namespace ShrinkageModels
  {
    public class LineWithShrinkage
    {
      public IRollLine Line { get; set; }
      public float Shrinkage { get; set; }
    }

    public class GoalShrinkageParametersFinder
    {
      private readonly IRollLine _line;
      private readonly IFilm _film;
      private readonly ShrinkageModel _model;

      public GoalShrinkageParametersFinder(IRollLine line, IFilm film, ShrinkageModel model)
      {
        _line = line;
        _film = film;
        _model = model;
      }

      public LineWithShrinkage GetLineParametersForSpecifiedLengthShrinkage(double lengthShrinkage, double eps)
      {

        var calculatedShrinkagesOnLines = new Dictionary<double, IRollLine>();
        var nearestShrinkage = 0.0f;
        var iteration = 0;
        const int maxIterationCount = 100;
        var random = new Random();

        while (iteration < maxIterationCount && !ShrinkageIsAchieved(nearestShrinkage, lengthShrinkage, eps))
        {
          var tempLine = _line.Clone();
          ++iteration;

          foreach (var drive in tempLine.Drives)
          {
            var minVelocity = (int)Math.Round(drive.MinVelocity * 100, 2);
            var maxVelocity = (int)Math.Round(drive.MaxVelocity * 100, 2);
            var newVelocity = random.Next(minVelocity, maxVelocity) / 100f;
            drive.Velocity = newVelocity;
          }
          nearestShrinkage = (float)_model.Calculate(tempLine, _film).Last().Sl;
          if (!calculatedShrinkagesOnLines.ContainsKey(nearestShrinkage))
            calculatedShrinkagesOnLines.Add(nearestShrinkage, _line);
          if (ShrinkageIsAchieved(nearestShrinkage, lengthShrinkage, eps))
            return new LineWithShrinkage { Line = tempLine, Shrinkage = nearestShrinkage };


          tempLine = null;
        }
        return null;
      }

      private bool ShrinkageIsAchieved(double calculated, double goal, double eps)
      {
        return Math.Abs(calculated - goal) < eps;
      }
    }
  }

}
