using System.Xml.Linq;

namespace NUnitReportMerge.Model
{
  public class NUnitAssembly
  {
    readonly XElement _xElement;
    readonly double _time;
    readonly int _asserts;
    readonly bool _isFailure;
    readonly bool _isInconclusive;

    //bug refactor xElement away
    public NUnitAssembly(XElement xElement, double time, int asserts, bool isFailure, bool isInconclusive)
    {
      _xElement = xElement;
      _time = time;
      _asserts = asserts;
      _isFailure = isFailure;
      _isInconclusive = isInconclusive;
    }

    public void AddTo(TestSuiteBuilder testSuiteBuilder)
    {
      testSuiteBuilder.Add(_xElement, _time, _asserts, _isFailure, _isInconclusive);
    }
  }
}