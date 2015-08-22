using System;
using System.Configuration;
using System.Text;

namespace NUnitSplitRunner
{
  static internal class EntryPoint
  {
    public static void Main(string[] args)
    {
      var options = new Options();

      CommandLine.Parser.Default.ParseArgumentsStrict(args, options);

      var runner = Runner.Create(
        new[] {options.RunnerArgs}, 
        options.AssembliesPerRun, 
        options.RunnerPath, 
        options.ParallelProcesses);
      runner.Run();
    }

  }
}