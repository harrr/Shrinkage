using System.Collections.Generic;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Repository
{
  public interface ILinesRepository
  {
    IEnumerable<IRollLine> Lines { get; }

    void AddNew();
    IRollDrive CreateDrive(IRollLine line, int number);
  }
}
