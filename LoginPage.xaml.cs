using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MedicalLaboratory
{
    public partial class LoginPage : Page
    {
        private string currentCaptcha;
        private int failedAttempts = 0;
        private DispatcherTimer blockTimer;
        private DateTime? sessionStartTime;
        private DispatcherTimer sessionTimer;

        public LoginPage()
        {
            InitializeComponent();
            GenerateCaptcha();

            blockTimer = new DispatcherTimer();
            blockTimer.Interval = TimeSpan.FromSeconds(10);
            blockTimer.Tick += BlockTimer_Tick;

            sessionTimer = new DispatcherTimer();
            sessionTimer.Interval = TimeSpan.FromMinutes(1);
            sessionTimer.Tick += SessionTimer_Tick;
        }

        private void GenerateCaptcha()
        {
            captchaCanvas.Children.Clear();

            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            currentCaptcha = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            for (int i = 0; i < currentCaptcha.Length; i++)
            {
                var text = new TextBlock
                {
                    Text = currentCaptcha[i].ToString(),
                    FontFamily = new FontFamily("Comic Sans MS"),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black,
                    RenderTransform = new RotateTransform(random.Next(-15, 15))
                };

                Canvas.SetLeft(text, 10 + i * 30 + random.Next(-5, 5));
                Canvas.SetTop(text, 10 + random.Next(-5, 5));
                captchaCanvas.Children.Add(text);

                if (random.Next(2) == 0)
                {
                    var line = new Line
                    {
                        X1 = random.Next(0, 150),
                        Y1 = random.Next(0, 40),
                        X2 = random.Next(0, 150),
                        Y2 = random.Next(0, 40),
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                    captchaCanvas.Children.Add(line);
                }
            }
        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            var password = txtPassword.Password;
            var textBox = new TextBox
            {
                Text = password,
                FontFamily = new FontFamily("Comic Sans MS"),
                Margin = new Thickness(0)
            };

            var grid = txtPassword.Parent as Grid;
            grid.Children.Remove(txtPassword);
            Grid.SetColumn(textBox, 0);
            grid.Children.Add(textBox);
            txtPassword.Tag = textBox;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            var textBox = txtPassword.Tag as TextBox;
            if (textBox != null)
            {
                var password = textBox.Text;
                var grid = textBox.Parent as Grid;
                grid.Children.Remove(textBox);

                txtPassword.Password = password;
                Grid.SetColumn(txtPassword, 0);
                grid.Children.Add(txtPassword);
            }
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
            txtCaptcha.Text = "";
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            if (blockTimer.IsEnabled)
            {
                var remainingTime = blockTimer.Interval - (DateTime.Now - (DateTime)blockTimer.Tag);
                lblMessage.Text = $"Система заблокирована. Попробуйте через {remainingTime.Seconds} секунд.";
                lblMessage.Visibility = Visibility.Visible;
                return;
            }

            if (failedAttempts > 0 && txtCaptcha.Text.ToUpper() != currentCaptcha)
            {
                lblMessage.Text = "Неверная CAPTCHA!";
                lblMessage.Visibility = Visibility.Visible;
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = chkShowPassword.IsChecked == true
                ? (txtPassword.Tag as TextBox)?.Text
                : txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Введите логин и пароль!";
                lblMessage.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                using (var db = new MedicalLaboratory20Entities())
                {
                    var user = db.Users.FirstOrDefault(u => u.Login == username && u.Password == password);

                    if (user != null)
                    {
                        // Запись успешного входа
                        var loginHistory = new LoginHistory
                        {
                            UserId = user.UserId, // Используем UserId вместо Id
                            Login = user.Login,
                            AttemptTime = DateTime.Now,
                            IsSuccess = true,
                            IPAddress = GetIPAddress()
                        };
                        db.LoginHistories.Add(loginHistory);
                        await db.SaveChangesAsync(); // Для асинхронного сохранения

                        failedAttempts = 0;
                        captchaPanel.Visibility = Visibility.Collapsed;
                        lblMessage.Visibility = Visibility.Hidden;

                        // Обновляем LastEnterTime (если есть такое поле)
                        if (db.Entry(user).Property("LastEnterTime") != null)
                        {
                            db.Entry(user).Property("LastEnterTime").CurrentValue = DateTime.Now;
                        }
                        await db.SaveChangesAsync();

                        // Определяем тип пользователя через RoleId (предполагая, что есть такое поле)
                        int userRole = user.RoleId ?? 0; // Или другое поле, определяющее роль

                        if (userRole == 1 || userRole == 2) // Админ или исследователь
                        {
                            sessionStartTime = DateTime.Now;
                            sessionTimer.Start();
                        }

                        ShowMainWindow(user, userRole);
                    }
                    else
                    {
                        // ... (остальной код обработки неудачного входа)
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private string GetIPAddress()
        {
            return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
                .AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
        }

        private void ShowMainWindow(SystemUsers systemUser, int userRole)
        {
            Window mainWindow;
            var user = new UserAdapter(systemUser); // Преобразуем SystemUsers в User

            switch (userRole)
            {
                case 1: // Администратор
                    mainWindow = new AdminWindow(user);
                    break;
                case 2: // Лаборант-исследователь
                    mainWindow = new LabResearcherWindow(user, sessionTimer);
                    break;
                case 3: // Бухгалтер
                    mainWindow = new AccountantWindow(user);
                    break;
                case 4: // Лаборант
                    var timer = new DispatcherTimer();
                    timer.Tag = DateTime.Now;
                    mainWindow = new LabAssistantWindow(user, timer, ((MainWindow)Application.Current.MainWindow).MainFrame);
                    break;
                default:
                    MessageBox.Show("Неизвестная роль пользователя!");
                    return;
            }

            mainWindow.Show();
            Window.GetWindow(this)?.Close();
        }

        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            blockTimer.Stop();
            btnLogin.IsEnabled = true;
            lblMessage.Visibility = Visibility.Hidden;
        }

        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            if (!sessionStartTime.HasValue) return;

            var sessionDuration = DateTime.Now - sessionStartTime.Value;
            var totalSessionMinutes = 10;
            var warningMinutes = 5;

            if (sessionDuration.TotalMinutes >= totalSessionMinutes - warningMinutes &&
                sessionDuration.TotalMinutes < totalSessionMinutes)
            {
                MessageBox.Show($"Внимание! До окончания сеанса осталось {warningMinutes} минут.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (sessionDuration.TotalMinutes >= totalSessionMinutes)
            {
                sessionTimer.Stop();
                MessageBox.Show("Время сеанса истекло. Система будет заблокирована на 1 минуту.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                var loginWindow = new LoginPage();
                Window.GetWindow(this)?.Close();

                var blockWindow = new BlockWindow(60);
                blockWindow.ShowDialog();
            }
        }
    }
}
