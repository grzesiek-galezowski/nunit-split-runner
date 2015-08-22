using CommandLine;
using CommandLine.Text;
using Microsoft.Build.Framework;

namespace NUnitSplitRunner
{
  public class Options
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

    [Option("runnerArgs", Required = true, HelpText = "Arguments for NUnit runner")]
    public string RunnerArgs { get; set; }

    [Option("runnerPath", Required = true, HelpText = "Absolute path to third party test runner")]
    public string RunnerPath { get; set; }


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