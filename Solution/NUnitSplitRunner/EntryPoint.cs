using System;
using System.Configuration;

namespace NUnitSplitRunner
{
  static internal class EntryPoint
  {
    public static void Main(string[] args)
    {
      var runner = Runner.Create(args, LoadMaxAllowedAssemblyCountPerRun(), args[0], LoadMaxDegreeOfParallelism());
      runner.Run();
    }

    public static int LoadMaxDegreeOfParallelism()
    {
      return Int32.Parse(ConfigurationManager.AppSettings["MaxDegreeOfParallelism"]); 
    }

    public static int LoadMaxAllowedAssemblyCountPerRun()
    {
      return Int32.Parse(ConfigurationManager.AppSettings["MaxAssemblyCountPerRun"]);
    }
  }
}