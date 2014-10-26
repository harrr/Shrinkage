using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.DataClasses;

namespace ShrinkageExplorer.Wpf.Repository.CodedRepository
{
  public class LineRepository : ILineRepository
  {
    public IEnumerable<RollLine> Lines { get; private set; }

    public LineRepository()
    {
      var drives1 = new List<RollDrive>
      {
        new RollDrive(new List<GeometryRoll>
        {
          new GeometryRoll(0,1,0.50f),
          new GeometryRoll(1,2,0.50f),
          new GeometryRoll(2,3,0.50f)
        }),
        new RollDrive(new List<GeometryRoll>
        {
          new GeometryRoll(3,4,0.50f),
          new GeometryRoll(4,5,0.50f),
        })
      };
      var drives2 = new List<RollDrive>
      {
        new RollDrive(new List<GeometryRoll>
        {
          new GeometryRoll(0,100,10),
          new GeometryRoll(100,100,20),
          new GeometryRoll(200,100,30)
        }),
        new RollDrive(new List<GeometryRoll>
        {
          new GeometryRoll(300,300,40),
          new GeometryRoll(400,400,50),
        })
      };

      var line1 = new RollLine(drives1);
      var line2 = new RollLine(drives2);
      Lines = new List<RollLine> { line1, line2 };
    }
  }
}
