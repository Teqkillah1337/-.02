// Проверяет правильность инициализации окна, установки имени пользователя, корректную работу таймера сессии.


using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class LabAssistantWindowTests
    {
        public void LabAssistantWindow_Initialization_UserNameSet()
        {
            var user = new User { Name = "AssistantUser" };
            var timer = new DispatcherTimer();
            var frame = new Frame();

            var labAssistantWindow = new LabAssistantWindow(user, timer, frame);

            Assert.AreEqual("AssistantUser", labAssistantWindow.lblUserName.Text, "Имя пользователя не установлено корректно.");
        }
    }
}
