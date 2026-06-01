using System;
using System.Collections.ObjectModel;
using CoinParkingSystem.Models;

namespace CoinParkingSystem.ViewModels
{
    public class MainNavigationViewModel : BaseViewModel
    {
        private object _currentView;
        private object _navigationService; // Team အဖွဲ့ဝင်များ သုံးထားသော Service Parameter

        public ObservableCollection<ParkingSlot> SharedParkingSlots { get; set; }

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public MainNavigationViewModel()
        {
            SharedParkingSlots = new ObservableCollection<ParkingSlot>();
            for (int i = 1; i <= 15; i++)
            {
                SharedParkingSlots.Add(new ParkingSlot { SlotNumber = i, IsOccupied = false });
            }

            _navigationService = new object();

            NavigateToMain();
        }

        public void NavigateToMain()
        {
            CurrentView = new MainViewModel(this, SharedParkingSlots);
        }

        public void NavigateToEntry()
        {
            CurrentView = new EntryViewModel(this, _navigationService, SharedParkingSlots);
        }

        public void NavigateToExit()
        {
            CurrentView = new ExitViewModel(this, _navigationService, SharedParkingSlots);
        }
    }
}