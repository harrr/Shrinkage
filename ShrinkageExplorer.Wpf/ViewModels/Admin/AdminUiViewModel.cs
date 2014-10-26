using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;
using ShrinkageExplorer.Wpf.Utilities;

namespace ShrinkageExplorer.Wpf.ViewModels.Admin
{
  public class AdminUiViewModel : ViewModelBase
  {
    private readonly IMessageService _messageService;

    private readonly ILinesRepository _lineRepository;
    private readonly IMaterialsRepository _materialRepository;
    private readonly IModelsRepository _modelRepository;
    private readonly IPropertiesRepository _propertiesRepository;
    private readonly IUsersRepository _usersRepository;

    private IEnumerable<IProperty> _allParameters;


    private MaterialParametersViewModel _materialParametersViewModel;
    private ModelParametersViewModel _modelParametersViewModel;
    private ParametersViewModel _parametersViewModel;
    private LinesViewModel _linesViewModel;
    private UsersViewModel _usersViewModel;


    private IRollLine _selectedLine;

    public AdminUiViewModel(IMainRepository mainRepository, IMessageService messageService)
    {
      _messageService = messageService;
      _lineRepository = mainRepository.LinesRepository;
      _materialRepository = mainRepository.MaterialsRepository;
      _modelRepository = mainRepository.ModelsRepository;
      _propertiesRepository = mainRepository.PropertiesRepository;
      _usersRepository = mainRepository.UsersRepository;

      AllParameters = _propertiesRepository.Properties;

      ModelParametersViewModel = new ModelParametersViewModel(AllParameters, _modelRepository);
      MaterialParametersViewModel = new MaterialParametersViewModel(AllParameters, _materialRepository);
      ParametersViewModel = new ParametersViewModel(_propertiesRepository);
      LinesViewModel = new LinesViewModel(_lineRepository);
      UsersViewModel = new UsersViewModel(_usersRepository);

      SaveChangesCommand = new RelayCommand(
        () =>
        {
          mainRepository.SaveChanges();
          _messageService.ShowMessage("Changes saved successfully");
        });
    }

    public IEnumerable<IProperty> AllParameters
    {
      get { return _allParameters; }
      private set { Set("AllParameters", ref _allParameters, value); }
    }

    public ModelParametersViewModel ModelParametersViewModel
    {
      get { return _modelParametersViewModel; }
      set { Set("ModelParametersViewModel", ref _modelParametersViewModel, value); }
    }

    public MaterialParametersViewModel MaterialParametersViewModel
    {
      get { return _materialParametersViewModel; }
      set { Set("ModelParametersViewModel", ref _materialParametersViewModel, value); }
    }


    public UsersViewModel UsersViewModel
    {
      get { return _usersViewModel; }
      set { Set("UsersViewModel", ref _usersViewModel, value); }
    }

    public LinesViewModel LinesViewModel
    {
      get { return _linesViewModel; }
      set { Set("LinesViewModel", ref _linesViewModel, value); }
    }

    public ParametersViewModel ParametersViewModel
    {
      get { return _parametersViewModel; }
      set { Set("ParametersViewModel", ref _parametersViewModel, value); }
    }

    public RelayCommand SaveChangesCommand { get; private set; }
  }
}