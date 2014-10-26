using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShrinkageExplorer.Core.DataClasses;

namespace ShrinkageExplorer.Wpf.Repository
{
  public interface ILineRepository
  {
    IEnumerable<RollLine> Lines { get; }
  }
}
