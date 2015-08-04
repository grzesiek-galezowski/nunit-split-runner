using System;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitCulture
  {
    public string CurrentCulture { private get; set; }
    public string CurrentUiCulture { private get; set; }

    public void AddTo(XElement results)
    {
      results.Add(XElement.Parse(XmlCulture.Format(
        "<culture-info current-culture=\"{0}\" current-uiculture=\"{1}\" />",
        CurrentCulture,
        CurrentUiCulture)));
    }
  }
}