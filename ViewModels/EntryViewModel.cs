using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CoinParkingSystem.Commands;
using CoinParkingSystem.Models;

namespace CoinParkingSystem.ViewModels
{
    public class EntryViewModel : BaseViewModel
    {
        private readonly MainNavigationViewModel _mainNav;
        private readonly object _navService; // Team ၏ တတိယ Parameter အား ထိန်းသိမ်းရန်
        private string _popupTitle = string.Empty;
        private bool _isPopupVisible = false;
        private string _inputPlateRegion = string.Empty;
        private string _inputPlateSecret = string.Empty;
        private string _carNoPart1 = string.Empty;
        private string _carNoPart2 = string.Empty;
        private ParkingSlot _selectedSlot;

        public ObservableCollection<ParkingSlot> ParkingSlots { get; set; }

        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }
        public bool IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }
        public string InputPlateRegion { get => _inputPlateRegion; set { _inputPlateRegion = value; OnPropertyChanged(); } }
        public string InputPlateSecret { get => _inputPlateSecret; set { _inputPlateSecret = value; OnPropertyChanged(); } }
        public string CarNoPart1 { get => _carNoPart1; set { _carNoPart1 = value; OnPropertyChanged(); } }
        public string CarNoPart2 { get => _carNoPart2; set { _carNoPart2 = value; OnPropertyChanged(); } }

        public ICommand SelectSlotCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ConfirmCommand { get; }

        // 🎯 [🚨 FIXED: CS1729] Arguments ၃ ခုလက်ခံနိုင်ရန် Constructor အား အချောသတ်ပြင်ဆင်ခြင်း
        public EntryViewModel(MainNavigationViewModel mainNav, object navService, ObservableCollection<ParkingSlot> sharedSlots)
        {
            _mainNav = mainNav;
            _navService = navService;
            ParkingSlots = sharedSlots;

            SelectSlotCommand = new RelayCommand(param =>
            {
                if (param == null) return;
                int slotNum = Convert.ToInt32(param);
                _selectedSlot = ParkingSlots.FirstOrDefault(s => s.SlotNumber == slotNum);

                if (_selectedSlot == null) return;

                if (_selectedSlot.IsOccupied)
                {
                    MessageBox.Show($"車室 No. {slotNum} は満車です.။", "案内", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                PopupTitle = $"【No. {slotNum} 入庫手続き】";
                ClearInputFields();
                IsPopupVisible = true;
            });

            CancelCommand = new RelayCommand(_ => IsPopupVisible = false);

            ConfirmCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrEmpty(CarNoPart1) || string.IsNullOrEmpty(CarNoPart2))
                {
                    MessageBox.Show("車両番号を入力してください.", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string fullCarNumber = $"{CarNoPart1}{CarNoPart2}";
                _selectedSlot.CarNumber = fullCarNumber;
                _selectedSlot.EntryTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                _selectedSlot.IsOccupied = true;

                IsPopupVisible = false;
                MessageBox.Show($"No. {_selectedSlot.SlotNumber} 駐車手続き完了しました.", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void ClearInputFields()
        {
            InputPlateRegion = string.Empty;
            InputPlateSecret = string.Empty;
            CarNoPart1 = string.Empty;
            CarNoPart2 = string.Empty;
        }
    }
}