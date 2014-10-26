using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;
using ShrinkageExplorer.Data;
using ShrinkageExplorer.Wpf.Utilities;
using ShrinkageExplorer.Wpf.ViewModels.Admin;
using ShrinkageExplorer.Wpf.ViewModels.Operator;

namespace ShrinkageExplorer.Wpf.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {
    private string _login;
    private string _password;
    private bool _isCompleted;

    private IUser _user;
    private readonly IUsersRepository _repository;
    private readonly IMessageService _messageService;
    private readonly IMainRepository _mainRepository;

    public LoginViewModel(IMainRepository repository, IMessageService messageService)
    {
      _repository = repository.UsersRepository;
      _messageService = messageService;
      _mainRepository = repository;

      LoginCommand = new RelayCommand(
        LoginCommandExecuted,
        () => User != null && !String.IsNullOrEmpty(Password)
      );
    }

    private void LoginCommandExecuted()
    {
      if (!User.CheckPassword(Password))
      {
        _messageService.ShowMessage("Incorrect password. Try again");
        return;
      }
      switch (User.UserType)
      {
        case UserTypes.Operator:
          {
            var view = ViewModelLocator.GetViewForViewModel(new OperatorUiViewModel(_mainRepository), false);
            view.Show();
            Application.Current.MainWindow = view;
            break;
          }
        case UserTypes.Administrator:
          {
            var view = ViewModelLocator.GetViewForViewModel(new AdminUiViewModel(_mainRepository, _messageService), false);
            view.Show();
            Application.Current.MainWindow = view;
            break;
          }
      }
      IsCompleted = true;

      IsCompleted = false;
      Password = "";
    }

    public string Login
    {
      get { return _login; }
      set
      {
        Set("Login", ref _login, value);
        User = _repository.GetUserByName(_login);
      }
    }

    public string Password
    {
      get { return _password; }
      set { Set("Password", ref _password, value); }
    }

    public bool IsCompleted
    {
      get { return _isCompleted; }
      set { Set("IsCompleted", ref _isCompleted, value); }
    }

    public IUser User
    {
      get { return _user; }
      set { Set("User", ref _user, value); }
    }

    public RelayCommand LoginCommand { get; private set; }

  }
}
