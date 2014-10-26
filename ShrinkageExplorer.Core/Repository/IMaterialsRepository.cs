using System.Collections.Generic;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Repository
{
  public interface IMaterialsRepository
  {
    IEnumerable<IMaterial> Materials { get; }

    void AddNew();
  }
}
