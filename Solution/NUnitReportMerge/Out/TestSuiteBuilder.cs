using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class TestSuiteBuilder
  {
    readonly List<XElement> _assemblyResults;
    string Result { get; set; }
    double Time { get; set; }
    int Asserts { get; set; }

    public TestSuiteBuilder(string result, double time, int asserts)
    {
      _assemblyResults = new List<XElement>();
      Result = result;
      Time = time;
      Asserts = asserts;
    }

    public static string CreateOutResult(string result, bool isFailure, bool isInconclusive)
    {
      string outResult;
      if (isFailure)
      {
        outResult = "Failure";
      }
      else if (isInconclusive && result == "Success")
      {
        outResult = "Inconclusive";
      }
      else
      {
        outResult = result;
      }
      return outResult;
    }

    public static bool IsInconclusive(XElement xElement)
    {
      return xElement.Attribute("result").Value == "Inconclusive";
    }

    public static bool IsFailure(XElement xElement)
    {
      return xElement.Attribute("result").Value == "Failure";
    }

    public void Add(XElement xElement, double time, int asserts, bool isFailure, bool isInconclusive)
    {
      _assemblyResults.Add(xElement);
      Result = CreateOutResult(Result, isFailure, isInconclusive); //bug move this upper
      Time += time;
      Asserts += asserts;
    }

    public XElement Build()
    {
      var results = AddResultsTag();
      var projectXml = CreateTestSuiteTag();
      projectXml.Add(results);
      return projectXml;
    }

    XElement AddResultsTag()
    {
      var results = XElement.Parse("<results/>");
      results.Add(_assemblyResults.ToArray());
      return results;
    }

    XElement CreateTestSuiteTag()
    {
      return XElement.Parse(
        string.Format(CultureInfo.InvariantCulture,
          "<test-suite type=\"Test Project\" name=\"\" executed=\"True\" result=\"{0}\" time=\"{1:F6}\" asserts=\"{2}\" />",
          Result,
          Time,
          Asserts));
    }
  }
}