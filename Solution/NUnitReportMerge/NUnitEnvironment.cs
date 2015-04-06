using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitEnvironment
  {
    public string NUnitVersion { get; set; }
    public string ClrVersion { get; set; }
    public string OsVersion { get; set; }
    public string Platform { get; set; }
    public string Cwd { get; set; }
    public string MachineName { get; set; }
    public string User { get; set; }
    public string UserDomain { get; set; }

    public XElement Xml()
    {
      return XElement.Parse(XmlCulture.Format(
        "<environment nunit-version=\"{0}\" clr-version=\"{1}\" os-version=\"{2}\" platform=\"{3}\" cwd=\"{4}\" machine-name=\"{5}\" user=\"{6}\" user-domain=\"{7}\" />",
        NUnitVersion,
        ClrVersion,
        OsVersion,
        Platform,
        Cwd,
        MachineName,
        User,
        UserDomain));
    }
  }
}