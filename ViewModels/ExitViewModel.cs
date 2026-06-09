using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CoinParkingSystem.Commands;
using CoinParkingSystem.Models;

namespace CoinParkingSystem.ViewModels
{
    public class ExitViewModel : BaseViewModel
    {
        private readonly MainNavigationViewModel _mainNav;
        private readonly object _navService;
        private readonly ReceiptService _receiptService;

        public ObservableCollection<ParkingSlot> SharedParkingSlots { get; set; }

        private ParkingSlot _selectedParkingSlot;
        private int _parkingFeeValue;
        private string _slotNumberInput = string.Empty;
        private string _entryTimeDisplay = "---";
        private string _exitTimeDisplay = "---";
        private string _durationDisplay = "---";
        private string _feeResultDisplay = "0";
        private string _paymentMethod = "現金";
        private string _cashInput = string.Empty;
        private string _paymentResult = "正しく番号を入力し料金を確認後、お支払いボタンを押してください。";

        public string SlotNumberInput { get => _slotNumberInput; set { _slotNumberInput = value; OnPropertyChanged(); } }
        public string EntryTimeDisplay { get => _entryTimeDisplay; set { _entryTimeDisplay = value; OnPropertyChanged(); } }
        public string ExitTimeDisplay { get => _exitTimeDisplay; set { _exitTimeDisplay = value; OnPropertyChanged(); } }
        public string DurationDisplay { get => _durationDisplay; set { _durationDisplay = value; OnPropertyChanged(); } }
        public string FeeResultDisplay { get => _feeResultDisplay; set { _feeResultDisplay = value; OnPropertyChanged(); } }

        public string PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                _paymentMethod = value;
                OnPropertyChanged();
                UpdateLivePaymentStatus();
            }
        }

        public string CashInput
        {
            get => _cashInput;
            set
            {
                _cashInput = value;
                OnPropertyChanged();
                UpdateLivePaymentStatus();
            }
        }

        public string PaymentResult { get => _paymentResult; set { _paymentResult = value; OnPropertyChanged(); } }

        public ParkingSlot SelectedParkingSlot
        {
            get => _selectedParkingSlot;
            set
            {
                _selectedParkingSlot = value;
                OnPropertyChanged();
                if (_selectedParkingSlot != null)
                {
                    SlotNumberInput = _selectedParkingSlot.SlotNumber.ToString();
                    EntryTimeDisplay = _selectedParkingSlot.EntryTime;
                }
            }
        }

        //calculate total time and fee
        public int ParkingFeeValue
        {
            get => _parkingFeeValue;
            set
            {
                _parkingFeeValue = value;
                OnPropertyChanged();
                FeeResultDisplay = _parkingFeeValue.ToString();

                ExitTimeDisplay = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                if (SelectedParkingSlot != null && DateTime.TryParse(SelectedParkingSlot.EntryTime, out DateTime startDateTime))
                {
                    TimeSpan duration = DateTime.Now - startDateTime;
                    int totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);

                    DurationDisplay = $"{totalMinutes} 分";
                }
                else
                {
                    DurationDisplay = "0 分";
                }

                UpdateLivePaymentStatus();
            }
        }

        public ICommand CalculateFeeCommand { get; }
        public ICommand PayCommand { get; }
        public ICommand CancelCommand { get; }

        public ExitViewModel(MainNavigationViewModel mainNav, object navService, ObservableCollection<ParkingSlot> sharedSlots)
        {
            _mainNav = mainNav;
            _navService = navService;
            SharedParkingSlots = sharedSlots;
             _receiptService = new ReceiptService();

            CalculateFeeCommand = new RelayCommand(_ =>
            {
                if (SelectedParkingSlot == null) return;
                ExitTimeDisplay = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ParkingFeeValue = SelectedParkingSlot.CalculateParkingFee(DateTime.Now);
                UpdateLivePaymentStatus();
            });

            PayCommand = new RelayCommand(_ =>
            {
                if (SelectedParkingSlot == null) return;

                string cleanMethodName = PaymentMethod.Replace("System.Windows.Controls.ComboBoxItem: ", "");

               
                if (cleanMethodName == "現金")
                {
                    if (!int.TryParse(CashInput, out int inserted) || inserted < ParkingFeeValue)
                    {
                        MessageBox.Show("投入金額が不足しています。正しい金額を入力してください。", "支払いエラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int change = inserted - ParkingFeeValue;
                    MessageBox.Show($"{cleanMethodName} での精算が完了しました！\nお釣り: {change} 円\nご利用ありがとうございました！", "精算完了", MessageBoxButton.OK, MessageBoxImage.Information);
                    try
                    {
                        _receiptService.GenerateCustomerReceipt(SelectedParkingSlot.SlotNumber, ParkingFeeValue);
                        _receiptService.SaveDailyIncome(SelectedParkingSlot.SlotNumber, ParkingFeeValue);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"レシートの保存中にエラーが発生しました: {ex.Message}", "レシートエラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
            
                    MessageBox.Show($"{cleanMethodName} での精算が完了しました！\nご利用ありがとうございました！", "精算完了", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                CompletePaymentProcess();
            });

            CancelCommand = new RelayCommand(_ => ReturnToMainDashboard());
        }

     
        private void UpdateLivePaymentStatus()
        {
            string cleanMethodName = PaymentMethod.Replace("System.Windows.Controls.ComboBoxItem: ", "");

            if (cleanMethodName == "現金")
            {
                if (int.TryParse(CashInput, out int inserted))
                {
                    if (inserted >= ParkingFeeValue)
                    {
                        PaymentResult = $"投入金額 : {inserted} 円\nお釣り (Change) : {inserted - ParkingFeeValue} 円\n\n「支払い」ボタンを押して精算を確定してください。";
                    }
                    else
                    {
                        PaymentResult = $"投入金額 : {inserted} 円\n不足金額 (Shortage) : {ParkingFeeValue - inserted} 円\n\nお金が不足しています。追加投入してください。";
                    }
                }
                else
                {
                    PaymentResult = $"請求金額 : {FeeResultDisplay} 円\n\n下の入力欄に投入金額を入力してください。";
                }
            }
            else
            {
                PaymentResult = $"決済方法 : {cleanMethodName}\n請求金額 : {FeeResultDisplay} 円\n\nお釣りはありません。「支払い」ボタンを押してください。";
            }
        }

        private void CompletePaymentProcess()
        {
            if (SelectedParkingSlot != null) SelectedParkingSlot.ClearVehicle();
            ReturnToMainDashboard();
        }

        private void ReturnToMainDashboard()
        {
            if (_mainNav != null) _mainNav.CurrentView = new MainViewModel(_mainNav, SharedParkingSlots);
        }     
    }
}
