using System.Xml.Linq;

namespace NUnitReportMerge.Input
{
  public class TestResultsEnvironment
  {
    readonly XElement _env;

    public TestResultsEnvironment(XElement env)
    {
      _env = env;
    }

    public string UserDomain()
    {
      return _env.Attribute("user-domain").Value;
    }

    public string User()
    {
      return _env.Attribute("user").Value;
    }

    public string MachineName()
    {
      return _env.Attribute("machine-name").Value;
    }

    public string Cwd()
    {
      return _env.Attribute("cwd").Value;
    }

    public string PlatformVersion()
    {
      return _env.Attribute("platform").Value;
    }

    public string OsVersion()
    {
      return _env.Attribute("os-version").Value;
    }

    public string ClrVersion()
    {
      return _env.Attribute("clr-version").Value;
    }

    public string NUnitVersion()
    {
      return _env.Attribute("nunit-version").Value;
    }
  }
}