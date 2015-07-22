using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;

namespace NUnitSplitRunner.Input
{
  public class ProgramArguments
  {
    private readonly IEnumerable<string> _args;

    public ProgramArguments(IEnumerable<string> args)
    {
      _args = args;
    }

    private static bool IsAssemblyPath(AnyFilePath arg)
    {
      return arg.Has(FileExtension.Value(".dll"));
    }

    public void SplitInto(List<AnyFilePath> dlls, RealRunnerInvocationOptions remainingTargetCommandline)
    {
      foreach (var arg in _args.Skip(1))
      {
        var dllPath = AnyFilePath.Value(arg);
        if (IsAssemblyPath(dllPath))
        {
          dlls.Add(dllPath);
        }
        else
        {
          remainingTargetCommandline.Add(arg);
        }
      }
    }
  }
}