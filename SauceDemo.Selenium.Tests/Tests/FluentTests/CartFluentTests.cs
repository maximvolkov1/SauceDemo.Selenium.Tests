using Allure.Net.Commons.Attributes;
using SauceDemo.Selenium.Tests.Drivers;
using SauceDemo.Selenium.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Tests.FluentTests
{
    public class CartFluentTests : BaseTest
    {
        [AllureFeature("Корзина")]
        [AllureStory("Содержимое корзины")]
        [AllureDescription("Тест добавляет товар в корзину и проверяет корректность его данных: название, цену, описание и количество. А также проверяет возможность перехода в карточку товара из корзины.")]
        [Test]
        public void Test_Fluent_ProductDetailsInCart()
        {
            var cartPage = new LoginPage(_driver!)
                .LoginAsValidUser(TestUsers.Standard, "secret_sauce")
                .AddProductToCart("Sauce Labs Fleece Jacket")
                .ClickCartIcon();
            Assert.Multiple(() =>
            {
                Assert.That(cartPage.GetProductQuantityByName("Sauce Labs Fleece Jacket"), Is.EqualTo("1"),
                    "Количество товара в корзине не соответствует ожидаемому!");

                Assert.That(cartPage.GetProductPriceByName("Sauce Labs Fleece Jacket"), Is.EqualTo("$49.99"),
                    "Цена товара в корзине отображается неверно!");

                Assert.That(cartPage.GetProductDescriptionByName("Sauce Labs Fleece Jacket"),
                    Does.Contain("It's not every day that you come across a midweight quarter-zip fleece jacket"),
                    "Описание товара в корзине не совпадает с оригинальным!");
            });
            var productTitle = cartPage
                .ClickProductName("Sauce Labs Fleece Jacket")
                .GetProductName();
            Assert.That(productTitle, Is.EqualTo("Sauce Labs Fleece Jacket"),
                "После клика на название товара в корзине открылась неверная страница карточки товара!");
        }
    }
}
