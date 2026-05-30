using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinParkingSystem.Models;
using CoinParkingSystem.ViewModels;

namespace CoinParkingSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainNavigationViewModel _mainNav;
        public ObservableCollection<ParkingSlot> ParkingSlots { get; set; }

        // shared data from parkingslot.cs << for show real data.
        // //no need to change< check parkingslot.cs bro
        public MainViewModel(MainNavigationViewModel mainNav, ObservableCollection<ParkingSlot> sharedSlots = null)
        {
            _mainNav = mainNav;
            ParkingSlots = sharedSlots; // 
        }
    }
}
