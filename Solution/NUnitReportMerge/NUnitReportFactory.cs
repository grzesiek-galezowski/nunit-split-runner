using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnitReportMerge.Model;

namespace NUnitReportMerge
{
  //bug refactor all of it
  public static class NUnitReportFactory
  {
    public static NUnitReport CreateFrom(IEnumerable<SingleRunReport> list)
    {
      var fullReport = NUnitReport.FullReport(list.First());

      foreach (var nextRunReport in list)
      {
        fullReport.AssertIsFromTheSameRunAs(nextRunReport);
        fullReport.AnnounceMergeWith(nextRunReport);
        fullReport.Add(nextRunReport);
      }

      return fullReport;
    }
  }
}