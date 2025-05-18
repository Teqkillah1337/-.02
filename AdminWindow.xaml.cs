using System;
using System.Linq;
using System.Windows;
using System.Data.Entity;

namespace MedicalLaboratory
{
    public partial class AdminWindow : Window
    {
        private User currentUser;

        public AdminWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
            lblUserName.Text = user.Name;

            LoadLoginHistory();
            LoadSupplies();
        }

        private void LoadLoginHistory()
        {
            using (var db = new MedicalLaboratory20Entities())
            {
                dgLoginHistory.ItemsSource = db.LoginHistories
                    .Include(lh => lh.SystemUsers) // Используем SystemUsers вместо User
                    .OrderByDescending(lh => lh.AttemptTime) // Используем AttemptTime вместо LoginTime
                    .ToList();
            }
        }

        private void LoadSupplies()
        {
            using (var db = new MedicalLaboratory20Entities())
            {
                // Используем PerformedServices вместо Supplies
                dgSupplies.ItemsSource = db.PerformedServices.ToList();
            }
        }

        private void SearchLoginHistory_Click(object sender, RoutedEventArgs e)
        {
            var searchText = txtSearchLogin.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadLoginHistory();
                return;
            }

            using (var db = new MedicalLaboratory20Entities())
            {
                dgLoginHistory.ItemsSource = db.LoginHistories
                    .Include(lh => lh.SystemUsers)
                    .Where(lh => lh.Login.Contains(searchText)) // Используем поле Login из LoginHistory
                    .OrderByDescending(lh => lh.AttemptTime)
                    .ToList();
            }
        }

        private void ResetLoginHistory_Click(object sender, RoutedEventArgs e)
        {
            txtSearchLogin.Text = "";
            LoadLoginHistory();
        }

        private void GenerateUsersReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал формирования отчетов будет реализован позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateServicesReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал формирования отчетов будет реализован позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}