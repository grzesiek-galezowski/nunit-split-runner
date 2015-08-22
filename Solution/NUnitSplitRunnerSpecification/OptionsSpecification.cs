using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using NUnit.Framework;
using NUnitSplitRunner;

namespace NUnitSplitRunnerSpecification
{
  public class OptionsSpecification
  {
    [Test]
    public void ShouldParseOwnOptionsAndLeaveNUnitOnesAlone()
    {
      //GIVEN
      var options = new Options();
      // Parse in 'strict mode', success or quit

      //WHEN - THEN
      var parser = Parser.Default;
      const int parallelProcesses = 10;
      const int assembliesPerRun = 15;
      const string runnerArgs = "\"runall blabla lol=12333\"";
      const string runnerPath = "aaaaaaaa";

      Assert.True(parser.ParseArgumentsStrict(
          new[]
          {
            "--parallelProcesses", parallelProcesses.ToString(),
            "--assembliesPerRun", assembliesPerRun.ToString(),
            "--runnerPath", runnerPath,
            "--runnerArgs", runnerArgs
          }, options));

      Assert.AreEqual(parallelProcesses, options.ParallelProcesses);
      Assert.AreEqual(assembliesPerRun, options.AssembliesPerRun);
      Assert.AreEqual(runnerArgs, options.RunnerArgs);
      Assert.AreEqual(runnerPath, options.RunnerPath);
    }

  }
}
