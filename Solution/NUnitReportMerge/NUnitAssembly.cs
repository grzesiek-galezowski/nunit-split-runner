using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitAssembly
  {
    private readonly XElement _xElement;

    public NUnitAssembly(XElement xElement)
    {
      _xElement = xElement;
    }
  }
}