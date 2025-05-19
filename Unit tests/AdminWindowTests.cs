// Проверяет правильность инициализации окна и установки имени пользователя.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class AdminWindowTests
    {
        public void AdminWindow_Initialization_UserNameSet()
        {
            var user = new User { Name = "AdminUser" };

            var adminWindow = new AdminWindow(user);

            Assert.AreEqual("AdminUser", adminWindow.lblUserName.Text, "Имя пользователя не установлено корректно.");
        }
    }
}
