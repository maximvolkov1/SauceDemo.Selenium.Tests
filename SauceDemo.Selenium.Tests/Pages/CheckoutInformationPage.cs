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
        private readonly By _errorLocator = By.CssSelector("[data-test='error']");

        [AllureStep("Считываем заголовок страницы ввода данных доставки")]
        public string GetTitleText() => WaitAndGetText(_titleLocator);

        [AllureStep("Отменяем оформление заказа и возвращаемся на витрину товаров")]
        public ProductPage ClickCancel()
        {
            WaitAndClick(_cancelButtonLocator);
            return new ProductPage(_driver);
        }

        [AllureStep("Вводим данные покупателя: Имя='{0}', Фамилия='{1}', Индекс='{2}'")]
        public CheckoutInformationPage FillCheckoutInformation(string firstName, string lastName, string zipOrPostalCode)
        {
            WaitAndSendKeys(_firstNameLocator, firstName);
            WaitAndSendKeys(_lastNameLocator, lastName);
            WaitAndSendKeys(_zipOrPostalCodeLocator, zipOrPostalCode);
            return this;
        }

        [AllureStep("Продолжаем оформление заказа и переходим к проверке чека")]
        public CheckoutOverviewPage ClickContinue()
        {
             WaitAndClick(_continueButtonLocator);
            return new CheckoutOverviewPage(_driver);
        }

        [AllureStep("Считываем ошибку при заполнении")]
        public string GetErrorText() => WaitAndGetText(_errorLocator);

    }
}
