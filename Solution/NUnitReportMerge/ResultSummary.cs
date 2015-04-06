using System;
using System.Globalization;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class ResultSummary
  {
    private readonly XElement _element;

    public ResultSummary(XElement initelement)
    {
      _element = initelement;
    }

    public DateTime DateTimeValue()
    {
      return DateTime.Parse(String.Concat(_element.Attribute("date").Value, " ", _element.Attribute("time").Value), CultureInfo.InvariantCulture);
    }

    public int Invalid()
    {
      return XmlCulture.GetInt(_element.Attribute("invalid").Value);
    }

    public int Skipped()
    {
      return XmlCulture.GetInt(_element.Attribute("skipped").Value);
    }

    public int Ignored()
    {
      return XmlCulture.GetInt(_element.Attribute("ignored").Value);
    }

    public int Inconclusive()
    {
      return XmlCulture.GetInt(_element.Attribute("inconclusive").Value);
    }

    public int NotRun()
    {
      return XmlCulture.GetInt(_element.Attribute("not-run").Value);
    }

    public int Failures()
    {
      return XmlCulture.GetInt(_element.Attribute("failures").Value);
    }

    public int Errors()
    {
      return XmlCulture.GetInt(_element.Attribute("errors").Value);
    }

    public int Total()
    {
      return XmlCulture.GetInt(_element.Attribute("total").Value);
    }
  }
}