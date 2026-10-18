using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V150.IndexedDB;
using OpenQA.Selenium.Firefox;
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
    public class PurchaseTests : BaseTest
    {
        public class ProductTests : BaseTest
        {

            [SetUp]
            public void Preconditions() => LoginAndVerify();

            [Test]
            public void Test_RemoveProduct_ShouldDecreaseCartCount()
            {
                ProductPage.AddProductToCart("Sauce Labs Backpack");
                ProductPage.AddProductToCart("Sauce Labs Fleece Jacket");

                ProductPage.RemoveProductFromCart("Sauce Labs Backpack");

                int actualCartCount = int.Parse(ProductPage.GetCartBadgeText());

                Assert.That(actualCartCount, Is.EqualTo(1),
                    $"После удаления товара счётчик корзины должен быть равен 1, но получили: {actualCartCount}");
            }

            [Test]
            public void Test_SuccessfulPurchaseFlow()
            {
                string[] productAdd = {
                "Sauce Labs Backpack", "Sauce Labs Bike Light" , "Sauce Labs Bolt T-Shirt",
                "Sauce Labs Fleece Jacket", "Sauce Labs Onesie", "Test.allTheThings() T-Shirt (Red)"
                };

                foreach (string product in productAdd)
                {
                    ProductPage.AddProductToCart(product);
                }

                int actualCartCount = int.Parse(ProductPage.GetCartBadgeText());

                Assert.That(actualCartCount, Is.EqualTo(6),
                    $"Количество товаров в корзине не совпадает. Ожидали 6, получили: {actualCartCount}");

                ProductPage.ClickCartIcon();
            }

            [Test]
            public void Test_Checkout_ClickCancel_ShouldReturnToCart()
            {
                ProductPage.AddProductToCart("Sauce Labs Backpack");
                ProductPage.ClickCartIcon();
                CartPage.ClickCheckout();
                CheckoutInformationPage.ClickCancel();
                string actualTitle = CartPage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Your Cart"),
                $"После нажатия на кнопку отмены открылась неверная страница! Ожидали заголовок 'Your Cart', но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_WithValidCustomerData_ShouldProceedToOverview()
            {
                ProductPage.AddProductToCart("Sauce Labs Backpack");
                ProductPage.ClickCartIcon();
                CartPage.ClickCheckout();
                CheckoutInformationPage.FillCheckoutInformation("Ivan", "Ivanov", "123456");
                CheckoutInformationPage.ClickContinue();
                string actualTitle = CheckoutOverviewPage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Checkout: Overview"),
                $"После ввода данных клиента открылась неверная страница! Ожидали заголовок 'Checkout: Overview', но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_ClickFinish_RedirectToCheckoutCompletePage()
            {
                ProductPage.AddProductToCart("Sauce Labs Backpack");
                ProductPage.ClickCartIcon();
                CartPage.ClickCheckout();
                CheckoutInformationPage.FillCheckoutInformation("Ivan", "Ivanov", "123456");
                CheckoutInformationPage.ClickContinue();
                CheckoutOverviewPage.ClickFinish();
                string actualTitle = CheckoutCompletePage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Checkout: Complete!"), $"После ввода данных клиента открылась неверная страница! " +
                    $"Ожидали заголовок 'Checkout: Complete!, но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_ClickBackHome_ShouldReturnToProductPage()
            {
                ProductPage.AddProductToCart("Sauce Labs Backpack");
                ProductPage.ClickCartIcon();
                CartPage.ClickCheckout();
                CheckoutInformationPage.FillCheckoutInformation("Ivan", "Ivanov", "123456");
                CheckoutInformationPage.ClickContinue();
                CheckoutOverviewPage.ClickFinish();
                CheckoutCompletePage.ClickBackHomeButton();
                string actualTitle = ProductPage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Products"), $"После ввода данных клиента открылась неверная страница! " +
                    $"Ожидали заголовок 'Products', но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_Super_Fluent()
            {
                string actualTitle = ProductPage
                    .AddProductToCart("Sauce Labs Backpack")
                    .ClickCartIcon()
                    .ClickCheckout()
                    .FillCheckoutInformation("Ivan", "Ivanov", "123456")
                    .ClickContinue()
                    .ClickFinish()
                    .ClickBackHomeButton()
                    .GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Products"));
            }
        }
    }
}
