using Allure.Net.Commons;
using Allure.NUnit;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
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

        [SetUp]
        public void Setup()
        {
            var options = new FirefoxOptions();
            options.SetPreference("browser.shell.checkDefaultBrowser", false);

            options.AddAdditionalOption("selenoid:options", new Dictionary<string, object>
        {
            { "enableVNC", true },
            { "enableVideo", true }
        });

            _driver = new OpenQA.Selenium.Remote.RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl("https://saucedemo.com");
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


