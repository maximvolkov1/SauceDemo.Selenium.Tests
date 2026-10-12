using SauceDemo.Selenium.Tests.Drivers;
using SauceDemo.Selenium.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.NUnit;
using Allure.Net.Commons.Attributes;

namespace SauceDemo.Selenium.Tests.Tests.FluentTests
{
    public class LoginFluentTests : BaseTest
    {

        [AllureFeature("Авторизация")]
        [AllureStory("Успешный вход")] 
        [AllureDescription("Тест проверяет, что стандартный пользователь успешно заходит на сайт Swag Labs")]
        [Test]
        public void Test_Fluent_ValidLogin()
        {
            var actualTitle = new LoginPage(_driver!)
                .LoginAsValidUser(TestUsers.Standard, "secret_sauce")
                .GetTitleText();
            Assert.That(actualTitle, Is.EqualTo("Products"));

        }

        [Test]
        public void Test_Fluent_LockedOutUserLogin()
        {
            string error = new LoginPage(_driver!)
                .Login(TestUsers.LockedOut, "secret_sauce")
                .GetErrorMessageText();
            Assert.That(error, Does.Contain("Sorry, this user has been locked out."), 
                $"Для заблокированного пользователя" + $" '{TestUsers.LockedOut}' " +
                $"на экране отобразился неверный текст ошибки! " +
                $"Реальный текст в браузере: '{error}'");
        }

        [Test]
        [TestCase("user_does_not_exist", "secret_sauce", "Username and password do not match any user in this service")]
        [TestCase("", "", "Username is required")]
        [TestCase("", "secret_sauce", "Username is required")]
        [TestCase("standard_user", "", "Password is required")]
        public void Test_Fluent_AuthorizationErrors(string username, string password, string expectedError)
        {
            string actualError = new LoginPage(_driver!)
                .Login(username, password)
                .GetErrorMessageText();
            Assert.That(actualError, Does.Contain(expectedError),
                $"Сайт выдал некорректный текст ошибки при входе с логином " +
                $"'{username}' и паролем '{password}'. " +
                $"Реальный текст на экране: '{actualError}'");
        }
    }
}
