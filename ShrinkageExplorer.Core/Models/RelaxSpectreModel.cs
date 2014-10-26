using System;
using System.Collections.Generic;
using System.Linq;
using ShrinkageExplorer.Core.ExtensionMethods;
using ShrinkageExplorer.Core.Interfaces;

namespace ShrinkageExplorer.Core.Models
{
  public class RelaxSpectreModel : ShrinkageModel
  {
    private double _n;
    private double _theta0;
    private double _theta1;
    private double _thetaM;
    private double _mu0;
    private double _tempCoeff; //b
    private double _refTemp; //Tr
    private double _spectrum0; //PSY
    private double _glassTemp; //Tg
    private double _k; //Angle coef
    private double _density;
    private double _specificHeat;
    private double _rollCoefficient;
    private double _airCoefficient;
    private double _airTemperature;

    private bool _constD;

    public override string ModelName
    {
      get
      {
        return "Relax spectre model";
      }
    }

    // Shift Factor Calculating

    private double ShiftFactorCalc(double temperature)
    {
      return Math.Exp(-_tempCoeff * (temperature - _refTemp));
    }

    private double Max_ViscosityCalc(double temperature)
    {
      var shiftFactor = ShiftFactorCalc(temperature);

      var t0 = _theta0 * shiftFactor;
      var t1 = _theta1 * shiftFactor;
      var tm = _thetaM * shiftFactor;
      var maxViscosity = (t1 - t0) + Math.Pow(t1, _k) / (1 - _k) * (Math.Pow(tm, 1 - _k) - Math.Pow(t1, 1 - _k));
      maxViscosity *= _spectrum0;
      return maxViscosity;
    }


    private double HE_ModuleCalc(double temperature)
    {

      var shiftFactor = ShiftFactorCalc(temperature);

      var t0 = _theta0 * shiftFactor;
      var t1 = _theta1 * shiftFactor;
      var tm = _thetaM * shiftFactor;

      var maxViscosity = Max_ViscosityCalc(temperature);

      var heModule = (t1 * t1 - t0 * t0) / 2 + Math.Pow(t1, _k) / (2 - _k) * (Math.Pow(tm, 2 - _k) - Math.Pow(t1, 2 - _k));
      heModule *= _spectrum0;
      heModule = maxViscosity * maxViscosity / heModule;

      return heModule;
    }

    public double[] ThermalBalanceCalc()
    {
      double initialThickness = IFilm.Thickness;
      double initialTemperature = IFilm.Temperature;

      var temperature = Times();
      var result = new double[temperature.Length];
      var currentThickness = initialThickness;
      var currentTemp = initialTemperature;
      for (int i = 0, k = 1; i < temperature.Length; i++)
      {
        double deltaTemp;
        if (i % 2 == 0)
        {
          var averageThickness = currentThickness + currentThickness * (Rolls[k - 1].Velocity / Rolls[k].Velocity) / 2;
          currentThickness = currentThickness * (Rolls[k - 1].Velocity / Rolls[k].Velocity);
          deltaTemp = 2 * _airCoefficient * (_airTemperature - currentTemp) /
                      _density / _specificHeat / averageThickness * temperature[i];
          k++;
        }
        else
        {
          deltaTemp = (_airCoefficient * (_airTemperature - currentTemp) +
                       _rollCoefficient * (Rolls[k].Temperature - currentTemp)) /
                      _density / _specificHeat / currentThickness * temperature[i];
        }
        currentTemp += deltaTemp;
        result[i] = currentTemp;
      }
      return result;
    }

    public override ShrinkageResult[] Calculate(IRollLine line, IFilm film)
    {
      Line = line;
      IFilm = film;
      InitializeCoefficients();

      Rolls = line.WorkingRolls.ToList();
      var results = new List<ShrinkageResult>();

      var Temperature = ThermalBalanceCalc();
      var time = Times();
      var rate = new double[time.Length + 1];
      var e = Es();
      var sigmas1 = new double[time.Length + 1];
      // initial action

      const double shearStress = 0;
      
      const double memoryCoef = 1;
      const double eps = 1E-8;

      double storeTemp = _glassTemp;
      const int nodes = 10;



      // first action
      for (var i = 0; i < Temperature.Length; i++)
      {
        // Temperature[i] = Math.Round(Temperature[i],1);
        if (Temperature[i] < _glassTemp)
        {
          rate[i] = 1;
          continue;
        }

        var shiftFactor = ShiftFactorCalc(Temperature[i]);

        var t0 = _theta0 * shiftFactor;
        var t1 = _theta1 * shiftFactor;
        var tm = _thetaM * shiftFactor;


        var initialSigma = (t1 * t1 - t0 * t0) / 2 
          + Math.Pow(t1, _k) / (2 - _k) 
          * (Math.Pow(tm, 2 - _k) - Math.Pow(t1, 2 - _k));


        var currentTime = Math.Round(time[i], 2);

        var firstIntegral = 
          MathMethods.SimpsonIntegral(x => 
                                        x * Math.Exp(-currentTime / x) 
                                        * (1 + currentTime / x), 
                                      t0, t1, nodes);

        var secondIntegral =
          MathMethods.SimpsonIntegral(x => 
                                        Math.Pow(x, 1 - _k) 
                                        * Math.Exp(-currentTime / x) 
                                        * (1 + currentTime / x), 
                                      t1, tm, nodes);

        var currentSigma = firstIntegral+ Math.Pow(t1, _k) * secondIntegral;

        rate[i] = currentSigma / initialSigma;
      }



      // second action
      double sigma1 = shearStress;
      bool ext = false;

      for (int i = 0; i < Temperature.Length; i++)
      {
        if (e[i] > 1)
        {
          double temperature = Temperature[i];
          double rateDef = (1 - 1 / e[i]) / (time[i]);
          double shiftFactor = ShiftFactorCalc(temperature);


          double sigmaE = 4 * _mu0 * shiftFactor *
                           Math.Pow(2 * rateDef, _n) * rateDef;


          sigma1 = sigmaE + memoryCoef * sigma1;

          ext = true;
        }

        sigma1 *= rate[i];
        sigmas1[i] = sigma1;
        //


        // finishing ...
        double heModule = HE_ModuleCalc(Temperature[i]);

        double sigmaModuleRel = sigma1 / heModule;


        double alpha;
        var a = 0.5;
        var b = 1.5;
        if (ext && _constD)
        {
          if (sigmaModuleRel > 1)
            a = Math.Pow(sigmaModuleRel, 0.5);

          
          alpha = MathMethods.BisectRoot(x => x * x * x - sigmaModuleRel * x - 1, a, b, eps);
          var tmp = 1 - 1/Math.Sqrt(alpha);
          results.Add(new ShrinkageResult((1 - alpha)*5, -tmp, -tmp));
        }
        else
        {
          alpha = Math.Sqrt(
            (sigma1 + Math.Sqrt(sigma1 * sigma1 + 4 * heModule * heModule)) 
            / (2 * heModule));

          results.Add(new ShrinkageResult(alpha - 1, 0, (1/alpha - 1)));
        }
      }
      //results.Reverse();
      return results.ToArray();
    }

    private void InitializeCoefficients()
    {
      _theta0 = IFilm.Material.GetScalarValue("theta0");
      _theta1 = IFilm.Material.GetScalarValue("theta1");
      _thetaM = IFilm.Material.GetScalarValue("thetaM");
      _spectrum0 = IFilm.Material.GetScalarValue("psi0");
      _k = IFilm.Material.GetScalarValue("k");
      _tempCoeff = IFilm.Material.GetScalarValue("b");
      _refTemp = IFilm.Material.GetScalarValue("Tr");
      _glassTemp = IFilm.Material.GetScalarValue("Tg");
      _density = IFilm.Material.GetScalarValue("rho");
      _specificHeat = IFilm.Material.GetScalarValue("C");
      _mu0 = IFilm.Material.GetScalarValue("mu0");
      _n = IFilm.Material.GetScalarValue("n");
      _rollCoefficient = Line.RollCoefficient;
      _airCoefficient = Line.AirCoefficient;
      _airTemperature = Line.AirTemperature;
      _constD = true;
    }
  }
}
