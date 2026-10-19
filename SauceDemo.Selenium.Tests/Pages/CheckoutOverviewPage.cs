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
        private readonly By _subtotalLabelLocator = By.CssSelector("[data-test='subtotal-label']");
        private readonly By _taxLabelLocator = By.CssSelector("[data-test='tax-label']");
        private readonly By _totalLabelLocator = By.CssSelector("[data-test='total-label']");

        [AllureStep("Считываем заголовок страницы проверки заказа")]
        public string GetTitleText() => WaitAndGetText(_titleLocator);

        [AllureStep("Отменяем заказ и возвращаемся на главную витрину товаров")]
        public ProductPage ClickCancel()
        {
            WaitAndClick(_cancelButtonLocator);
            return new ProductPage(_driver);
        }

        [AllureStep("Подтверждаем заказ и завершаем оформление")]
        public CheckoutCompletePage ClickFinish()
        {
            WaitAndClick(_finishButtonLocator);
            return new CheckoutCompletePage(_driver);
        }

        [AllureStep("Считываем чистую стоимость товаров (Item total)")]
        public string GetItemTotalText() => WaitAndGetText(_subtotalLabelLocator);

        [AllureStep("Считываем сумму налога (Tax)")]
        public string GetTaxText() => WaitAndGetText(_taxLabelLocator);

        [AllureStep("Считываем итоговую сумму к оплате (Total)")]
        public string GetTotalText() => WaitAndGetText(_totalLabelLocator);

    }
}
