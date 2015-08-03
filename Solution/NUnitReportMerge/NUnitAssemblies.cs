using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitAssemblies
  {
    public void UnionWith(NUnitAssemblies nUnitAssemblies)
    {
      _assemblies = _assemblies.Union(nUnitAssemblies._assemblies);
    }

    public NUnitAssemblies(IEnumerable<XElement> assemblies)
    {
      _assemblies = assemblies;
    }

    public XElement Xml()
    {
      var tuple = Fold();
      var result = tuple.Item1;
      var time = tuple.Item2;
      var asserts = tuple.Item3;

      var projectXml =
        XElement.Parse(
          string.Format(CultureInfo.InvariantCulture,
                        "<test-suite type=\"Test Project\" name=\"\" executed=\"True\" result=\"{0}\" time=\"{1:F6}\" asserts=\"{2}\" />",
                        result,
                        time,
                        asserts));
      var results = XElement.Parse("<results/>");

      results.Add(_assemblies.ToArray());
      projectXml.Add(results);
      return projectXml;
    }

    Tuple<string, double, int> Fold()
    {
      return _assemblies.Aggregate(Tuple.Create("Success", 0.0, 0), (aggregatedValue, assembly) =>
      {
        //TODO refactor
        return new ResultTimeAsserts(aggregatedValue, assembly).Fold();
      });
    }

    public class ResultTimeAsserts
    {
      private readonly XElement _assembly;

      public ResultTimeAsserts(Tuple<string, double, int> tuple, XElement assembly)
      {
        _assembly = assembly;
        Result = tuple.Item1;
        Time = tuple.Item2;
        Asserts = tuple.Item3;
      }

      public string Result { get; set; }
      public double Time { get; set; }
      public int Asserts { get; set; }

      Tuple<string, double, int> AsTuple()
      {
        return Tuple.Create(Result, Time, Asserts);
      }

      public string CreateOutResult()
      {
        string outResult;
        if (_assembly.Attribute("result").Value == "Failure")
        {
          outResult = "Failure";
        }
        else if (_assembly.Attribute("result").Value == "Inconclusive" && Result == "Success")
        {
          outResult = "Inconclusive";
        }
        else
        {
          outResult = Result;
        }
        return outResult;
      }

      public Tuple<string, double, int> ToTupleWithAssemblyData(string outResult)
      {
        return Tuple.Create(outResult, Time + XmlCulture.GetDouble(_assembly.Attribute("time").Value),
          Asserts + XmlCulture.GetInt(_assembly.Attribute("asserts").Value));
      }

      public Tuple<string, double, int> Fold()
      {
        var outResult = CreateOutResult();

        return ToTupleWithAssemblyData(outResult);
      }
    }


    private IEnumerable<XElement> _assemblies;
  }
}