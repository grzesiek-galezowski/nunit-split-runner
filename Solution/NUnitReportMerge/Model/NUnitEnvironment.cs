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
  }
}