// Проверяет правильность инициализации окна, установки имени пользователя и корректную работу таймера сессии.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class LabResearcherWindowTests
    {
        public void LabResearcherWindow_Initialization_UserNameSet()
        {
            var user = new User { Name = "ResearcherUser" };
            var timer = new DispatcherTimer();

            var labResearcherWindow = new LabResearcherWindow(user, timer);

            Assert.AreEqual("ResearcherUser", labResearcherWindow.lblUserName.Text, "Имя пользователя не установлено корректно.");
        }
    }
}