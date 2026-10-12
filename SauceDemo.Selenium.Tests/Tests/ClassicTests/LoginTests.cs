using SauceDemo.Selenium.Tests.Drivers;
using SauceDemo.Selenium.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Tests.ClassicTests
{
    public class LoginTests : BaseTest
    {
        [Test]
        public void Test_ValidLogin()
        {
            var productPage = new LoginPage(_driver!)
                .LoginAsValidUser(TestUsers.Standard, "secret_sauce");

            Assert.That(productPage.GetTitleText(), Is.EqualTo("Products"));
        }

        [Test]
        public void Test_LockedOutUserLogin()
        {
            var loginPage = new LoginPage(_driver!);

            loginPage.Login(TestUsers.LockedOut, "secret_sauce");

            string error = loginPage.GetErrorMessageText();
            Assert.That(error, Does.Contain("Sorry, this user has been locked out."));
        }

        [Test]
        [TestCase("user_does_not_exist", "secret_sauce", "Username and password do not match any user in this service")]
        [TestCase("", "", "Username is required")]
        [TestCase("", "secret_sauce", "Username is required")]
        [TestCase("standard_user", "", "Password is required")]
        public void Test_AuthorizationErrors(string username, string password, string expectedError)
        {
            var loginPage = new LoginPage(_driver!);

            loginPage.Login(username, password);
            string actualError = loginPage.GetErrorMessageText();

            Assert.That(actualError, Does.Contain(expectedError),
                $"Сайт выдал некорректный текст ошибки при входе с логином '{username}' и паролем '{password}'. Реальный текст на экране: '{actualError}'");
        }
    }
}
