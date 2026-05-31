using System.Windows;
using CoinParkingSystem.ViewModels;

namespace CoinParkingSystem
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // ဗဟို မောင်းနှင်မည့် စနစ်ကြီးကို နှိုးလိုက်ခြင်း
            var navigationVM = new MainNavigationViewModel();

            // MainWindow ကို ဆွဲဖွင့်ပြီး စနစ်နှင့် ချိတ်ဆက်ခြင်း
            var mainWindow = new MainWindow
            {
                DataContext = navigationVM
            };
            MainWindow.Show();
        }
    }
}