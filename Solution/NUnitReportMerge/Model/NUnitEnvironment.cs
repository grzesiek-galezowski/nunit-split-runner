using NUnitReportMerge.Input;
using NUnitReportMerge.Out;

namespace NUnitReportMerge.Model
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

    public void AddTo(OutResultsBuilder outResultsBuilder)
    {
      outResultsBuilder.AddEnvironmentInformation(
        NUnitVersion, 
        ClrVersion, 
        OsVersion, 
        Platform, 
        Cwd, 
        MachineName, 
        User, UserDomain);
    }

    public static NUnitEnvironment From(TestResultsEnvironment testResultsEnvironment)
    {
      return new NUnitEnvironment
      {
        NUnitVersion = testResultsEnvironment.NUnitVersion(),
        ClrVersion = testResultsEnvironment.ClrVersion(),
        OsVersion = testResultsEnvironment.OsVersion(),
        Platform = testResultsEnvironment.PlatformVersion(),
        Cwd = testResultsEnvironment.Cwd(),
        MachineName = testResultsEnvironment.MachineName(),
        User = testResultsEnvironment.User(),
        UserDomain = testResultsEnvironment.UserDomain()
      };
    }
  }
}