using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CoinParkingSystem.Commands;
using CoinParkingSystem.Models;

namespace CoinParkingSystem.ViewModels
{
    // [Dashboard (MainView) အတွက် 
    public class MainViewModel : BaseViewModel
    {
        // Screen ကူးပြောင်းခြင်းများကို လှမ်း၍ အမိန့်ပေးနိုင်ရန် ဗဟို Navigation 
        private readonly MainNavigationViewModel _mainNav;

        // ဗဟို Shared Memory မှ ပါကင်အကွက် ၁၅ ကွက် စာရင်းကို Dashboard
        public ObservableCollection<ParkingSlot> ParkingSlots { get; set; }

        private int _availableSlotsCount;

        // UI ပေါ်ပါကင်အရေအတွက်" စာသားကို Real-time
        public int AvailableSlotsCount
        {
            get => _availableSlotsCount;
            set
            {
                _availableSlotsCount = value;
                OnPropertyChanged(); // အရေအတွက် ကိန်းဂဏန်း ပြောင်းသွားတိုင်း UI ကို ချက်ချင်း အလိုအလျောက် သွားပြင်
            }
        }

        // XAML ဘက်က ခလုတ်များနှင့် ချိတ်ဆက်မည့် (Commands) များ
        public ICommand MapsToEntryCommand { get; }
        public ICommand MapsToExitCommand { get; }
        public ICommand UpdateAvailableSlotsCommand { get; }

        // 
        // ဗဟို Navigation စနစ်နှင့် Shared Slots ဒေတာများကို Dependency Injection စနစ်ဖြင့် တိုက်ရိုက် လှမ်းယူလိုက်ခြင်း
        public MainViewModel(MainNavigationViewModel mainNav, ObservableCollection<ParkingSlot> sharedSlots)
        {
            _mainNav = mainNav;
            ParkingSlots = sharedSlots; // ဗဟိုမှ ပေးလိုက်သော ပါကင် Slots ၁၅ ကွက်ကို ဒေတာအဖြစ် သိမ်းဆည်းခြင်း

           
            MapsToEntryCommand = new RelayCommand(_ => _mainNav.MapsToEntry());
            MapsToExitCommand = new RelayCommand(_ => _mainNav.MapsToExit());
            UpdateAvailableSlotsCommand = new RelayCommand(_ => UpdateAvailableSlotsCount());

            // ပါကင် ၁၅ ကွက်လုံးကို စောင့်ကြည့်ပြီး ကားအဝင်အထွက်ကြောင့် အပြောင်းအလဲဖြစ်တိုင်း သိရှိနိုင်ရန် သော့ခတ် (Event ချိတ်)
            foreach (ParkingSlot slot in ParkingSlots)
            {
                slot.PropertyChanged += Slot_PropertyChanged;
            }

            // စတင်ပွင့်ချိန်တွင် အားနေသော ပါကင်အရေအတွက်ကို အရင်ဆုံး တွက်ချက်ခြင်း
            UpdateAvailableSlotsCount();
        }

        
        private void Slot_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            if (e.PropertyName == nameof(ParkingSlot.IsOccupied))
            {
                // 
                UpdateAvailableSlotsCount();
            }
        }

        //
        public void UpdateAvailableSlotsCount()
        {
            // ပါကင် ၁၅ ကွက်ထဲမှ ကားမရှိသေးသော (IsOccupied == false) 
            AvailableSlotsCount = ParkingSlots.Count(slot => !slot.IsOccupied);
        }
    }
}