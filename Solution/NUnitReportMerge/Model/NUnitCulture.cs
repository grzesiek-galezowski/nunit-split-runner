using NUnitReportMerge.Out;

namespace NUnitReportMerge.Model
{
  public class NUnitCulture
  {
    public string CurrentCulture { private get; set; }
    public string CurrentUiCulture { private get; set; }

    public void AddTo(OutResultsBuilder outResultsBuilder)
    {
      outResultsBuilder.AddCultureInfo(CurrentCulture, CurrentUiCulture);
    }

    public static NUnitCulture From(TestResultsCulture testResultsCulture)
    {
      return new NUnitCulture
      {
        CurrentCulture = testResultsCulture.CurrentCulture(),
        CurrentUiCulture = testResultsCulture.CurrentUiCulture()
      };
    }
  }
}