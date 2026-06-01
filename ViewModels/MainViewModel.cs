using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CoinParkingSystem.Commands;
using CoinParkingSystem.Models;

namespace CoinParkingSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainNavigationViewModel _mainNav;

        public ObservableCollection<ParkingSlot> ParkingSlots { get; set; }

        private int _availableSlotsCount;
        private MainNavigationViewModel mainNavigationViewModel;

        public int AvailableSlotsCount
        {
            get => _availableSlotsCount;
            set
            {
                _availableSlotsCount = value;
                OnPropertyChanged();
            }
        }

        public ICommand MapsToEntryCommand { get; }
        public ICommand MapsToExitCommand { get; }
        public ICommand UpdateAvailableSlotsCommand { get; }

        public MainViewModel(
            MainNavigationViewModel mainNav,
            ObservableCollection<ParkingSlot> sharedSlots)
        {
            _mainNav = mainNav;
            ParkingSlots = sharedSlots;

            MapsToEntryCommand =
                new RelayCommand(_ => _mainNav.MapsToEntry());

            MapsToExitCommand =
                new RelayCommand(_ => _mainNav.MapsToExit());

            UpdateAvailableSlotsCommand =
                new RelayCommand(_ => UpdateAvailableSlotsCount());

            foreach (ParkingSlot slot in ParkingSlots)
            {
                slot.PropertyChanged += Slot_PropertyChanged;
            }

            UpdateAvailableSlotsCount();
        }

        public MainViewModel(MainNavigationViewModel mainNavigationViewModel)
        {
            this.mainNavigationViewModel = mainNavigationViewModel;
        }

        private void Slot_PropertyChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ParkingSlot.IsOccupied))
            {
                UpdateAvailableSlotsCount();
            }
        }

        public void UpdateAvailableSlotsCount()
        {
            AvailableSlotsCount =
                ParkingSlots.Count(slot => !slot.IsOccupied);
        }
    }
}