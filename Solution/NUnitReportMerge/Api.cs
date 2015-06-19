using System;
using System.IO;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  static public class Api
  {
    public static void WriteMergedNunitResults(string directory, string filter, string outfile)
    {
      var reports = XmlReportFiles.LoadFrom(directory, filter);
      var state = NUnitReportFactory.CreateFrom(reports);
      var mergedReport = state.MergeAsXml();
      File.WriteAllText(outfile, mergedReport.ToString());
    }
  }
}