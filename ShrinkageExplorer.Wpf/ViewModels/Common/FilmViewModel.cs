using GalaSoft.MvvmLight;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Wpf.ViewModels.Common
{

  public class FilmViewModel : ViewModelBase
  {
    public IFilm Film { get; private set; }

    public float Thickness
    {
      get
      {
        return Film.Thickness;
      }
      set
      {
        if (Film.Thickness == value)
          return;
        Film.Thickness = value;
        RaisePropertyChanged(() => Thickness);
      }
    }

    public float Width
    {
      get
      {
        return Film.Width;
      }
      set
      {
        if (Film.Width == value)
          return;
        Film.Width = value;
        RaisePropertyChanged(() => Width);
      }
    }

    public float Temperature
    {
      get
      {
        return Film.Temperature;
      }
      set
      {
        if (Film.Temperature == value)
          return;
        Film.Temperature = value;
        RaisePropertyChanged(() => Temperature);
      }
    }

    public IMaterial Material
    {
      get
      {
        return Film.Material;
      }
      set
      {
        if (Film.Material == value)
          return;
        Film.Material = value;
        RaisePropertyChanged(() => Material);
      }
    }

    public FilmViewModel(IFilm film)
    {
      Film = film;
    }
  }
}