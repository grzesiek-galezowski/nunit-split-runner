using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public static class XmlReportFiles
  {
    public static ReportDocument[] LoadFrom(string directory, string filter)
    {
      var files = Directory.GetFiles(directory, filter, SearchOption.AllDirectories);
      Console.WriteLine("Found reports: " + String.Join(", ", files));
      return files.Select(fileName => XDocument.Parse(File.ReadAllText(fileName))).Select(d => new ReportDocument(d)).ToArray();
    }
  }
}