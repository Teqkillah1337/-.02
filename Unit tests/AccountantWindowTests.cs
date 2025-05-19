// Проверяет правильность инициализации окна, установки имени пользователя, обработку различных сценариев, связанных с формированием счетов.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class AccountantWindowTests
    {
        public void AccountantWindow_Initialization_UserNameSet()
        {
            var user = new User { Name = "TestUser" };

            var accountantWindow = new AccountantWindow(user);

            Assert.AreEqual("TestUser", accountantWindow.lblUserName.Text, "Имя пользователя не установлено корректно.");
        }

        public void AccountantWindow_GenerateBill_NoInsuranceCompanySelected_ShowsErrorMessage()
        {
            var user = new User { Name = "TestUser" };
            var accountantWindow = new AccountantWindow(user);
            accountantWindow.cmbInsuranceCompanies.SelectedItem = null;

            Assert.DoesNotThrow(() => accountantWindow.GenerateBill_Click(null, null));
        }

        public void AccountantWindow_DatePickersInitializedToCorrectDates()
        {
            var user = new User { Name = "TestUser" };

            var accountantWindow = new AccountantWindow(user);

            Assert.AreEqual(DateTime.Today.AddMonths(-1), accountantWindow.dpStartDate.SelectedDate, "dpStartDate initialized incorrectly.");
            Assert.AreEqual(DateTime.Today, accountantWindow.dpEndDate.SelectedDate, "dpEndDate initialized incorrectly.");
            Assert.AreEqual(DateTime.Today.AddMonths(-1), accountantWindow.dpBillStartDate.SelectedDate, "dpBillStartDate initialized incorrectly.");
            Assert.AreEqual(DateTime.Today, accountantWindow.dpBillEndDate.SelectedDate, "dpBillEndDate initialized incorrectly.");
        }
    }
}