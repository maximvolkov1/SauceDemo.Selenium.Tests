using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class CheckoutInformationPage : BasePage
    {
        public CheckoutInformationPage(IWebDriver driver) : base(driver) { }

        private readonly By _titleLocator = By.CssSelector("[data-test='title']");
        private readonly By _firstNameLocator = By.CssSelector("[data-test='firstName']");
        private readonly By _lastNameLocator = By.CssSelector("[data-test='lastName']");
        private readonly By _zipOrPostalCodeLocator = By.CssSelector("[data-test='postalCode']");
        private readonly By _cancelButtonLocator = By.CssSelector("[data-test='cancel']");
        private readonly By _continueButtonLocator = By.CssSelector("[data-test='continue']");

        [AllureStep("Считываем заголовок страницы ввода данных доставки")]
        public string GetTitleText()
        {
            return _driver.FindElement(_titleLocator).Text;
        }

        [AllureStep("Отменяем оформление заказа и возвращаемся на витрину товаров")]
        public ProductPage ClickCancel()
        {
            _driver.FindElement(_cancelButtonLocator).Click();
            return new ProductPage(_driver);
        }

        [AllureStep("Вводим данные покупателя: Имя='{0}', Фамилия='{1}', Индекс='{2}'")]
        public CheckoutInformationPage FillCheckoutInformation(string firstName, string lastName, string zipOrPostalCode)
        {
            _driver.FindElement(_firstNameLocator).SendKeys(firstName);
            _driver.FindElement(_lastNameLocator).SendKeys(lastName);
            _driver.FindElement(_zipOrPostalCodeLocator).SendKeys(zipOrPostalCode);
            return this;
        }

        [AllureStep("Продолжаем оформление заказа и переходим к проверке чека")]
        public CheckoutOverviewPage ClickContinue()
        {
             _driver.FindElement(_continueButtonLocator).Click();
            return new CheckoutOverviewPage(_driver);
        }
    }
}
