using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class BasePage
    {
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;

        public BasePage(IWebDriver driver) 
        { 
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        private readonly By _cartIconLocator = By.CssSelector("[data-test='shopping-cart-link']");
        private readonly By _cartBadgeLocator = By.CssSelector("[data-test='shopping-cart-badge']");
        private readonly By _menuButtonLocator = By.Id("react-burger-menu-btn");
        private readonly By _logoutLinkLocator = By.Id("logout_sidebar_link");

        [AllureStep("Нажимаем на иконку корзины в шапке сайта и переходим в неё")]
        public CartPage ClickCartIcon()
        {
            WaitAndClick(_cartIconLocator);
            return new CartPage(_driver);
        }

        [AllureStep("Считываем количество товаров на счетчике корзины")]
        public string GetCartBadgeText() => _driver.FindElement(_cartBadgeLocator).Text;

        [AllureStep("Открываем боковое меню и нажимаем кнопку выхода 'Logout'")]
        public LoginPage ClickLogout()
        {
            WaitAndClick(_menuButtonLocator);   
            WaitAndClick(_logoutLinkLocator);   
            return new LoginPage(_driver);
        }

        [AllureStep("Ожидаем элемент и кликаем")]
        protected void WaitAndClick(By locator)
        {
            IWebElement element = _wait.Until(d => d.FindElement(locator));
            element.Click();
        }

        [AllureStep("Ожидаем элемент и заполняем")]
        protected void WaitAndSendKeys(By locator, string text)
        {
            IWebElement element = _wait.Until(d => d.FindElement(locator));
            element.Clear();
            element.SendKeys(text);
        }

        [AllureStep("Ожидаем элемент и считываем его текст")]
        protected string WaitAndGetText(By locator)
        {
            return _wait.Until(d => d.FindElement(locator).Text);
        }

        [AllureStep("Ожидаем элемент и получаем элемент")]
        protected IWebElement WaitAndGetElement(By locator)
        {
            return _wait.Until(d => d.FindElement(locator));
        }

        [AllureStep("Ожидаем элементы и получаем элементы")]
        protected ReadOnlyCollection<IWebElement> WaitAndGetElements(By locator)
        {
            return _wait.Until(d => d.FindElements(locator));
        }

    }
}
