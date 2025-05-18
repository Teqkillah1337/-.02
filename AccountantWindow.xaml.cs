using System;
using System.Linq;
using System.Windows;

namespace MedicalLaboratory
{
    public partial class AccountantWindow : Window
    {
        private User currentUser;

        public AccountantWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
            lblUserName.Text = user.Name;

            LoadInsuranceCompanies();

            dpStartDate.SelectedDate = DateTime.Today.AddMonths(-1);
            dpEndDate.SelectedDate = DateTime.Today;
            dpBillStartDate.SelectedDate = DateTime.Today.AddMonths(-1);
            dpBillEndDate.SelectedDate = DateTime.Today;
        }

        private void LoadInsuranceCompanies()
        {
            using (var db = new MedicalLaboratory20Entities())
            {
                cmbInsuranceCompanies.ItemsSource = db.InsuranceCompanies.ToList();
                cmbInsuranceCompanies.DisplayMemberPath = "Name";
                if (cmbInsuranceCompanies.Items.Count > 0)
                    cmbInsuranceCompanies.SelectedIndex = 0;
            }
        }

        private void ViewReports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал просмотра отчетов будет реализован позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateBill_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал формирования счетов будет реализован позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}