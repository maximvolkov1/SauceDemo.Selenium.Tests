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
    public class PurchaseTests : BaseTest
    {

        [TestFixture]
        public class ProductTests : BaseTest
        {
            private LoginPage _loginPage;
            private ProductPage _productPage;
            private CartPage _cartPage;
            private CheckoutInformationPage _checkoutInformationPage;
            private CheckoutCompletePage _checkoutCompletePage;
            private CheckoutOverviewPage _checkoutOverviewPage;
            [SetUp]
            public void Preconditions()
            {
                _loginPage = new LoginPage(_driver!);
                _productPage = new ProductPage(_driver!);
                _cartPage = new CartPage(_driver!);
                _checkoutInformationPage = new CheckoutInformationPage(_driver!);
                _checkoutCompletePage = new CheckoutCompletePage(_driver!);
                _checkoutOverviewPage = new CheckoutOverviewPage(_driver!);

                _loginPage.Login("standard_user", "secret_sauce");
                Assert.That(_productPage.GetTitleText(), Is.EqualTo("Products"), "Ошибка предсловия: не попали в магазин!");
            }

            [Test]
            public void Test_RemoveProduct_ShouldDecreaseCartCount()
            {
                _productPage.AddProductToCart("Sauce Labs Backpack");
                _productPage.AddProductToCart("Sauce Labs Fleece Jacket");

                _productPage.RemoveProductFromCart("Sauce Labs Backpack");

                int actualCartCount = int.Parse(_productPage.GetCartBadgeText());

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
                    _productPage.AddProductToCart(product);
                }

                int actualCartCount = int.Parse(_productPage.GetCartBadgeText());

                Assert.That(actualCartCount, Is.EqualTo(6),
                    $"Количество товаров в корзине не совпадает. Ожидали 6, получили: {actualCartCount}");

                _productPage.ClickCartIcon();
            }

            [Test]
            public void Test_Checkout_ClickCancel_ShouldReturnToCart()
            {
                _productPage.AddProductToCart("Sauce Labs Backpack");
                _productPage.ClickCartIcon();
                _cartPage.ClickCheckout();
                _checkoutInformationPage.ClickCancel();
                string actualTitle = _cartPage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Your Cart"),
                $"После нажатия на кнопку отмены открылась неверная страница! Ожидали заголовок 'Your Cart', но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_WithValidCustomerData_ShouldProceedToOverview()
            {
                _productPage.AddProductToCart("Sauce Labs Backpack");
                _productPage.ClickCartIcon();
                _cartPage.ClickCheckout();
                _checkoutInformationPage.FillCheckoutInformation("Ivan", "Ivanov", "123456");
                _checkoutInformationPage.ClickContinue();
                string actualTitle = _checkoutOverviewPage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Checkout: Overview"),
                $"После ввода данных клиента открылась неверная страница! Ожидали заголовок 'Checkout: Overview', но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_ClickFinish_RedirectToCheckoutCompletePage()
            {
                _productPage.AddProductToCart("Sauce Labs Backpack");
                _productPage.ClickCartIcon();
                _cartPage.ClickCheckout();
                _checkoutInformationPage.FillCheckoutInformation("Ivan", "Ivanov", "123456");
                _checkoutInformationPage.ClickContinue();
                _checkoutOverviewPage.ClickFinish();
                string actualTitle = _checkoutCompletePage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Checkout: Complete!"), $"После ввода данных клиента открылась неверная страница! " +
                    $"Ожидали заголовок 'Checkout: Complete!, но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_ClickBackHome_ShouldReturnToProductPage()
            {
                _productPage.AddProductToCart("Sauce Labs Backpack");
                _productPage.ClickCartIcon();
                _cartPage.ClickCheckout();
                _checkoutInformationPage.FillCheckoutInformation("Ivan", "Ivanov", "123456");
                _checkoutInformationPage.ClickContinue();
                _checkoutOverviewPage.ClickFinish();
                _checkoutCompletePage.ClickBackHomeButton();
                string actualTitle = _productPage.GetTitleText();
                Assert.That(actualTitle, Is.EqualTo("Products"), $"После ввода данных клиента открылась неверная страница! " +
                    $"Ожидали заголовок 'Products', но увидели: '{actualTitle}'");
            }
            [Test]
            public void Test_Checkout_Super_Fluent()
            {
                string actualTitle = _productPage
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
