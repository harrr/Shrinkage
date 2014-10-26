using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Wpf.ViewModels.Admin
{
  public class ModelParametersViewModel : ViewModelBase
  {
    private readonly IModelsRepository _repository;
    private IModel _selectedModel;
    private ObservableCollection<IProperty> _availableProperties;
    private IEnumerable<IModel> _models;
    private IEnumerable<IProperty> _allProperties;


    public ModelParametersViewModel(IEnumerable<IProperty> allProperties, IModelsRepository repository)
    {
      _repository = repository;
      AllProperties = allProperties;
      Models = repository.Models;


      InitializeCommands();
    }

    private void InitializeCommands()
    {
      AddPropertyCommand = new RelayCommand<IEnumerable>(properties =>
      {
        foreach (var property in properties.Cast<IProperty>().ToList())
        {
          AvailableProperties.Remove(property);
          SelectedModelProperties.Add(property);
        }
      }, properties => properties != null && properties.Cast<IProperty>().Any());

      RemovePropertyCommand = new RelayCommand<IEnumerable>(properties =>
      {
        foreach (var property in properties.Cast<IProperty>().ToList())
        {
          AvailableProperties.Add(property);
          SelectedModelProperties.Remove(property);
        }
      }, properties => properties != null && properties.Cast<IProperty>().Any());

      AddNewModelCommand = new RelayCommand(_repository.AddNew);
    }

    public IEnumerable<IProperty> AllProperties
    {
      get { return _allProperties; }
      set { Set("AllProperties", ref _allProperties, value); }
    }

    public ObservableCollection<IProperty> AvailableProperties
    {
      get
      {
        if (SelectedModel == null) return null;
        return _availableProperties
          ?? (_availableProperties =
            new ObservableCollection<IProperty>(AllProperties.Except(SelectedModelProperties)));
      }
    }

    public IEnumerable<IModel> Models
    {
      get { return _models; }
      set { Set("Models", ref _models, value); }
    }

    public IModel SelectedModel
    {
      get { return _selectedModel; }
      set
      {
        Set("SelectedModel", ref _selectedModel, value);
        _availableProperties = null;
        RaisePropertyChanged("SelectedModelProperties");
        RaisePropertyChanged("AvailableProperties");
      }
    }

    public ICollection<IProperty> SelectedModelProperties
    {
      get
      {
        return _selectedModel == null ? null : _selectedModel.RequiredProperties;
      }
    }

    public RelayCommand<IEnumerable> AddPropertyCommand { get; private set; }
    public RelayCommand<IEnumerable> RemovePropertyCommand { get; private set; }
    public RelayCommand AddNewModelCommand { get; private set; }
  }
}
