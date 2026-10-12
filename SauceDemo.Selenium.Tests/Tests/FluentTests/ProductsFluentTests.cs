using Allure.Net.Commons.Attributes;
using SauceDemo.Selenium.Tests.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Selenium.Tests.Tests.FluentTests
{

    [AllureFeature("Витрина товаров")]

    public class ProductsFluentTests : BaseTest
    {
        [AllureStory("Отображение деталей товара")]
        [AllureDescription("Тест проверяет, что при переходе в карточку товара корректно отображаются его название, цена, описание и картинка")]
        [Test]
        public void Test_ProductDetails_ShouldShowCorrectInformation()
        {
            
        }

        [AllureStory("Сортировка каталога")]
        [AllureDescription("Тест проверяет, что при выборе сортировки от низких цен к высоким товары на витрине выстраиваются в правильном порядке")]
        [Test]
        public void Test_ProductSorting_ByPriceLowToHigh()
        {
            
        }
    }
}
