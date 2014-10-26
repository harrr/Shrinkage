using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Models;
using ShrinkageExplorer.Wpf.ViewModels.Common;

namespace ShrinkageExplorer.Wpf.ViewModels.Operator
{
  public class ShrinkageModelViewModel : ViewModelBase
  {
    private FilmViewModel _film;
    private bool _isInProgress;
    private bool _isResultFounded;
    private RollLineViewModel _line;
    private IModel _model;
    private ShrinkageResult _result;
    private Dictionary<int, float> _resultToChart;
    private ShrinkageResult[] _results;
    private Dictionary<int, IEnumerable<Point3D>> _shrinkageOfRoll;
    private IRollLine _temporaryLine;

    public ShrinkageModelViewModel()
    {
      ShrinkageOfRoll = new Dictionary<int, IEnumerable<Point3D>>();

      CalculateCommand = new RelayCommand(() =>
      {
        _temporaryLine = _line.ModelLine.Clone();
        IsResultFounded = false;
        CalculationResults = _model.MathModel.Calculate(_temporaryLine, _film.Film);
        if (CalculationResults == null)
          return;
        Result = CalculationResults.Last();

        var results = new Dictionary<int, float>();
        int calcResultCount = CalculationResults.Count();
        int drivesCount = Line.Drives.Count;
        var rollsCount = _line.Rolls.Count;
        var step = (float)calcResultCount / rollsCount;
        Task.Factory.StartNew(
          () =>
          {
            IsInProgress = true;
            var drives = _temporaryLine.Drives.ToList();
            for (int i = 0; i < drivesCount; ++i)
              ShrinkageOfRoll[i + 1] = CalculateShrinkageRelationForRoll(drives[i]);
            for (int i = 3; i < rollsCount; ++i)
              results.Add(i + 1, (float)CalculationResults[(int)(i * step)].Sl * 100);

          }).ContinueWith(task =>
        {
          ResultsToChart = results;

          IsResultFounded = true;
        }, TaskScheduler.FromCurrentSynchronizationContext());
      },
        () =>
        {
          var model = Model.MathModel;
          if (model == null)
            return false;

          var empModel = model as EmpiricModel;
          return empModel == null || empModel.Check(Line.ModelLine, Film.Film);
        });
    }

    public bool IsInProgress
    {
      get { return _isInProgress; }
      set { Set(() => IsInProgress, ref _isInProgress, value); }
    }

    public bool IsResultFounded
    {
      get { return _isResultFounded; }
      set { Set(() => IsResultFounded, ref _isResultFounded, value); }
    }

    public RollLineViewModel Line
    {
      get { return _line; }
      set
      {
        Set(() => Line, ref _line, value);
        if (Line == null || ShrinkageOfRoll.Count > 0) return;
        for (int i = 0; i < Line.Drives.Count; ++i)
          ShrinkageOfRoll.Add(i + 1, new Point3D[] { });
      }
    }

    public FilmViewModel Film
    {
      get { return _film; }
      set { Set(() => Film, ref _film, value); }
    }

    public IModel Model
    {
      get { return _model; }
      set { Set(() => Model, ref _model, value); }
    }


    public ShrinkageResult Result
    {
      get { return _result; }
      set { Set(() => Result, ref _result, value); }
    }

    public ShrinkageResult[] CalculationResults
    {
      get { return _results; }
      set { Set(() => CalculationResults, ref _results, value); }
    }

    public Dictionary<int, IEnumerable<Point3D>> ShrinkageOfRoll
    {
      get { return _shrinkageOfRoll; }
      private set { Set(() => ShrinkageOfRoll, ref _shrinkageOfRoll, value); }
    }

    public Dictionary<int, float> ResultsToChart
    {
      get { return _resultToChart; }
      private set { Set(() => ResultsToChart, ref _resultToChart, value); }
    }

    public RelayCommand CalculateCommand { get; private set; }

    public IEnumerable<Point3D> CalculateShrinkageRelationForRoll(IRollDrive drive)
    {
      float currentVelocity = drive.Velocity;
      float currentTemperature = drive.Temperature;

      var velocityStep = (drive.MaxVelocity - drive.MinVelocity) / 10f;
      var temperatureStep = (drive.MaxTemperature - drive.MinTemperature) / 10f;


      var pointList = new List<Point3D>();

      for (drive.Velocity = drive.MinVelocity;
        drive.Velocity < drive.MaxVelocity;
        drive.Velocity += velocityStep)
      {
        for (drive.Temperature = drive.MinTemperature;
          drive.Temperature < drive.MaxTemperature;
          drive.Temperature += temperatureStep)
        {
          double sl = _model.MathModel.Calculate(_temporaryLine, _film.Film).Last().Sl;
          pointList.Add(new Point3D(drive.Velocity*60, drive.Temperature, sl));
        }
      }
      drive.Velocity = currentVelocity;
      drive.Temperature = currentTemperature;

      return pointList;
    }
  }
}