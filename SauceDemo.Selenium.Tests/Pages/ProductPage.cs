using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Allure.Net.Commons.Attributes;

namespace SauceDemo.Selenium.Tests.Pages
{
    public class ProductPage : BasePage
    {
        public ProductPage (IWebDriver driver) : base(driver) { }
            

        private readonly By _titleLocator = By.CssSelector("[data-test='title']");
        private readonly By _productNamesLocator = By.CssSelector("[data-test='inventory-item-name']");
        private readonly By _productPricesLocator = By.CssSelector("[data-test='inventory-item-price']");
        private readonly By _sortContainerLocator = By.CssSelector("[data-test='product-sort-container']");
        private readonly By _productDescriptionsLocator = By.CssSelector("[data-test='inventory-item-desc']");
        private readonly By _productImagesLocator = By.CssSelector(".inventory_item_img img");

        [AllureStep("Считываем заголовок страницы витрины")]
        public string GetTitleText()
        {
            return _driver.FindElement(_titleLocator).Text;
        }

        [AllureStep("Добавляем товар '{0}' в корзину")]
        public ProductPage AddProductToCart(string productName)
        {
            var locator = By.XPath($"//div[@class='inventory_item'][.//div[contains(@class, 'inventory_item_name') and text()='{productName}']]//button");
            _driver.FindElement(locator).Click();
            return this;
        }

        [AllureStep("Удаляем товар '{0}' в корзину")]
        public ProductPage RemoveProductFromCart(string productName)
        {
            var locator = By.XPath($"//div[@class='inventory_item'][.//div[contains(@class, 'inventory_item_name') and text()='{productName}']]//button");
            _driver.FindElement(locator).Click();
            return this;
        }

        [AllureStep("Считываем названия всех товаров с витрины")]
        public List<string> GetAllProductNames()
        {
            return _driver.FindElements(_productNamesLocator)
                .Select(element => element.Text.Trim())
                .ToList();
        }

        [AllureStep("Считываем цены всех товаров с витрины")]
        public List<double> GetAllProductPrices()
        {
            return _driver.FindElements(_productPricesLocator)
                .Select(element => element.Text.Replace("$", "").Trim())
                .Select(text => double.Parse(text, System.Globalization.CultureInfo.InvariantCulture))
                .ToList();
        }

        [AllureStep("Выбираем сортировку '{0}'")]
        public ProductPage SelectSortOptionByText(string optionText)
        {
            new SelectElement(_driver.FindElement(_sortContainerLocator)).SelectByText(optionText);
            return this;
        }

        [AllureStep("Переходим в карточку товара по клику на название '{0}'")]
        public ProductDetailsPage ClickProductSpecificationByName(string productName)
        {
            var locator = By.XPath($"//div[contains(@class, 'inventory_item_name') and text()='{productName}']");
            _driver.FindElement(locator).Click();
            return new ProductDetailsPage(_driver);
        }

        [AllureStep("Переходим в карточку товара по клику на изображение '{0}'")]
        public ProductDetailsPage ClickProductImageByName(string productName)
        {
            var locator = By.XPath($"//div[@class='inventory_item'][.//div[contains(@class, 'inventory_item_name') and text()='{productName}']]//div[@class='inventory_item_img']//a");
            _driver.FindElement(locator).Click();
            return new ProductDetailsPage(_driver);
        }

        [AllureStep("Считываем описания всех товаров с витрины")]
        public List<string> GetAllProductDescriptions()
        {
            return _driver.FindElements(_productDescriptionsLocator)
                .Select(element => element.Text.Trim())
                .ToList();
        }

        [AllureStep("Считываем ссылки на изображения всех товаров с витрины")]
        public List<string?> GetAllProductImageUrls()
        {
            return _driver.FindElements(_productImagesLocator)
                .Select(element => element.GetAttribute("src"))
                .ToList();
        }
    }
}
