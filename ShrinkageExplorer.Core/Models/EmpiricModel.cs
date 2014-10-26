using System;
using System.Linq;
using System.Reflection;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Models
{
  public class EmpiricModel : ShrinkageModel
  {
    private static readonly Type[] EmpiricModelsTypes;

    public virtual bool Check(IRollLine line, IFilm film)
    {
      return true;
    }


    static EmpiricModel()
    {
      EmpiricModelsTypes = Assembly.GetAssembly(typeof(EmpiricModel))
                              .GetTypes()
                              .Where(t => t.IsSubclassOf(typeof(EmpiricModel)))
                              .ToArray();
    }

    public override ShrinkageResult[] Calculate(IRollLine line, IFilm film)
    {
      EmpiricModel model = null;
      bool check = false;
      foreach (var empiricModelType in EmpiricModelsTypes)
      {
        var tmpModel = Activator.CreateInstance(empiricModelType) as EmpiricModel;
        check = tmpModel.Check(line, film);
        if (tmpModel == null || !check) continue;
        model = tmpModel;
        break;
      }
      //var model = new EmpiricModel(Line, IFilm);
      if (model == null || !check)
        return null;

      return model.Calculate(line, film);
    }

    public override string ModelName
    {
      get { return "Empiric model"; }
    }
  }
}
