using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class BasePage
    {
        protected readonly IWebDriver _driver;
        public BasePage(IWebDriver driver) => _driver = driver;
        private readonly By _cartIconLocator = By.CssSelector("[data-test='shopping-cart-link']");
        private readonly By _cartBadgeLocator = By.CssSelector("[data-test='shopping-cart-badge']");
        private readonly By _menuButtonLocator = By.Id("react-burger-menu-btn");
        private readonly By _logoutLinkLocator = By.Id("logout_sidebar_link");

        [AllureStep("Нажимаем на иконку корзины в шапке сайта и переходим в неё")]
        public CartPage ClickCartIcon()
        {
            _driver.FindElement(_cartIconLocator).Click();
            return new CartPage(_driver);
        }

        [AllureStep("Считываем количество товаров на счетчике корзины")]
        public string GetCartBadgeText() => _driver.FindElement(_cartBadgeLocator).Text;

        [AllureStep("Открываем боковое меню и нажимаем кнопку выхода 'Logout'")]
        public LoginPage ClickLogout()
        {
            _driver.FindElement(_menuButtonLocator).Click();
            _driver.FindElement(_logoutLinkLocator).Click();
            return new LoginPage(_driver);
        }
    }
}
