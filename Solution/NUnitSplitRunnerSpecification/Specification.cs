using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnitSplitRunner;

namespace NUnitSplitRunnerSpecification
{
  public class Specification
  {
    [Test, Explicit]
    public void ShouldPassSimpleSanityCheck()
    {
      //GIVEN
      Console.WriteLine(Directory.GetCurrentDirectory());
      string[] args = new[]
      {
        @"C:\Users\astral\Documents\GitHub\nunit-split-runner\Solution\packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe",
        //@"C:\Users\cid\Documents\nunit-split-runner\Solution\packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe",
        "./ClassLibrary1.dll",
        "./ClassLibrary2.dll",
        "./ClassLibrary3.dll",
        "./ClassLibrary4.dll",
        "./ClassLibrary5.dll",
      };
      var runner = Runner.Create(args, 1, args[0], 4);

      //WHEN
      runner.Run();

      //THEN
      NUnitReportAssertions.AssertReportsEqual("GoldenMasterTestResult.xml", "TestResult.xml");
    }
  }
}
