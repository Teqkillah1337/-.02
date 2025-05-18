using System.Windows;
namespace MedicalLaboratory
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Открываем только MainWindow
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

    }
}