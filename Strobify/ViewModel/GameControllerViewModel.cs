namespace Strobify.ViewModel
{
    using System.Linq;
    using Strobify.Model;
    using Strobify.Helpers;
    using GalaSoft.MvvmLight;
    using Strobify.Services.Interfaces;
    using System.Collections.ObjectModel;

    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; } = new ObservableCollection<GameController>();
        private readonly IDeviceService _deviceService;
        private readonly ILightService _lightService;

        public GameControllerViewModel(IDeviceService deviceService, ILightService lightService)
        {
            this._deviceService = deviceService;
            this._lightService = lightService;
            InitCommands();
            InitGameControllerList();
            StartLightService();
        }

        private GameController _selectedDevice;
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

        private string _controllerButtonText = "7";
        public string ControllerButtonText
        {
            get { return _controllerButtonText; }
            set { Set(ref _controllerButtonText, value); }
        }
        private string _keyboardButtonText = "L";
        public string KeyboardButtonText
        {
            get { return _keyboardButtonText; }
            set
            {
                Set(ref _keyboardButtonText, value);
            }
        }

        private short _delay = 150;
        public short Delay
        {
            get { return _delay; }
            set { Set(ref _delay, value); }
        }

        private short _repeats = 12;
        public short Repeats
        {
            get { return _repeats; }
            set { Set(ref _repeats, value); }
        }

        public RelayCommand GetDevicesCommand { get; private set; }
        public RelayCommand GetButtonIdCommand { get; private set; }
        public RelayCommand StartCommand { get; private set; }

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand((parameter) => InitGameControllerList()
            );
            this.GetButtonIdCommand = new RelayCommand((parameter) => InitControllerButtonAssign(), (paramater) => true  // add switch to true if there is a selected Item to avoid exception.
            );
            this.StartCommand = new RelayCommand((parameter) => StartLightService(), (parameter) => true
            );
        }

        public void InitGameControllerList()
        {
            GameControllers.Clear();
            foreach (var device in _deviceService.GetDevices())
            {
                GameControllers.Add(device);
            }
            SelectedDevice = GameControllers.FirstOrDefault();
        }

        private void InitControllerButtonAssign()
        {
            if (SelectedDevice != null)
            {
                _deviceService.AssignButtonsToController(SelectedDevice, KeyboardButtonText);
                ControllerButtonText = _deviceService.GetGameControllerButtonId(_selectedDevice);
            }
        }

        private void StartLightService()
        {
            if (SelectedDevice != null)
            {
                _lightService.Delay = Delay;
                _lightService.Repeats = Repeats;
                _deviceService.StartLightService(SelectedDevice, ControllerButtonText, KeyboardButtonText);
            }
        }
    }
}