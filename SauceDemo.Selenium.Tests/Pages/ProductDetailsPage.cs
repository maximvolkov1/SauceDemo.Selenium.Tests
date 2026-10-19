using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class ProductDetailsPage : BasePage
    {
        public ProductDetailsPage(IWebDriver driver) : base(driver) { }

        private readonly By _backToProductsButtonLocator = By.CssSelector("[data-test='back-to-products']");
        private readonly By _productNameLocator = By.CssSelector("[data-test='inventory-item-name']");
        private readonly By _productPriceLocator = By.CssSelector("[data-test='inventory-item-price']");
        private readonly By _productDescriptionLocator = By.CssSelector("[data-test='inventory-item-desc']");
        private readonly By _productImageLocator = By.CssSelector(".inventory_details_img");
        private readonly By _cartButtonLocator = By.CssSelector(".btn_inventory");

        [AllureStep("Считываем название товара в его карточке")]
        public string GetProductName() => WaitAndGetText(_productNameLocator);

        [AllureStep("Считываем цену товара в его карточке")]
        public string GetProductPrice() => WaitAndGetText(_productPriceLocator);

        [AllureStep("Считываем описание товара в его карточке")]
        public string GetProductDescription() => WaitAndGetText(_productDescriptionLocator);

        [AllureStep("Нажимаем кнопку 'Back to Products' для возврата на витрину")]
        public ProductPage ClickBackToProducts()
        {
            WaitAndClick(_backToProductsButtonLocator);
            return new ProductPage(_driver);
        }

        [AllureStep("Проверяем, отображается ли изображение товара в его карточке")]
        public bool IsImageDisplayed() => WaitAndGetElement(_productImageLocator).Displayed;

        [AllureStep("Считываем ссылку на изображение товара в его карточке")]
        public string? GetProductImageUrl() => WaitAndGetElement(_productImageLocator).GetAttribute("src");

        [AllureStep("Считываем текущий текст кнопки корзины ('Add to cart' / 'Remove')")]
        public string GetCartButtonText() => WaitAndGetText(_cartButtonLocator);

        [AllureStep("Нажимаем на кнопку добавления/удаления товара в его карточке")]
        public ProductDetailsPage ClickCartButton()
        {
            WaitAndClick(_cartButtonLocator);
            return this;
        }
    }

}
