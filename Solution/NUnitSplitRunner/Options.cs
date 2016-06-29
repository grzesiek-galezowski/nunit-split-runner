using System;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;
using Microsoft.Build.Framework;

namespace NUnitSplitRunner
{
  public interface IOptions
  {
    int ParallelProcesses { get; }
    int AssembliesPerRun { get; }
    TimeSpan NunitTimeout { get; }
    string RunnerArgs { get; }
    string RunnerPath { get; }

    void DumpTo(ILogger logger);
  }

  public class Options : IOptions
  {
    [Option(
      "parallelProcesses",
      Required = false,
      HelpText = "Max number of NUnit processes to run at the same time.",
      DefaultValue = 1)]
    public int ParallelProcesses { get; set; }

    [Option("assembliesPerRun",
      Required = false,
      HelpText = "Maximum number of assemblies per NUnit run.",
      DefaultValue = 10)]
    public int AssembliesPerRun { get; set; }

    public TimeSpan NunitTimeout
    {
      get { return TimeSpan.FromSeconds(NunitTimeoutSeconds); }
    }

    [Option("nunitTimeoutSeconds",
      Required = false,
      HelpText = "A timeout in seconds for single Nunit process to run.",
      DefaultValue = 60 * 5)]
    public int NunitTimeoutSeconds { get; set; }

    [Option("runnerArgs", Required = true, HelpText = "Arguments for NUnit runner")]
    public string RunnerArgs { get; set; }

    [Option("runnerPath", Required = true, HelpText = "Absolute path to third party test runner")]
    public string RunnerPath { get; set; }

    private PropertyInfo[] _propertyInfos;

    public void DumpTo(ILogger logger)
    {
      logger.Info("Showing run arguments.");
      if (_propertyInfos == null)
        _propertyInfos = GetType().GetProperties();

      foreach (var info in _propertyInfos)
      {
        var value = info.GetValue(this, null) ?? "<null>";
        logger.Info(info.Name + ": " + value);
      }
    }


    [HelpOption]
    public string GetUsage()
    {
      var help = new HelpText
      {
        Heading = new HeadingInfo("NUnit Split Runner", "1.0"),
        Copyright = new CopyrightInfo("Grzegorz Ga³êzowski", 2015),
        AdditionalNewLineAfterOption = true,
        AddDashesToOption = true
      };
      help.AddPreOptionsLine("Licensed under MIT license");
      help.AddOptions(this);

      return help.ToString();
    }
  }
}