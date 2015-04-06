using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitAssemblies
  {
    public NUnitAssemblies UnionWith(NUnitAssemblies nUnitAssemblies)
    {
      return new NUnitAssemblies(_assemblies.Union(nUnitAssemblies._assemblies));
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

      var projectEl =
        XElement.Parse(
          String.Format(CultureInfo.InvariantCulture,
                        "<test-suite type=\"Test Project\" name=\"\" executed=\"True\" result=\"{0}\" time=\"{1:F6}\" asserts=\"{2}\" />",
                        result,
                        time,
                        asserts));
      var results = XElement.Parse("<results/>");

      results.Add(_assemblies.ToArray());
      projectEl.Add(results);
      return projectEl;
    }

    private Tuple<string, double, int> Fold()
    {
      return _assemblies.Aggregate(Tuple.Create("Success", 0.0, 0), FoldAssemblyToProjectTuple);
    }


    private static Tuple<string, double, int> FoldAssemblyToProjectTuple(
      Tuple<string, double, int> aggregatedValue, XElement assembly)
    {
      var result = aggregatedValue.Item1;
      var time = aggregatedValue.Item2;
      var asserts = aggregatedValue.Item3;

      string outResult;

      if (assembly.Attribute("result").Value == "Failure")
      {
        outResult = "Failure";
      }
      else if (assembly.Attribute("result").Value == "Inconclusive" && result == "Success")
      {
        outResult = "Inconclusive";
      }
      else
      {
        outResult = result;
      }

      return Tuple.Create(outResult, time + XmlCulture.GetDouble(assembly.Attribute("time").Value),
                          asserts + XmlCulture.GetInt(assembly.Attribute("asserts").Value));
    }


    private readonly IEnumerable<XElement> _assemblies;
  }
}