using System.Collections.Generic;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Repository
{
  public interface IModelsRepository
  {
    IEnumerable<IModel> Models { get; }

    void AddNew();
  }
}
