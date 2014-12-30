using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitEnvironment
  {
    private string NUnitVersion { get; set; }
    private string ClrVersion { get; set; }
    private string OsVersion { get; set; }
    private string Platform { get; set; }
    private string Cwd { get; set; }
    private string MachineName { get; set; }
    private string User { get; set; }
    private string UserDomain { get; set; }

    public static NUnitEnvironment ExtractFrom(XDocument xDoc)
    {
      var env = TestResultsEnvironment.ExtractFrom(xDoc);
      var actualEnv = new NUnitEnvironment
      {
        NUnitVersion = env.NUnitVersion(),
        ClrVersion = env.ClrVersion(),
        OsVersion = env.OsVersion(),
        Platform = env.PlatformVersion(),
        Cwd = env.Cwd(),
        MachineName = env.MachineName(),
        User = env.User(),
        UserDomain = env.UserDomain()
      };

      return actualEnv;
    }

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