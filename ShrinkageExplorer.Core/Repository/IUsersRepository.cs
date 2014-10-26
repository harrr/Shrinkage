using System.Collections.Generic;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Repository
{
  public interface IUsersRepository
  {
    IEnumerable<IUser> Users { get; }

    void AddNew();

    IUser GetUserByName(string name);
  }
}
