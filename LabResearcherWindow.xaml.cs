using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace MedicalLaboratory
{
    public partial class LabResearcherWindow : Window
    {
        private User currentUser;
        private DispatcherTimer sessionTimer;

        public LabResearcherWindow(User user, DispatcherTimer timer)
        {
            InitializeComponent();
            currentUser = user;
            sessionTimer = timer;

            lblUserName.Text = user.Name;
            UpdateSessionTimer();

            var updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(1);
            updateTimer.Tick += (s, e) => UpdateSessionTimer();
            updateTimer.Start();

            LoadAnalyzers();
        }

        private void LoadAnalyzers()
        {
            cmbAnalyzers.Items.Add("Ledetect");
            cmbAnalyzers.Items.Add("Biorad");
            cmbAnalyzers.SelectedIndex = 0;
        }

        private void Analyzer_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (var db = new MedicalLaboratory20Entities())
            {
                var analyzerName = cmbAnalyzers.SelectedItem.ToString();
                var services = db.Services
                    .Where(s => s.AvailableAnalyzers != null && s.AvailableAnalyzers.Contains(analyzerName))
                    .ToList();

                dgServices.ItemsSource = services;
            }
        }

        private void SendToAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал отправки на анализатор будет реализован в сессии 3", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApproveResult_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал подтверждения результатов будет реализован в сессии 3", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateSessionTimer()
        {
            if (sessionTimer.Tag is DateTime startTime)
            {
                var elapsed = DateTime.Now - startTime;
                lblSessionTimer.Text = $"{elapsed.Hours:00}:{elapsed.Minutes:00}";
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            sessionTimer.Stop();
            new MainWindow().Show();
            this.Close();
        }
    }
}