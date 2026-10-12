using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        public LoginPage(IWebDriver driver) => _driver = driver;

        private readonly By _usernameLocator = By.CssSelector("[data-test='username']");
        private readonly By _passwordLocator = By.CssSelector("[data-test='password']");
        private readonly By _loginButtonLocator = By.CssSelector("[data-test='login-button']");
        private readonly By _errorMessageLocator = By.CssSelector("[data-test='error']");

        public LoginPage Login(string username, string password)
        {
            var usernameField = _driver.FindElement(_usernameLocator);
            usernameField.Clear();
            usernameField.SendKeys(username);

            var passwordField = _driver.FindElement(_passwordLocator);
            passwordField.Clear();
            passwordField.SendKeys(password);

            _driver.FindElement(_loginButtonLocator).Click();
            return this;
        }

        [AllureStep("Успешный вход в систему под пользователем '{0}'")]
        public ProductPage LoginAsValidUser(string username, string password)
        {
            Login(username, password);
            return new ProductPage(_driver);
        }

        [AllureStep("Попытка входа с невалидными данными под пользователем '{0}'")]
        public LoginPage LoginAsInvalidUser(string username, string password)
        {
            Login(username, password);
            return this;
        }

        [AllureStep("Считывание текста появившейся ошибки на странице логина")]
        public string GetErrorMessageText()
        {
            return _driver.FindElement(_errorMessageLocator).Text;
        }
    }
}
