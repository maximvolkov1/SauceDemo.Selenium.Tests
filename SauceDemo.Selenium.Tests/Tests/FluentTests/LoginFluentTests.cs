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
    [TestFixture]
    public class LoginFluentTests : BaseTest
    {

        [AllureFeature("Авторизация")]
        [AllureStory("Успешный вход")] 
        [AllureDescription("Тест проверяет, что стандартный пользователь успешно заходит на сайт Swag Labs")]
        [Test]
        public void Test_Fluent_ValidLogin()
        {
            var actualTitle = LoginPage
                .LoginAsValidUser(TestUsers.Standard, "secret_sauce")
                .GetTitleText();
            Assert.That(actualTitle, Is.EqualTo("Products"));
        }

        [AllureFeature("Авторизация")]
        [AllureStory("Пользователь заблокирован")]
        [AllureDescription("Тест проверяет, что заблокированный пользователь не может авторизоваться на сайте Swag Labs")]
        [Test]
        public void Test_Fluent_LockedOutUserLogin()
        {
            string error = LoginPage
                .Login(TestUsers.LockedOut, "secret_sauce")
                .GetErrorMessageText();
            Assert.That(error, Does.Contain("Sorry, this user has been locked out."), 
                $"Для заблокированного пользователя" + $" '{TestUsers.LockedOut}' " +
                $"на экране отобразился неверный текст ошибки! " +
                $"Реальный текст в браузере: '{error}'");
        }

        [AllureFeature("Авторизация")]
        [AllureStory("Негативные сценарии (ошибки входа)")]
        [AllureDescription("Проверка валидации полей логина и пароля при некорректных данных")]
        [Test]
        [TestCase("user_does_not_exist", "secret_sauce", "Username and password do not match any user in this service")]
        [TestCase("", "", "Username is required")]
        [TestCase("", "secret_sauce", "Username is required")]
        [TestCase("standard_user", "", "Password is required")]
        public void Test_Fluent_AuthorizationErrors(string username, string password, string expectedError)
        {
            string actualError = LoginPage
                .Login(username, password)
                .GetErrorMessageText();
            Assert.That(actualError, Does.Contain(expectedError),
                $"Сайт выдал некорректный текст ошибки при входе с логином " +
                $"'{username}' и паролем '{password}'. " +
                $"Реальный текст на экране: '{actualError}'");
        }

        [AllureFeature("Выход из личного кабинета")]
        [AllureStory("Успешный выход")]
        [AllureDescription("Тест проверяет, что пользователь успешно выходит из учётной записи")]
        [Test]
        public void Test_User_Can_LogoutFluent_Successfully()
        {
            LoginPage
                .LoginAsValidUser(TestUsers.Standard, "secret_sauce")
                .ClickLogout();                                       
            Assert.That(_driver!.Url, Is.EqualTo("https://www.saucedemo.com/"),
                "После логаута пользователь не вернулся на главную страницу авторизации!");
        }
    }
}
