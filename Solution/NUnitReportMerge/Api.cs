using System;
using System.IO;
using System.Xml.Linq;
using AtmaFileSystem;

namespace NUnitReportMerge
{
  static public class Api
  {
    public static void WriteMergedNunitResults(string directory, string filter, string outfile)
    {
      var reports = XmlReportFiles.LoadFrom(DirectoryName.Value(directory), filter);
      var state = NUnitReportFactory.CreateFrom(reports);
      var mergedReport = state.MergeAsXml();
      File.WriteAllText(outfile, mergedReport.ToString());
    }
  }
}