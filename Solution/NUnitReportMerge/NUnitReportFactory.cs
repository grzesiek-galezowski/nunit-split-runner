using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  //bug refactor all of it
  public class NUnitReportFactory
  {
    public static NUnitReport CreateFrom(IEnumerable<ReportDocument> list)
    {
      var report = list.First();

      return ToNUnitRunReport(list, report);
    }

    private static NUnitReport ToNUnitRunReport(IEnumerable<ReportDocument> list, ReportDocument report)
    {
      var resultSummary = report.NUnitSummary();
      var environment = report.Environment();
      var culture = report.Culture();
      var noElementsAtFirst = new NUnitAssemblies(Enumerable.Empty<XElement>());

      foreach (var item in list)
      {
        var currentSummary = resultSummary;
        var nUnitEnvironment = environment;
        var nUnitCulture = culture;
        var assemblies = noElementsAtFirst;

        Validations.CheckReportsCoherence(item, nUnitEnvironment, nUnitCulture);

        Console.WriteLine(
          "Merging " + item.NUnitSummary().Total + 
          " existing tests with  " + currentSummary.Total);

        item.AddTo(assemblies);

        resultSummary = item.MergeWith(currentSummary);
        environment = nUnitEnvironment;
        culture = nUnitCulture;
        noElementsAtFirst = assemblies;


      }

      var nUnitRunInfo = new NUnitReport(resultSummary, environment, culture, noElementsAtFirst);
      return nUnitRunInfo;
    }
  }
}