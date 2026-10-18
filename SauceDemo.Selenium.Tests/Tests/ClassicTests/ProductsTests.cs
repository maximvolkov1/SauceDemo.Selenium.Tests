using SauceDemo.Selenium.Tests.Drivers;
using SauceDemo.Selenium.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Tests.ClassicTests
{
    [TestFixture]
    public class ProductsTests : BaseTest
    {
        [SetUp]
        public void Preconditions() => LoginAndVerify();

        [Test]
        public void Test_AllPricesAreValid()
        {
            List<double> allPrices = ProductPage.GetAllProductPrices();
            Assert.That(allPrices, Is.All.GreaterThan(0.0),
                "Обнаружены товары с некорректной (нулевой или отрицательной) ценой!");
        }

        [Test]
        public void Test_ProductSortingByPriceLowToHigh()
        {
            ProductPage.SelectSortOptionByText("Price (low to high)");
            List<double> actualPrices = ProductPage.GetAllProductPrices();
            List<double> expectedPrices = actualPrices.OrderBy(p => p).ToList();
            Assert.That(actualPrices, Is.EqualTo(expectedPrices),
                "Сортировка цен на сайте от меньшей к большей работает некорректно!");
        }

        [Test]
        public void Test_ProductSortingByPriceHighToLow()
        {
            ProductPage.SelectSortOptionByText("Price (high to low)");
            List<double> actualPrices = ProductPage.GetAllProductPrices();
            List<double> expectedPrices = actualPrices.OrderByDescending(p => p).ToList();
            Assert.That(actualPrices, Is.EqualTo(expectedPrices),
                "Сортировка цен на сайте от большей к меньшей работает некорректно!");
        }

        [Test]
        public void Test_ProductSortingByNameAToZ()
        {
            ProductPage.SelectSortOptionByText("Name (A to Z)");
            List<string> actualProductNames = ProductPage.GetAllProductNames();
            List<string> expectedProductNames = actualProductNames.OrderBy(p => p).ToList();
            Assert.That(actualProductNames, Is.EqualTo(expectedProductNames),
               "Сортировка товаров по алфавиту на сайте от A до Z работает некорректно!");
        }

        [Test]
        public void Test_ProductSortingByNameZToA()
        {
            ProductPage.SelectSortOptionByText("Name (Z to A)");
            List<string> actualProductNames = ProductPage.GetAllProductNames();
            List<string> expectedProductNames = actualProductNames.OrderByDescending(p => p).ToList();
            Assert.That(actualProductNames, Is.EqualTo(expectedProductNames),
               "Сортировка товаров по алфавиту на сайте от Z до A работает некорректно!");
        }

        [Test]
        public void Test_ClickProductTitle()
        {
            string targetProduct = "Sauce Labs Backpack";
            var productDetailsPage = ProductPage.ClickProductSpecificationByName(targetProduct);
            Assert.That(productDetailsPage.GetProductName(), Is.EqualTo(targetProduct));
        }

        [Test]
        public void Test_ClickProductImage_ShouldOpenCorrectDetailsPage()
        {
            string expectedProductName = "Sauce Labs Bike Light";
            ProductDetailsPage productDetailsPage = ProductPage.ClickProductImageByName(expectedProductName);
            string actualProductName = productDetailsPage.GetProductName();
            Assert.That(actualProductName, Is.EqualTo(expectedProductName),
                $"При клике на картинку открылась неверная карточка товара! Ожидали: '{expectedProductName}', но получили: '{actualProductName}'");
        }

    }
}
