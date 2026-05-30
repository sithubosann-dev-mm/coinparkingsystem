using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinParkingSystem.Services;
using CoinParkingSystem.Commands;
using CoinParkingSystem.Models;
using System.Collections.ObjectModel;

namespace CoinParkingSystem.ViewModels
{

    public class ExitViewModel : BaseViewModel
    {
        private readonly MainNavigationViewModel _mainNav;
        public ObservableCollection<ParkingSlot> ParkingSlots { get; set; }

        
        public ExitViewModel()
        {
        }

        public ExitViewModel(MainNavigationViewModel mainNav, ObservableCollection<ParkingSlot> sharedSlots = null)
        {
            _mainNav = mainNav;
            ParkingSlots = sharedSlots;
        }

        
    }
}
