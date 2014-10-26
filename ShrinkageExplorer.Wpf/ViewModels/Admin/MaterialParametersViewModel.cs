
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;
using ShrinkageExplorer.Data;

namespace ShrinkageExplorer.Wpf.ViewModels.Admin
{
  public class MaterialParametersViewModel : ViewModelBase
  {
    private IMaterial _selectedMaterial;
    private ObservableCollection<IProperty> _availableProperties;
    private IEnumerable<IMaterial> _materials;
    private IEnumerable<IProperty> _allProperties;


    public MaterialParametersViewModel(IEnumerable<IProperty> allProperties, IMaterialsRepository repository)
    {
      AllProperties = allProperties;
      Materials = repository.Materials;


      AddPropertyCommand = new RelayCommand<IEnumerable>(properties =>
      {
        foreach (var property in properties.Cast<Parameter>().ToList())
        {
          AvailableProperties.Remove(property);
          SelectedMaterial.Properties.Add(new MaterialParameter(property));
        }
      }, properties => properties != null && properties.Cast<IProperty>().Any());
      RemovePropertyCommand = new RelayCommand<IEnumerable>(properties =>
      {
        foreach (var property in properties.Cast<MaterialParameter>().ToList())
        {
          AvailableProperties.Add(property.Parameter);
          SelectedMaterial.Properties.Remove(property);
        }
      }, properties => properties != null && properties.Cast<MaterialParameter>().Any());

      AddNewMaterialCommand = new RelayCommand(repository.AddNew);
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
        if (SelectedMaterial == null) return null;
        return _availableProperties
          ?? (_availableProperties =
            new ObservableCollection<IProperty>(AllProperties.Except(SelectedMaterial.Properties)));
      }
    }


    public IEnumerable<IMaterial> Materials
    {
      get { return _materials; }
      set { Set("Models", ref _materials, value); }
    }

    public IMaterial SelectedMaterial
    {
      get { return _selectedMaterial; }
      set
      {
        Set("SelectedModel", ref _selectedMaterial, value);
        _availableProperties = null;
        RaisePropertyChanged("SelectedMaterialProperties");
        RaisePropertyChanged("AvailableProperties");
      }
    }


    public ICollection<IMaterialProperty> SelectedMaterialProperties
    {
      get { return _selectedMaterial == null ? null : _selectedMaterial.Properties; }
    }

    public RelayCommand<IEnumerable> AddPropertyCommand { get; private set; }
    public RelayCommand<IEnumerable> RemovePropertyCommand { get; private set; }
    public RelayCommand AddNewMaterialCommand { get; private set; }
  }
}
