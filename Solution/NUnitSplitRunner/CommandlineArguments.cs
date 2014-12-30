namespace NUnitSplitRunner
{
  public class CommandlineArguments
  {
    private string _content = string.Empty;

    public void Add(string arg)
    {
      _content += " " + arg;
    }

    public override string ToString()
    {
      return _content;
    }
  }
}