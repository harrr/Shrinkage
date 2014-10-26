using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Wpf.ViewModels.Admin
{
  public class UsersViewModel : ViewModelBase
  {
    private readonly IUsersRepository _repository;
    private IEnumerable<IUser> _users;


    public UsersViewModel(IUsersRepository repository)
    {
      _repository = repository;
      _users = _repository.Users;

      AddNewUserCommand = new RelayCommand(_repository.AddNew);
    }


    public IEnumerable<IUser> Users
    {
      get { return _users; }
      set { Set("Users", ref _users, value); }
    }


    public RelayCommand AddNewUserCommand { get; private set; }
  }
}
