using GalaSoft.MvvmLight;
using Strobify.Helpers;
using Strobify.Model;
using System;
using System.Collections.ObjectModel;

namespace Strobify.ViewModel
{
    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; } = new ObservableCollection<GameController>();

        object _selectedDevice;
        public object SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                Set(ref _selectedDevice, value);
            }
        }

        public RelayCommand GetDevicesCommand { get; set; }

        public string DevicesContent
        {
            get
            {
                return "Get devices";
            }
        }

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand((parameter) =>
                { InitGameControllerList(); }
            );
        }

        private void InitGameControllerList()
        {
            GameControllers.Clear();
            GameControllers.Add(new GameController { Name = "Kierownica", DeviceGuid = new Guid("13757675-0365-49DD-972D-D5954E7E7FD3") });
        }

        public GameControllerViewModel()
        {
            InitCommands();
        }
    }
}
