using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitReportFactory
  {
    private static Tuple<NUnitResultSummary, NUnitEnvironment, NUnitCulture, NUnitAssemblies> Fold(IEnumerable<ReportDocument> docs)
    {
      var report = docs.First();

      var resultSummary = report.NUnitSummary();
      var environment = report.Environment();
      var culture = report.Culture();
      var noElementsAtFirst = new NUnitAssemblies(Enumerable.Empty<XElement>());

      var state = Tuple.Create(resultSummary, environment, culture, noElementsAtFirst);

      return docs
        .Aggregate(state, Folder);
    }

    private static Tuple<NUnitResultSummary, NUnitEnvironment, NUnitCulture, NUnitAssemblies> Folder(
      Tuple<NUnitResultSummary, NUnitEnvironment, NUnitCulture, NUnitAssemblies> state, ReportDocument xDoc)
    {
      var currentSummary = state.Item1;
      var nUnitEnvironment = state.Item2;
      var nUnitCulture = state.Item3;
      var assemblies = state.Item4;

      Validations.CheckReportsCoherence(xDoc, nUnitEnvironment, nUnitCulture);

      Console.WriteLine("Merging " + xDoc.NUnitSummary().Total + " existing tests with  " + currentSummary.Total);
      assemblies.JoinWith(xDoc.Assemblies());
      return Tuple.Create(
        xDoc.NUnitSummary().MergeWith(currentSummary),
        nUnitEnvironment,
        nUnitCulture,
        assemblies);
    }

    public static NUnitReport CreateFrom(IEnumerable<ReportDocument> list)
    {
      var tuple = Fold(list);
      var nUnitRunInfo = new NUnitReport(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
      return nUnitRunInfo;
    }
  }
}