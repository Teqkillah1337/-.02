using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace MedicalLaboratory
{
    public partial class LabAssistantWindow : Window
    {
        private User currentUser;
        private DispatcherTimer sessionTimer;
        private Frame mainFrame; // Добавляем поле для Frame

        public LabAssistantWindow(User user, DispatcherTimer timer, Frame frame)
        {
            InitializeComponent();
            currentUser = user;
            sessionTimer = timer;
            mainFrame = frame; // Сохраняем ссылку на Frame
            // Отображаем информацию о пользователе
            lblUserName.Text = user.Name;
            // Обновляем таймер сессии
            UpdateSessionTimer();
            // Запускаем обновление таймера каждую секунду
            var updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(1);
            updateTimer.Tick += (s, e) => UpdateSessionTimer();
            updateTimer.Start();
        }

        private void UpdateSessionTimer()
        {
            if (sessionTimer.Tag is DateTime startTime)
            {
                var elapsed = DateTime.Now - startTime;
                lblSessionTimer.Text = $"{elapsed.Hours:00}:{elapsed.Minutes:00}";
            }
        }

        private void AcceptBiomaterial_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал приема биоматериала будет реализован в сессии 2", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал формирования отчетов будет реализован позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            sessionTimer.Stop();
            var loginPage = new LoginPage();
            mainFrame.Navigate(loginPage); // Используем переданный Frame
            this.Close();
        }
    }
}