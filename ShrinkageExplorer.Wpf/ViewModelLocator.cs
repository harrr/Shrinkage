
using System;
using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ShrinkageExplorer.Core.Repository;
using ShrinkageExplorer.Data;
using ShrinkageExplorer.Wpf.Utilities;
using ShrinkageExplorer.Wpf.ViewModels;
using ShrinkageExplorer.Wpf.ViewModels.Admin;
using ShrinkageExplorer.Wpf.ViewModels.Common;
using ShrinkageExplorer.Wpf.ViewModels.Operator;
using ShrinkageExplorer.Wpf.Views;
using ShrinkageExplorer.Wpf.Views.Common;

namespace ShrinkageExplorer.Wpf
{
  public class ViewModelLocator
  {
    private static readonly Dictionary<Type, Type> ViewForViewModel;

    static ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      if (ViewModelBase.IsInDesignModeStatic)
      {
        // SimpleIoc.Default.Register<ILinesRepository, LineRepository>();
      }
      else
      {
        SimpleIoc.Default.Register<IMainRepository, MainRepository>();
        SimpleIoc.Default.Register<IDialogService, WpfWindowDialogService>();
        SimpleIoc.Default.Register<IMessageService, WpfMessageService>();
      }

      SimpleIoc.Default.Register<OperatorUiViewModel>();
      SimpleIoc.Default.Register<AdminUiViewModel>();
      SimpleIoc.Default.Register<RollLineViewModel>();
      SimpleIoc.Default.Register<LoginViewModel>();

      ViewForViewModel = new Dictionary<Type, Type>
      {
        {typeof(AdminUiViewModel), typeof(Views.Admin.MainView)},
        {typeof(OperatorUiViewModel), typeof(Views.Operator.MainView)},

        {typeof(RollDriveViewModel), typeof(DrivePropertiesView)},
        {typeof(RollViewModel), typeof(RollPropertiesView)},
        {typeof(LoginViewModel), typeof(LoginView)},
      };
    }

    public static Window GetViewForViewModel(ViewModelBase viewModel, bool isDialogWindow = true)
    {
      var viewModelType = viewModel.GetType();
      if (!ViewForViewModel.ContainsKey(viewModel.GetType()))
        throw new ArgumentException("There are no View for this ViewModel", "viewModel");
      var viewType = ViewForViewModel[viewModelType];
      var window = Activator.CreateInstance(viewType) as Window;
      if (isDialogWindow)
        window.Owner = Application.Current.MainWindow;
      window.DataContext = viewModel;
      return window;
    }

    /// <summary>
    /// Gets the OperatorUiViewModel property.
    /// </summary>
    public OperatorUiViewModel OperatorUiViewModel
    {
      get
      {
        return ServiceLocator.Current.GetInstance<OperatorUiViewModel>();
      }
    }

    public LoginViewModel LoginViewModel
    {
      get
      {
        return ServiceLocator.Current.GetInstance<LoginViewModel>();
      }
    }

    public AdminUiViewModel AdminUiViewModel
    {
      get
      {
        return ServiceLocator.Current.GetInstance<AdminUiViewModel>();
      }
    }

    public static IDialogService DialogService
    {
      get { return new WpfWindowDialogService(); }
    }
    /// <summary>
    /// Cleans up all the resources.
    /// </summary>
    public static void Cleanup()
    {
    }
  }
}