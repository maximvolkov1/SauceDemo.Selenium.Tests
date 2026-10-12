using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class CheckoutOverviewPage : BasePage
    {
        public CheckoutOverviewPage(IWebDriver driver) : base(driver) { }
        private readonly By _titleLocator = By.CssSelector("[data-test='title']");
        private readonly By _cancelButtonLocator = By.CssSelector("[data-test='cancel']");
        private readonly By _finishButtonLocator = By.CssSelector("[data-test='finish']");

        [AllureStep("Считываем заголовок страницы проверки заказа")]
        public string GetTitleText()
        {
            return _driver.FindElement(_titleLocator).Text;
        }

        [AllureStep("Отменяем заказ и возвращаемся на главную витрину товаров")]
        public ProductPage ClickCancel()
        {
            _driver.FindElement(_cancelButtonLocator).Click();
            return new ProductPage(_driver);
        }

        [AllureStep("Подтверждаем заказ и завершаем оформление")]
        public CheckoutCompletePage ClickFinish()
        {
            _driver.FindElement(_finishButtonLocator).Click();
            return new CheckoutCompletePage(_driver);
        }
    }
}
