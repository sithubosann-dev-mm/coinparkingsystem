using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoinParkingSystem.Models
{
    public class ParkingSlot : INotifyPropertyChanged
    {
        private int _slotNumber;
        private bool _isOccupied;
        private string _carNumber = string.Empty;
        private string _entryTime = string.Empty;

        public string CarNumber
        {
            get => _carNumber;
            set { _carNumber = value; OnPropertyChanged(); }
        }

        public string EntryTime
        {
            get => _entryTime;
            set { _entryTime = value; OnPropertyChanged(); }
        }

        public int SlotNumber
        {
            get => _slotNumber;
            set { _slotNumber = value; OnPropertyChanged(); }
        }

        public bool IsOccupied
        {
            get => _isOccupied;
            set { _isOccupied = value; OnPropertyChanged(); }
        }

        public void RegisterVehicle(string carNumber, string phoneNumber)
        {
            CarNumber = carNumber;
            EntryTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            IsOccupied = true;
        }

        public void ClearVehicle()
        {
            CarNumber = string.Empty;
            EntryTime = string.Empty;
            IsOccupied = false;
        }

        public bool VerifyExitSecurity(string inputCarNumber)
        {
            if (string.IsNullOrEmpty(CarNumber)) return false;
            return CarNumber == inputCarNumber;
        }


        public int CalculateParkingFee(DateTime endTime)
        {
   
            if (string.IsNullOrEmpty(EntryTime) || !DateTime.TryParse(EntryTime, out DateTime startDateTime))
            {
                return 0;
            }

            if (endTime <= startDateTime) return 0;

            TimeSpan duration = endTime - startDateTime;
            double totalMinutes = duration.TotalMinutes;

         
            int blocksOf30Min = (int)(Math.Ceiling(totalMinutes / 30.0));
            int calculatedFee = blocksOf30Min * 200;

         
            int maxLimit = 1200; 

           
            if (startDateTime.DayOfWeek != DayOfWeek.Saturday && startDateTime.DayOfWeek != DayOfWeek.Sunday)
            {
                maxLimit = 700;
            }
            if (calculatedFee > maxLimit)
            {
                calculatedFee = maxLimit;
            }

            return calculatedFee;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}