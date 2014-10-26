using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShrinkageExplorer.Core.Interfaces;
using ShrinkageExplorer.Core.Repository;

namespace ShrinkageExplorer.Wpf.ViewModels.Admin
{
  public class LinesViewModel: ViewModelBase
  {
    private readonly ILinesRepository _repository;
    private IEnumerable<IRollLine> _lines;


    public LinesViewModel(ILinesRepository repository)
    {
      _repository = repository;
      _lines = _repository.Lines;

      AddNewLineCommand = new RelayCommand(_repository.AddNew);
    }


    public IEnumerable<IRollLine> Lines
    {
      get { return _lines; }
      set { Set("Lines", ref _lines, value); }
    }


    public RelayCommand AddNewLineCommand { get; private set; }
  }
}
