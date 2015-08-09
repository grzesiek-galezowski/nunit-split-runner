using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class TestResultsCulture
  {
    readonly XElement _cultureElement;

    public TestResultsCulture(XElement cultureElement)
    {
      _cultureElement = cultureElement;
    }

    public string CurrentCulture()
    {
      return _cultureElement.Attribute("current-culture").Value;
    }

    public string CurrentUiCulture()
    {
      return _cultureElement.Attribute("current-uiculture").Value;
    }
  }
}