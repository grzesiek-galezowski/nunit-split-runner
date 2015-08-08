using System;
using System.IO;
using System.Xml.Linq;
using AtmaFileSystem;
using NUnitReportMerge.Out;

namespace NUnitReportMerge
{
  static public class Api
  {
    public static void WriteMergedNunitResults(string directory, string filter, string outfile)
    {
      var reports = XmlReportFiles.LoadFrom(DirectoryName.Value(directory), filter);
      var state = NUnitReportFactory.CreateFrom(reports);
      var builder = new OutResultsBuilder();
      state.Xml(builder);
      File.WriteAllText(outfile, builder.Build().ToString());
    }
  }
}