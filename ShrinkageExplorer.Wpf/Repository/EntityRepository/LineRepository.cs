using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.DataClasses;
using ShrinkageExplorer.Data;

namespace ShrinkageExplorer.Wpf.Repository.EntityRepository
{
  public class LineRepository : ILineRepository
  {
    private ShrinkageEntities _entities;
    public LineRepository()
    {
      _entities = new ShrinkageEntities();
    }

    public IEnumerable<RollLine> Lines
    {
      get
      {

      }
    }
  }
}
