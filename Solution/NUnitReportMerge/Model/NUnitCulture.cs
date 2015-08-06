using System;

namespace NUnitReportMerge
{
  public class NUnitCulture
  {
    public string CurrentCulture { private get; set; }
    public string CurrentUiCulture { private get; set; }

    public void AddTo(OutResultsBuilder outResultsBuilder)
    {
      outResultsBuilder.AddCultureInfo(CurrentCulture, CurrentUiCulture);
    }
  }
}