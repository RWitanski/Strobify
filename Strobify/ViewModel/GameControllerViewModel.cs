using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Strobify.Helpers;
namespace Strobify.ViewModel
{
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using System.Collections.ObjectModel;

    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; } = new ObservableCollection<GameController>();
        private readonly IDeviceService _deviceService = SimpleIoc.Default.GetInstance<IDeviceService>();

        GameController _selectedDevice;
        public GameController SelectedDevice
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

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand((parameter) =>
                { InitGameControllerList(); }
            );
        }

        private void InitGameControllerList()
        {
            GameControllers.Clear();
            
            foreach (var device in _deviceService.GetDevices())
            {
                GameControllers.Add(device);
            }
        }

        public GameControllerViewModel()
        {
            InitCommands();
        }
    }
}