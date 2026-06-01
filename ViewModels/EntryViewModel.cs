using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoinParkingSystem.Commands;
using CoinParkingSystem.Models;
using System.Collections.ObjectModel;

namespace CoinParkingSystem.ViewModels
{
    public class EntryViewModel : BaseViewModel
    {
        private readonly MainNavigationViewModel _mainNav;

        public ObservableCollection<ParkingSlot> ParkingSlots { get; set; }

        private ParkingSlot _selectedSlot;
        public ParkingSlot SelectedSlot
        {
            get => _selectedSlot;
            set
            {
                _selectedSlot = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectSlotCommand { get; }

        public EntryViewModel(MainNavigationViewModel mainNav, ObservableCollection<ParkingSlot> sharedSlots)
        {
            _mainNav = mainNav;
            ParkingSlots = sharedSlots;

            SelectSlotCommand = new RelayCommand(SelectParkingSlot);
        }

        public void SelectParkingSlot(object parameter)
        {
            if (parameter == null) return;

            int slotNumber = int.Parse(parameter.ToString());
            SelectedSlot = ParkingSlots[slotNumber - 1];

            if (SelectedSlot.IsOccupied)
            {
                MessageBox.Show($"パーキング{slotNumber}は既に満車です。", "確認", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"パーキング{slotNumber}を登録しますか?",
                "確認",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                RegisterCarEntry();
            }
        }

        public void RegisterCarEntry()
        {
            if (SelectedSlot == null)
                return;

            SelectedSlot.IsOccupied = true;
            SelectedSlot.EntryTime = DateTime.Now;

            MessageBox.Show(
                $"パーキング{SelectedSlot.SlotNumber}が登録されました。",
                "完了",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            _mainNav?.MapsToMain();
        }
    }
}