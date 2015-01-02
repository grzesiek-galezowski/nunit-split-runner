using System.Reflection;
using NUnit.Framework;

namespace ClassLibrary3
{
    public class Tests
    {
      [Test]
      public void Inconclusive()
      {
        Assert.Inconclusive(Message(MethodBase.GetCurrentMethod().Name));
      }

      [Test]
      public void Failed()
      {
        Assert.Fail(Message(MethodBase.GetCurrentMethod().Name));
      }

      [Test]
      public void Passed()
      {
        Assert.Pass(Message(MethodBase.GetCurrentMethod().Name));
      }

      [Test]
      public void Ignored()
      {
        Assert.Ignore(Message(MethodBase.GetCurrentMethod().Name));
      }

      private static string Message(string status)
      {
        return "this is " + status + " test from " + Assembly.GetExecutingAssembly().FullName;
      }

    }


}
