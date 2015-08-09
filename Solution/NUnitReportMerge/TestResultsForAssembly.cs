using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class TestResultsForAssembly
  {
    readonly double _time;
    readonly int _asserts;
    readonly bool _isFailure;
    readonly bool _isInconclusive;

    public double Time()
    {
      return _time;
    }

    public int Asserts()
    {
      return _asserts;
    }

    public bool IsFailure()
    {
      return _isFailure;
    }

    public bool IsInconclusive()
    {
      return _isInconclusive;
    }

    public TestResultsForAssembly(XElement r)
    {
      _time = XmlCulture.GetDouble(r.Attribute("time").Value);
      _asserts = XmlCulture.GetInt(r.Attribute("asserts").Value);
      _isFailure = IsFailure(r);
      _isInconclusive = IsInconclusive(r);
    }

    static bool IsInconclusive(XElement xElement)
    {
      return xElement.Attribute("result").Value == "Inconclusive";
    }

    static bool IsFailure(XElement xElement)
    {
      return xElement.Attribute("result").Value == "Failure";
    }
  }
}