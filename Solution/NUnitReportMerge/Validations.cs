using System;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  static internal class Validations
  {
    public static void CheckReportsCoherence(ReportDocument xDoc, NUnitEnvironment nUnitEnvironment, NUnitCulture nUnitCulture)
    {
// Sanity check!
      if (nUnitEnvironment != xDoc.Environment() || nUnitCulture != xDoc.Culture())
      {
        Console.WriteLine(
          "Unmatched environment and/or cultures detected: some of theses results files are not from the same test run.");
      }
    }
  }
}