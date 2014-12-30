using System;
using System.Globalization;

namespace NUnitReportMerge
{
  public class XmlCulture
  {
    public static int GetInt(string str)
    {
      return Convert.ToInt32(str, CultureInfo.InvariantCulture);
    }

    public static double GetDouble(string str)
    {
      return Convert.ToDouble(str, CultureInfo.InvariantCulture);
    }

    public static string Format(string format, params object[] values)
    {
      return string.Format(CultureInfo.InvariantCulture, format, values);
    }
  }
}