using System.Collections.Generic;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitReport
  {
    private readonly NUnitResultSummary _summary;
    private readonly NUnitEnvironment _nUnitEnvironment;
    private readonly NUnitCulture _nUnitCulture;
    private readonly NUnitAssemblies _assemblies;

    public NUnitReport(NUnitResultSummary summary, NUnitEnvironment nUnitEnvironment, NUnitCulture nUnitCulture, NUnitAssemblies assemblies)
    {
      _summary = summary;
      _nUnitEnvironment = nUnitEnvironment;
      _nUnitCulture = nUnitCulture;
      _assemblies = assemblies;
    }

    private NUnitResultSummary Summary
    {
      get { return _summary; }
    }

    private NUnitEnvironment NUnitEnvironment
    {
      get { return _nUnitEnvironment; }
    }

    private NUnitCulture NUnitCulture
    {
      get { return _nUnitCulture; }
    }

    private NUnitAssemblies Assemblies
    {
      get { return _assemblies; }
    }

    public XElement MergeAsXml()
    {
      var results = Summary.Xml();
      results.Add(NUnitEnvironment.Xml(), NUnitCulture.Xml(), Assemblies.Xml());
      return results;
    }
  }
}