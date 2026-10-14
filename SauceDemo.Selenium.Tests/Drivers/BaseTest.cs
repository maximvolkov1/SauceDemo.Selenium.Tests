using Allure.Net.Commons;
using Allure.NUnit;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
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

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            string baseUrl = _configuration["TestConfig:BaseUrl"] ?? "https://saucedemo.com";
            string gridUrl = _configuration["TestConfig:GridUrl"] ?? "http://localhost:4444/wd/hub";
            string browser = _configuration["TestConfig:Browser"] ?? "Firefox";

            DriverOptions options = browser.ToLower() == "chrome"
                ? new ChromeOptions()
                : new FirefoxOptions();

            _driver = new RemoteWebDriver(new Uri(gridUrl), options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Window.Maximize();

            _driver.Navigate().GoToUrl(baseUrl);
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
                    }
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"[ОШИБКА] Не удалось создать скриншот падения: {ex.Message}");
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
    }
}


