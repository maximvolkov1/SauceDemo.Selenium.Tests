using Allure.Net.Commons;
using Allure.NUnit;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using SauceDemo.Selenium.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]

namespace SauceDemo.Selenium.Tests.Drivers
{
    [AllureNUnit]
    public class BaseTest
    {
        protected IWebDriver? _driver;
        protected IConfiguration? _configuration;
        protected BasePage BasePage { get; private set; }
        protected LoginPage LoginPage { get; private set; }
        protected ProductPage ProductPage { get; private set; }
        protected CartPage CartPage { get; private set; }
        protected CheckoutInformationPage CheckoutInformationPage { get; private set; }
        protected CheckoutOverviewPage CheckoutOverviewPage { get; private set; }
        protected CheckoutCompletePage CheckoutCompletePage { get; private set; }

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string baseUrl = _configuration["TestConfig:BaseUrl"] ?? "https://www.saucedemo.com";
            string gridUrl = _configuration["TestConfig:GridUrl"] ?? "http://localhost:4444/wd/hub";
            string browser = _configuration["TestConfig:Browser"] ?? "Firefox";

            DriverOptions options = browser.ToLower() == "chrome"
                ? new ChromeOptions()
                : new FirefoxOptions();

            string testName = TestContext.CurrentContext.Test.Name;
            string cleanTestName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
            string videoFileName = $"{cleanTestName}_{DateTime.Now:yyyyMMdd_HHmmss}.mp4";

            var selenoidOptions = new Dictionary<string, object>
            {
                { "enableVideo", true },          
                { "videoName", videoFileName }    
            };
            options.AddAdditionalOption("selenoid:options", selenoidOptions);

            _driver = new RemoteWebDriver(new Uri(gridUrl), options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(baseUrl);

            BasePage = new BasePage(_driver);
            LoginPage = new LoginPage(_driver);
            ProductPage = new ProductPage(_driver);
            CartPage = new CartPage(_driver);
            CheckoutInformationPage = new CheckoutInformationPage(_driver);
            CheckoutOverviewPage = new CheckoutOverviewPage(_driver);
            CheckoutCompletePage = new CheckoutCompletePage(_driver);
        }

        [TearDown]
        public void Teardown()
        {
            if (_driver != null)
            {
                try
                {
                    if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
                    {
                        TakeScreenshotOfFailure();
                        AttachVideoUrlOfFailure();
                    }
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"[ОШИБКА] Не удалось собрать артефакты падения: {ex.Message}");
                }
                finally
                {
                    _driver?.Quit();
                    _driver?.Dispose();
                    _driver = null;
                }
            }
        }

        private void TakeScreenshotOfFailure()
        {
            var screenshotDriver = (ITakesScreenshot)_driver!;
            Screenshot screenshot = screenshotDriver.GetScreenshot();

            string testName = TestContext.CurrentContext.Test.Name;
            string cleanTestName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
            string fileName = $"{cleanTestName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            screenshot.SaveAsFile(path);

            AllureApi.AddAttachment("Скриншот падения теста", "image/png", path);

            TestContext.WriteLine($"[ВНИМАНИЕ] Тест упал! Скриншот сохранен по пути: {path}");
        }

        private void AttachVideoUrlOfFailure()
        {
            try
            {
                if (_driver is RemoteWebDriver remoteDriver)
                {
                    string sessionId = remoteDriver.SessionId.ToString();

                    string gridUrl = _configuration?["TestConfig:GridUrl"] ?? "http://localhost:4444/wd/hub";
                    var uri = new Uri(gridUrl);

                    string videoUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}/video/{sessionId}.mp4";

                    AllureApi.AddAttachment("Видеозапись упавшего теста (Ссылка)", "text/uri-list", videoUrl);
                    TestContext.WriteLine($"[ВНИМАНИЕ] Ссылка на видеозапись добавлена в Allure: {videoUrl}");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"[ПРЕДУПРЕЖДЕНИЕ] Не удалось сгенерировать ссылку на видео Selenoid: {ex.Message}");
            }
        }
        protected Action LoginAndVerify => () =>
        {
            LoginPage.LoginAsValidUser("standard_user", "secret_sauce");
            Assert.That(ProductPage.GetTitleText(), Is.EqualTo("Products"), "Ошибка предсловия!");
        };
    }
}


