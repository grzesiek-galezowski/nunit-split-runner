using System.Collections.Generic;
using System.Linq;
using NUnitSplitRunner;

internal class ProgramArguments
{
  private IEnumerable<string> _args;

  public ProgramArguments(string[] args)
  {
    _args = args;
  }

  private static bool IsAssemblyPath(string arg)
  {
    return arg.EndsWith(".dll");
  }

  public void SplitInto(ICollection<string> dlls, RealRunnerInvocationOptions remainingTargetCommandline)
  {
    foreach (var arg in _args.Skip(1))
    {
      if (IsAssemblyPath(arg))
      {
        dlls.Add(arg);
      }
      else
      {
        remainingTargetCommandline.Add(arg);
      }
    }
  }
}