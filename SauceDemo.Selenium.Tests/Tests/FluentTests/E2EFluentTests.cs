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
    public class E2EFluentTests : BaseTest
    {
        [Test]
        [AllureFeature("Оформление заказа")]
        [AllureStory("Сквозной сценарий покупки")]
        [AllureDescription("Тест проверяет полный цикл: логин, " +
            "добавление товара, переход в корзину, ввод данных на чекауте, " +
            "проверку суммирования цен и финальный экран успеха.")]
        public void Test_Fluent_SuccessfulFullPurchase()
        {
            var informationPage = new LoginPage(_driver!)
                .LoginAsValidUser(TestUsers.Standard, "secret_sauce")
                .AddProductToCart("Sauce Labs Fleece Jacket")
                .ClickCartIcon()
                .ClickCheckout();

            var overviewPage = informationPage.FillCheckoutInformation("Ivanov", "Ivan", "123456").ClickContinue();
            Assert.Multiple(() =>
            {
                Assert.That(overviewPage.GetItemTotalText(), Does.Contain("$49.99"));
                Assert.That(overviewPage.GetTaxText(), Does.Contain("$4.00"));
                Assert.That(overviewPage.GetTotalText(), Does.Contain("$53.99"));
            });
            var completePage = overviewPage.ClickFinish();
        }
    }
}
