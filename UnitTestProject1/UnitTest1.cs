using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        //[TestMethod]
        //public void RegisterTwoImplementations_GetAllInstances_ReturnsBothInstances()
        //{
        //    SimpleIoc.Default.Register<Provider1>();
        //    SimpleIoc.Default.Register<Provider2>();

        //    SimpleIoc.Default.Register<BaseProvider>(() =>
        //            SimpleIoc.Default.GetInstance<Provider1>(), "a");

        //    SimpleIoc.Default.Register<BaseProvider>(() =>
        //            SimpleIoc.Default.GetInstance<Provider2>(), "b");

        //    var result = SimpleIoc.Default.GetAllInstances<BaseProvider>();

        //    Assert.AreSame(result, !null);
        //    Assert.That(result.Count(), Is.EqualTo(2));
        //    Assert.That(result.Any(x => x.GetType() == typeof(Provider1)), Is.True);
        //    Assert.That(result.Any(x => x.GetType() == typeof(Provider2)), Is.True);
        //}

    }
    public interface IProvider { }
    public abstract class BaseProvider : IProvider { }
    public class Provider1 : BaseProvider { }
    public class Provider2 : BaseProvider { }

}
