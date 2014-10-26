using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Wpf.ViewModels.Admin
{
  public class ParametersViewModel : ViewModelBase
  {
    private readonly IPropertiesRepository _repository;
    private IEnumerable<IProperty> _properties;


    public ParametersViewModel(IPropertiesRepository repository)
    {
      _repository = repository;
      _properties = _repository.Properties;

      AddNewPropertyCommand = new RelayCommand(_repository.AddNew);
    }


    public IEnumerable<IProperty> Properties
    {
      get { return _properties; }
      set { Set("Properties", ref _properties, value); }
    }


    public RelayCommand AddNewPropertyCommand { get; private set; }
  }
}
