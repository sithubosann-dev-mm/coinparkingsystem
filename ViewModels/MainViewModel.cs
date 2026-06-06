using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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
        private string _typedSlotNumber = string.Empty;
        private string _carPlateNumber = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _selectedSlotGuide = string.Empty;
        private string _actionButtonText = "確定 / ENTER";
        private bool _isInputtingDetails = false;
        private int _availableSlotsCount;

        public ObservableCollection<ParkingSlot> ParkingSlots { get; set; }

        public string TypedSlotNumber
        {
            get => _typedSlotNumber;
            set { _typedSlotNumber = value; OnPropertyChanged(); }
        }

        public string CarPlateNumber
        {
            get => _carPlateNumber;
            set
            {
                _carPlateNumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedCarPlateNumber));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedPhoneNumber));
            }
        }

        public string FormattedCarPlateNumber
        {
            get
            {
                if (string.IsNullOrEmpty(CarPlateNumber)) return "";
                if (CarPlateNumber.Length <= 2) return CarPlateNumber;
                return $"{CarPlateNumber.Substring(0, 2)}-{CarPlateNumber.Substring(2)}";
            }
            set
            {
                CarPlateNumber = value?.Replace("-", "") ?? string.Empty;
            }
        }

        public string FormattedPhoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(PhoneNumber)) return "";
                if (PhoneNumber.Length <= 3) return PhoneNumber;
                if (PhoneNumber.Length <= 7) return $"{PhoneNumber.Substring(0, 3)}-{PhoneNumber.Substring(3)}";
                return $"{PhoneNumber.Substring(0, 3)}-{PhoneNumber.Substring(3, 4)}-{PhoneNumber.Substring(7)}";
            }
            set
            {
                PhoneNumber = value?.Replace("-", "") ?? string.Empty;
            }
        }

        public string SelectedSlotGuide { get => _selectedSlotGuide; set { _selectedSlotGuide = value; OnPropertyChanged(); } }
        public string ActionButtonText { get => _actionButtonText; set { _actionButtonText = value; OnPropertyChanged(); } }
        public bool IsInputtingDetails { get => _isInputtingDetails; set { _isInputtingDetails = value; OnPropertyChanged(); } }
        public int AvailableSlotsCount { get => _availableSlotsCount; set { _availableSlotsCount = value; OnPropertyChanged(); } }

        public ICommand NumberClickCommand { get; }
        public ICommand ClearClickCommand { get; }
        public ICommand MainActionCommand { get; }
        public ICommand BackClickCommand { get; }

        public MainViewModel(MainNavigationViewModel mainNav, ObservableCollection<ParkingSlot> sharedSlots)
        {
            _mainNav = mainNav;
            ParkingSlots = sharedSlots;

            NumberClickCommand = new RelayCommand(param =>
            {
                if (param == null) return;
                string input = param.ToString();

                if (!IsInputtingDetails)
                {
                    if (string.IsNullOrEmpty(TypedSlotNumber) && input == "0") return;
                    if (input == "-") return;

                    if (TypedSlotNumber.Length < 2)
                    {
                        string preview = TypedSlotNumber + input;
                        if (int.TryParse(preview, out int parsed) && parsed > 15)
                        {
                            MessageBox.Show("駐車スペース番号は1から15の間で選択してください。", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                            TypedSlotNumber = string.Empty;
                            return;
                        }
                        TypedSlotNumber = preview;
                    }
                }
                else
                {
                    if (CarPlateNumber.Length < 4)
                    {
                        if (input == "-") return;
                        CarPlateNumber += input;
                    }
                    else if (PhoneNumber.Length < 11)
                    {
                        var targetSlot = ParkingSlots.FirstOrDefault(s => s.SlotNumber == int.Parse(TypedSlotNumber));
                        if (targetSlot != null && targetSlot.IsOccupied) return;

                        if (input == "-") return;
                        PhoneNumber += input;
                    }
                }
            });

            ClearClickCommand = new RelayCommand(_ =>
            {
                if (!IsInputtingDetails)
                {
                    TypedSlotNumber = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(PhoneNumber)) PhoneNumber = string.Empty;
                    else CarPlateNumber = string.Empty;
                }
            });

            MainActionCommand = new RelayCommand(_ => ProcessMainFlow());
            BackClickCommand = new RelayCommand(_ => ResetToStep1());

            foreach (var slot in ParkingSlots) slot.PropertyChanged += Slot_PropertyChanged;
            UpdateAvailableSlotsCount();
        }

        private void ProcessMainFlow()
        {
            if (!IsInputtingDetails)
            {
                if (!int.TryParse(TypedSlotNumber, out int slotNum) || slotNum < 1 || slotNum > 15)
                {
                    MessageBox.Show("正しい駐車スペース番号(1-15)を入力してください。", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var targetSlot = ParkingSlots.FirstOrDefault(s => s.SlotNumber == slotNum);
                if (targetSlot == null) return;

                if (!targetSlot.IsOccupied)
                {
                    SelectedSlotGuide = $"【No. {slotNum} 入庫手続き】";
                    ActionButtonText = "入庫";
                }
                else
                {
                    SelectedSlotGuide = $"【No. {slotNum} 出庫手続き】";
                    ActionButtonText = "出庫 / 精算";
                }

                CarPlateNumber = string.Empty;
                PhoneNumber = string.Empty;
                IsInputtingDetails = true;
            }
            else
            {
                int slotNum = int.Parse(TypedSlotNumber);
                var targetSlot = ParkingSlots.FirstOrDefault(s => s.SlotNumber == slotNum);
                if (targetSlot == null) return;

                if (!targetSlot.IsOccupied)
                {
                    if (CarPlateNumber.Length < 4)
                    {
                        MessageBox.Show("車両番号を4桁で入力してください。", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    targetSlot.RegisterVehicle(FormattedCarPlateNumber, FormattedPhoneNumber);
                    MessageBox.Show($"No. {slotNum} への入庫手続きが完了しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetToStep1();
                }
                else
                {
                    if (string.IsNullOrEmpty(CarPlateNumber))
                    {
                        MessageBox.Show("車両番号を入力してください。", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (!targetSlot.VerifyExitSecurity(FormattedCarPlateNumber))
                    {
                        MessageBox.Show("車両番号が一致しません。正しい番号を入力してください。", "セキュリティエラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        CarPlateNumber = string.Empty;
                        return;
                    }

                    DateTime endTime = DateTime.Now;
                    int parkingFee = targetSlot.CalculateParkingFee(endTime);

                    string startTimeStr = targetSlot.EntryTime;
                    string endTimeStr = endTime.ToString("yyyy/MM/dd HH:mm:ss");

                    string ticketMessage = "----------------------------------------\n" +
                                           "   【 出庫内容確認 / EXIT TICKET 】\n" +
                                           "----------------------------------------\n\n" +
                                           $"   駐車番号 (Slot No)  :    No. {slotNum}\n\n" +
                                           $"   車両番号 (Plate No) :    {FormattedCarPlateNumber}\n\n" +
                                           $"   入庫時間 (Start)    :    {startTimeStr}\n\n" +
                                           $"   出庫時間 (End)      :    {endTimeStr}\n\n" +
                                           $"   駐車料金 (Fee)      :    {parkingFee} 円\n\n" +
                                           "----------------------------------------\n" +
                                           "   精算画面 (Kaikei) へ進みますか？";

                    MessageBoxResult result = MessageBox.Show(ticketMessage, "出庫確認", MessageBoxButton.YesNo, MessageBoxImage.None);

                    if (result == MessageBoxResult.Yes)
                    {
                        if (_mainNav != null)
                        {
                            // 🎯 [🚨 FIXED CRITICAL NAVIGATION LINK]: မင်းတို့ Team ရဲ့ ExitViewModel တည်ဆောက်ပုံစနစ်အတိုင်း ကွက်တိချိတ်ဆက်လိုက်ခြင်း!
                            var exitVM = new ExitViewModel(_mainNav, new object(), ParkingSlots);


                                exitVM.SelectedParkingSlot = targetSlot;
                                exitVM.ParkingFeeValue = parkingFee;

                              _mainNav.CurrentView = exitVM;

                            // 💡 တွက်ချက်ထားသော ကားခအား ExitViewModel ထဲသို့ စမတ်ကျကျ လှမ်းထည့်ပေးလိုက်ခြင်း



                        }
                    }
                    else return;

                    ResetToStep1();
                }
            }
        }

        private void ResetToStep1()
        {
            TypedSlotNumber = string.Empty;
            CarPlateNumber = string.Empty;
            PhoneNumber = string.Empty;
            ActionButtonText = "確定 / ENTER";
            IsInputtingDetails = false;
        }

        private void Slot_PropertyChanged(object sender, PropertyChangedEventArgs e) => UpdateAvailableSlotsCount();
        public void UpdateAvailableSlotsCount() => AvailableSlotsCount = ParkingSlots.Count(slot => !slot.IsOccupied);
    }
}