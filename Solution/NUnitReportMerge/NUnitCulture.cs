using System;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitCulture
  {
    private string CurrentCulture { get; set; }
    private String CurrentUiCulture { get; set; }

    public static NUnitCulture ExtractFrom(XDocument xDoc)
    {
      var cultureElement = xDoc.Element("test-results").Element("culture-info");

      var culture = new NUnitCulture
      {
        CurrentCulture = cultureElement.Attribute("current-culture").Value,
        CurrentUiCulture = cultureElement.Attribute("current-uiculture").Value
      };

      return culture;
    }

    public XElement Xml()
    {
      return XElement.Parse(XmlCulture.Format(
        "<culture-info current-culture=\"{0}\" current-uiculture=\"{1}\" />",
        CurrentCulture,
        CurrentUiCulture));
    }
  }
}