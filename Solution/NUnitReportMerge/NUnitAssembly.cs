using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitAssembly //bug make sure it is needed or remove it
  {
    private readonly XElement _xElement;

    public NUnitAssembly(XElement xElement)
    {
      _xElement = xElement;
    }
  }
}