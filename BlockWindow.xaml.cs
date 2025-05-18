using System;
using System.Windows;
using System.Windows.Threading;

namespace MedicalLaboratory
{
    public partial class BlockWindow : Window
    {
        private int remainingSeconds;
        private DispatcherTimer timer;

        public BlockWindow(int blockSeconds)
        {
            InitializeComponent();
            remainingSeconds = blockSeconds;
            UpdateTimerText();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateTimerText();

            if (remainingSeconds <= 0)
            {
                timer.Stop();
                this.Close();
            }
        }

        private void UpdateTimerText()
        {
            lblTimer.Text = $"Осталось: {remainingSeconds} секунд";
        }
    }
}