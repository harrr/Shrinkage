using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Data
{
  public class LineRepository : ILinesRepository
  {
    private readonly ShrinkageEntities _entities;
    public LineRepository(ShrinkageEntities entities)
    {
      _entities = entities;
      Lines = _entities.Lines.Local;
      
    }


    public IEnumerable<IRollLine> Lines { get; private set; }

    #region Члены ILinesRepository


    public void AddNew()
    {
      _entities.Lines.Add(new Line
      {
        Name = ""
      });
    }

    public IRollDrive CreateDrive(IRollLine line, int number)
    {
      var dbLine = _entities.Lines.First(l => l.Name == line.Name);
      return _entities.Drives.Add(new Drive
      {
        Line = dbLine,
        Number = number
      });
    }

    #endregion
  }
}
