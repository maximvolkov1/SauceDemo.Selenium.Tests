using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class CheckoutCompletePage : BasePage
    {
        public CheckoutCompletePage(IWebDriver driver) : base(driver) { }

        private readonly By _titleLocator = By.CssSelector("[data-test='title']");
        private readonly By _textThanksForOrderLocator = By.CssSelector("[data-test='complete-header']");
        private readonly By _backHomeButtonLocator = By.CssSelector("[data-test='back-to-products']");
        private readonly By _generatePdfOrderButtonLocator = By.CssSelector("[data-test='generate-pdf-order']");

        [AllureStep("Считываем заголовок финальной страницы заказа")]
        public string GetTitleText() => WaitAndGetText(_titleLocator);

        [AllureStep("Считываем текст подтверждения заказа ('Thank you for your order!')")]
        public string GetTextThanksForOrder() => WaitAndGetText(_textThanksForOrderLocator);

        [AllureStep("Нажимаем кнопку 'Back Home' и возвращаемся на главную витрину товаров")]
        public ProductPage ClickBackHomeButton()
        {
            WaitAndClick(_backHomeButtonLocator);
            return new ProductPage(_driver);
        }

        [AllureStep("Запрашиваем генерацию PDF-документа с подтверждением заказа")]
        public CheckoutCompletePage ClickGeneratePdfOrderButton()
        {
            WaitAndClick(_generatePdfOrderButtonLocator);
            return this;
        }

    }
}
