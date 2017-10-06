using System;
using Rhino.Mocks;
using GalaSoft.MvvmLight.Ioc;

namespace UnitTestProject
{
    public class UnitTest1
    {
        
        public void TestMethod1()
        {
            //DeviceObjectReader dor = new DeviceObjectReader();
            //var _mockTranslationMaster = MockRepository.GeneratePartialMock<ITranslationMaster>();
            //ITranslationMaster translationMaster = dor.GetTranslationsFromDeviceXml(@"C:\ProgramData\PDQ\Devices\4099\1629 0003\3.13.1.1\device.xml");

            //var _mockLanguageStringCollection = MockRepository.GenerateMock<ILanguageStringCollection>();
            //_mockLanguageStringCollection.Stub(l => l.LanguageName).Return(new System.Globalization.CultureInfo("fr"));
            //_mockLanguageStringCollection.Stub(l => l.LanguageName).Return(new System.Globalization.CultureInfo("fr"));

            //var _mockLanguageFileGenerator = MockRepository.GenerateMock<ILanguageFileGenerator>();

            //_mockLanguageFileGenerator.CreateLanguageFile(1, _mockLanguageStringCollection);

            //ITranslationMaster tlm = new Tr

            //// Rhino.Mocks.Mocker<ICloneable>
        }
    }
    public interface IProvider { }
    public abstract class BaseProvider : IProvider { }
    public class Provider1 : BaseProvider { }
    public class Provider2 : BaseProvider { }

    //[Test]
    //public void RegisterTwoImplementations_GetAllInstances_ReturnsBothInstances()
    //{
    //    SimpleIoc.Default.Register<Provider1>();
    //    SimpleIoc.Default.Register<Provider2>();

    //    SimpleIoc.Default.Register<BaseProvider>(() =>
    //            SimpleIoc.Default.GetInstance<Provider1>(), "a");

    //    SimpleIoc.Default.Register<BaseProvider>(() =>
    //            SimpleIoc.Default.GetInstance<Provider2>(), "b");

    //    var result = SimpleIoc.Default.GetAllInstances<BaseProvider>();

    //    Assert.That(result, Is.Not.Null);
    //    Assert.That(result.Count(), Is.EqualTo(2));
    //    Assert.That(result.Any(x => x.GetType() == typeof(Provider1)), Is.True);
    //    Assert.That(result.Any(x => x.GetType() == typeof(Provider2)), Is.True);
    //}
}
