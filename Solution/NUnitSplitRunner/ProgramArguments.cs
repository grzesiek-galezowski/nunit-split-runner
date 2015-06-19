using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;

namespace NUnitSplitRunner
{
  public class ProgramArguments
  {
    private readonly IEnumerable<string> _args;

    public ProgramArguments(IEnumerable<string> args)
    {
      _args = args;
    }

    private static bool IsAssemblyPath(AnyPathWithFileName arg)
    {
      return arg.Has(FileExtension.Value(".dll"));
    }

    public void SplitInto(List<AnyPathWithFileName> dlls, RealRunnerInvocationOptions remainingTargetCommandline)
    {
      foreach (var arg in _args.Skip(1))
      {
        var dllPath = AnyPathWithFileName.Value(arg);
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