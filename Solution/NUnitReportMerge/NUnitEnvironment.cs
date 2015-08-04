using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitEnvironment
  {
    public string NUnitVersion { private get; set; }
    public string ClrVersion { private get; set; }
    public string OsVersion { private get; set; }
    public string Platform { private get; set; }
    public string Cwd { private get; set; }
    public string MachineName { private get; set; }
    public string User { private get; set; }
    public string UserDomain { private get; set; }

    public void AddTo(XElement results)
    {
      results.Add(XElement.Parse(XmlCulture.Format(
        "<environment nunit-version=\"{0}\" clr-version=\"{1}\" os-version=\"{2}\" platform=\"{3}\" cwd=\"{4}\" machine-name=\"{5}\" user=\"{6}\" user-domain=\"{7}\" />",
        NUnitVersion,
        ClrVersion,
        OsVersion,
        Platform,
        Cwd,
        MachineName,
        User,
        UserDomain)));
    }
  }
}