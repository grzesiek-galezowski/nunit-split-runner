using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnitReportMerge.Model;

namespace NUnitReportMerge
{
  //bug refactor all of it
  public class NUnitReportFactory
  {
    public static NUnitReport CreateFrom(IEnumerable<SingleRunReport> list)
    {
      var firstRunReport = list.First();

      var fullReport = new NUnitReport(
        firstRunReport, new NUnitAssemblies(Enumerable.Empty<XElement>()));

      foreach (var nextRunReport in list)
      {
        nextRunReport.AssertIsFromTheSameRunAs(firstRunReport);
        fullReport.AnnounceMergeWith(nextRunReport);
        fullReport.Add(nextRunReport);
      }

      return fullReport;
    }
  }
}