using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Data
{
  public class UsersRepository : IUsersRepository
  {
    private ShrinkageEntities _entities;

    public IEnumerable<IUser> Users
    {
      get { return _entities.Users.Local; }
    }

    public void AddNew()
    {
      _entities.Users.Add(new User());
    }

    public UsersRepository(ShrinkageEntities entities)
    {
      _entities = entities;
    }

    public IUser GetUserByName(string name)
    {
      return Users.FirstOrDefault(u => u.Name == name);
    }
  }
}
