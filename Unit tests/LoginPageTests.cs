// Проверяет генерацию капчи при инициализации страницы.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class LoginPageTests
    {
        public void LoginPage_Initialization_CaptchaNotNull()
        {
            var loginPage = new LoginPage();

            Assert.IsNotEmpty(loginPage.currentCaptcha, "Капча не была сгенерирована."); 
        }
    }
}
