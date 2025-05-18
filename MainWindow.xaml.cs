using System.Windows;
using System.Windows.Threading;
namespace MedicalLaboratory
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage());
        }
        public void OpenLabAssistantWindow(User user, DispatcherTimer timer)
        {
            var labAssistantWindow = new LabAssistantWindow(user, timer, MainFrame);
            labAssistantWindow.Show();
        }
    }
}