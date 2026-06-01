using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinParkingSystem.ViewModels;

namespace CoinParkingSystem.Models
{
    //for real time, entry and exit data
    public class ParkingSlot : BaseViewModel
    {
        public int SlotNumber { get; set; }

        private bool _isOccupied;
        public bool IsOccupied
        {
            get => _isOccupied;
            set
            {
                _isOccupied = value;
                OnPropertyChanged();
            }
        }

        public DateTime? EntryTime { get; set; }
    }
}
