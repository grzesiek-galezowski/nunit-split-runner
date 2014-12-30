using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitReport
  {
    public static Tuple<NUnitResultSummary, NUnitEnvironment, NUnitCulture, NUnitAssemblies> Fold(IEnumerable<XDocument> docs)
    {
      var xDocument = docs.First();

      var resultSummary = NUnitResultSummary.From(xDocument);
      var environment = NUnitEnvironment.ExtractFrom(xDocument);
      var culture = NUnitCulture.ExtractFrom(xDocument);
      var noElementsAtFirst = new NUnitAssemblies(Enumerable.Empty<XElement>());

      var state = Tuple.Create(resultSummary, environment, culture, noElementsAtFirst);

      return docs.Aggregate(state, Folder);
    }

    private static Tuple<NUnitResultSummary, NUnitEnvironment, NUnitCulture, NUnitAssemblies> Folder(
      Tuple<NUnitResultSummary, NUnitEnvironment, NUnitCulture, NUnitAssemblies> state, XDocument xDoc)
    {
      var currentSummary = state.Item1;
      var nUnitEnvironment = state.Item2;
      var nUnitCulture = state.Item3;
      var assemblies = state.Item4;

      Validations.CheckReportsCoherence(xDoc, nUnitEnvironment, nUnitCulture);

      return Tuple.Create(
        NUnitResultSummary.From(xDoc).MergeWith(currentSummary),
        nUnitEnvironment,
        nUnitCulture,
        assemblies.UnionWithOthers(NUnitAssemblies.From(xDoc)));
    }
  }
}