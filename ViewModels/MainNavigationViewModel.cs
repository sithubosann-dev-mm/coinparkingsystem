using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinParkingSystem.Models; // 

namespace CoinParkingSystem.ViewModels
{
    public class MainNavigationViewModel : BaseViewModel
    {
        // စနစ်မှာ Screen တစ်ခုကနေတစ်ခု ကူးပြောင်းတိုင်း (ဥပမာ- Entry ကနေ Main ပြန်သွားတိုင်း) ပါကင်အချက်အလက်တွေ ရိုးရိုးကြီး Reset ဖြစ်ပြီး ပျောက်မသွားစေဖို့အတွက်
        //MainNavigationViewModel ထဲမှာ SharedParkingSlots  ကို တစ်ခုတည်း ကြိုဆောက်ထားပြီး၊  Mem 2 (Dashboard)၊ Mem 3 (entry)၊ Member 4 (exit) တို့ဆီကို Dependency Injection (ပါရာမီတာဖြင့် ပေးပို့ခြင်း) စနစ်နဲ့ shareထားပါတယ်။
        //ဒါကြောင့် တစ်ယောက်က ပြင်လိုက်တဲ့ Real Data ကို အားလုံးက တပြိုင်နက်တည်း မှန်မှန်ကန်ကန် မြင်တွေ့နေရတာ ဖြစ်ပါတယ်
        //related to parkingsolot.cs
        public ObservableCollection<ParkingSlot> SharedParkingSlots { get; set; }

        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public void MapsToMain()
        {
            //for mem2
            CurrentViewModel = new MainViewModel(this, SharedParkingSlots); 
        }
        // for mem3
        public void MapsToEntry()
        {
            CurrentViewModel = new EntryViewModel(this, SharedParkingSlots);
        }
        //for mem4

        public void MapsToExit()
        {
            CurrentViewModel = new ExitViewModel(this, SharedParkingSlots);
        }

        public MainNavigationViewModel()
        {
           // built parking 15
            SharedParkingSlots = new ObservableCollection<ParkingSlot>();
            for (int i = 1; i <= 15; i++)
            {
                SharedParkingSlots.Add(new ParkingSlot
                {
                    SlotNumber = i,
                    IsOccupied = false
                });
            }

            MapsToMain();
        }
    }
}
