using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AtmaFileSystem;

namespace NUnitReportMerge
{
  public static class XmlReportFiles
  {
    public static ReportDocument[] LoadFrom(DirectoryName directory, string filter)
    {
      var files = Directory.GetFiles(directory.ToString(), filter, SearchOption.AllDirectories);
      Console.WriteLine("Found reports: " + String.Join(", ", files));
      return files.Select(fileName => XDocument.Parse(File.ReadAllText(fileName)))
        .Select(d => new ReportDocument(d)).ToArray();
    }
  }
}