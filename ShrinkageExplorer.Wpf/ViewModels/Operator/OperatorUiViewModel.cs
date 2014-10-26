using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GalaSoft.MvvmLight;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;
using ShrinkageExplorer.Data.PartialClasses;
using ShrinkageExplorer.Wpf.ViewModels.Common;

namespace ShrinkageExplorer.Wpf.ViewModels.Operator
{
  public class OperatorUiViewModel : ViewModelBase
  {
    private readonly ILinesRepository _lineRepository;
    private readonly IMaterialsRepository _materialRepository;
    private readonly IModelsRepository _modelRepository;
    private FilmViewModel _film;
    private RollLineViewModel _line;
    private ShrinkageModelViewModel _model;
    private OptimizationViewModel _optimization;

    [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public OperatorUiViewModel(IMainRepository mainRepository)
    {
      _lineRepository = mainRepository.LinesRepository;
      _materialRepository = mainRepository.MaterialsRepository;
      _modelRepository = mainRepository.ModelsRepository;

      Optimization = new OptimizationViewModel();

      Models = _modelRepository.Models.Select(m => new ShrinkageModelViewModel { Model = m }).ToList();
      Model = Models.First();

      Materials = _materialRepository.Materials;
      Lines = new ObservableCollection<RollLineViewModel>(_lineRepository.Lines.Select(line => new RollLineViewModel(line)));
      Line = Lines.FirstOrDefault();
      Film = new FilmViewModel(new Film
      {
        Material = Materials.First(),
        Width = 2,
        Thickness = 0.0005f,
        Temperature = 210
      });
    }


    public RollLineViewModel Line
    {
      get { return _line; }
      set
      {
        Set(() => Line, ref _line, value);
        Model.Line = _line;
        Optimization.InitialLine = Line;
      }
    }

    public FilmViewModel Film
    {
      get { return _film; }
      set
      {
        Set(() => Film, ref _film, value);
        Model.Film = _film;
        Optimization.InitialFilm = _film;
      }
    }

    public ShrinkageModelViewModel Model
    {
      get { return _model; }
      set
      {
        if (_model == value || value == null)
          return;
        _model = value;
        _model.Film = Film;
        _model.Line = Line;
        Optimization.Model = value;
        RaisePropertyChanged(() => Model);
      }
    }


    public OptimizationViewModel Optimization
    {
      get { return _optimization; }
      set { Set(() => Optimization, ref _optimization, value); }
    }

    public ObservableCollection<RollLineViewModel> Lines { get; private set; }

    public IEnumerable<IMaterial> Materials { get; private set; }

    public IEnumerable<ShrinkageModelViewModel> Models { get; private set; }


  }
}