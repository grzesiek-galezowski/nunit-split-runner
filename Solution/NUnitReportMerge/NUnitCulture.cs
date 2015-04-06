using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitCulture
  {
    public string CurrentCulture { private get; set; }
    public String CurrentUiCulture { private get; set; }

    public XElement Xml()
    {
      return XElement.Parse(XmlCulture.Format(
        "<culture-info current-culture=\"{0}\" current-uiculture=\"{1}\" />",
        CurrentCulture,
        CurrentUiCulture));
    }
  }
}