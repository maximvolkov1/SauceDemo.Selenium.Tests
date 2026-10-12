using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class CartPage : BasePage
    {
        public CartPage(IWebDriver driver) : base(driver) { }

        private readonly By _titleLocator = By.CssSelector("[data-test='title']");
        private readonly By _checkoutButtonLocator = By.CssSelector("[data-test='checkout']");
        private readonly By _continueShoppingButtonLocator = By.CssSelector("[data-test='continue-shopping']");
        private readonly By _cartQuantityLocator = By.CssSelector("[data-test='item-quantity']");
        private readonly By _inventoryItemNameLocator = By.CssSelector("[data-test='inventory-item-name']");
        private readonly By _inventoryItemDescLocator = By.CssSelector("[data-test='inventory-item-desc']");
        private readonly By _inventoryItemPriceLocator = By.CssSelector("[data-test='inventory-item-price']");

        [AllureStep("Считываем заголовок страницы корзины")]
        public string GetTitleText()
        {
            return _driver.FindElement(_titleLocator).Text;
        }

        [AllureStep("Переходим к оформлению заказа (Checkout)")]
        public CheckoutInformationPage ClickCheckout()
        {
            _driver.FindElement(_checkoutButtonLocator).Click();
            return new CheckoutInformationPage(_driver);
        }

        [AllureStep("Возвращаемся из корзины обратно на витрину товаров")]
        public ProductPage ClickContinueShopping()
        {
            _driver.FindElement(_continueShoppingButtonLocator).Click();
            return new ProductPage(_driver);
        }

        [AllureStep("Удаляем товар '{0}' из корзины")]
        public CartPage RemoveProductByName(string productSystemName)
        {
            By dynamicRemoveLocator = By.CssSelector($"[data-test='remove-{productSystemName}']");
            _driver.FindElement(dynamicRemoveLocator).Click();
            return this;
        }

        [AllureStep("Считываем количество товара '{0}' в корзине")]
        public string GetProductQuantityByName(string productName)
        {
            var itemContainer = _driver.FindElement(By.XPath($"//div[@class='cart_item' and .//div[@data-test='inventory-item-name' and text()='{productName}']]"));
            return itemContainer.FindElement(_cartQuantityLocator).Text;
        }

        [AllureStep("Считываем описание товара '{0}' в корзине")]
        public string GetProductDescriptionByName(string productName)
        {
            var itemContainer = _driver.FindElement(By.XPath($"//div[@class='cart_item' and .//div[@data-test='inventory-item-name' and text()='{productName}']]"));
            return itemContainer.FindElement(_inventoryItemDescLocator).Text;
        }

        [AllureStep("Считываем цену товара '{0}' в корзине")]
        public string GetProductPriceByName(string productName)
        {
            var itemContainer = _driver.FindElement(By.XPath($"//div[@class='cart_item' and .//div[@data-test='inventory-item-name' and text()='{productName}']]"));
            return itemContainer.FindElement(_inventoryItemPriceLocator).Text;
        }

        [AllureStep("Кликаем по названию товара '{0}', чтобы перейти в его карточку")]
        public ProductDetailsPage ClickProductName(string productName)
        {
            var itemContainer = _driver.FindElement(By.XPath($"//div[@class='cart_item' and .//div[@data-test='inventory-item-name' and text()='{productName}']]"));
            itemContainer.FindElement(_inventoryItemNameLocator).Click();
            return new ProductDetailsPage(_driver);
        }
    }
}
