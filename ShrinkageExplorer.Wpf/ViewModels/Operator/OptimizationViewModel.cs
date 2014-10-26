using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.ShrinkageModels;
using ShrinkageExplorer.Wpf.ViewModels.Common;

namespace ShrinkageExplorer.Wpf.ViewModels.Operator
{
  public class OptimizationViewModel : ViewModelBase
  {
    private float _eps;
    private FilmViewModel _initialFilm;
    private RollLineViewModel _initialLine;
    private ShrinkageModelViewModel _model;
    private RollLineViewModel _resultLine;
    private float _slRequired;
    private float _slResult;

    public OptimizationViewModel()
    {
      FindConfigurationCommand = new RelayCommand(() =>
      {
        var finder = new GoalShrinkageParametersFinder(
          InitialLine.ModelLine, InitialFilm.Film, Model.Model.MathModel);

        LineWithShrinkage lineWithShrinkage = finder.GetLineParametersForSpecifiedLengthShrinkage(SlRequired, Eps);
        if (lineWithShrinkage == null)
        {
          MessageBox.Show("Cannot find line configuration for given line");
          return;
        }
        ResultLine = new RollLineViewModel(lineWithShrinkage.Line);
        SlResult = lineWithShrinkage.Shrinkage;
      });
    }

    public ShrinkageModelViewModel Model
    {
      get { return _model; }
      set { Set(() => Model, ref _model, value); }
    }

    public RollLineViewModel InitialLine
    {
      get { return _initialLine; }
      set { Set(() => InitialLine, ref _initialLine, value); }
    }

    public FilmViewModel InitialFilm
    {
      get { return _initialFilm; }
      set { Set(() => InitialFilm, ref _initialFilm, value); }
    }

    public RollLineViewModel ResultLine
    {
      get { return _resultLine; }
      set { Set(() => ResultLine, ref _resultLine, value); }
    }

    public float Eps
    {
      get { return _eps; }
      set
      {
        _eps = value / 100;
        RaisePropertyChanged(() => Eps);
      }
    }

    public float SlRequired
    {
      get { return _slRequired; }
      set
      {
        _slRequired = value / 100;
        RaisePropertyChanged(() => SlRequired);
      }
    }

    public float SlResult
    {
      get { return _slResult; }
      set { Set(() => SlResult, ref _slResult, value); }
    }

    public RelayCommand FindConfigurationCommand { get; private set; }
  }
}