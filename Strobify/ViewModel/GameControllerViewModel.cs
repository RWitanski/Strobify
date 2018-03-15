namespace Strobify.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using Strobify.Helpers;
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using System.Collections.ObjectModel;

    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; } = new ObservableCollection<GameController>();
        private readonly IDeviceService _deviceService = SimpleIoc.Default.GetInstance<IDeviceService>();

        public GameControllerViewModel(IDeviceService deviceService)
        {
            this._deviceService = deviceService;
            InitCommands();
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

        private string _assignedControllerButtonText = "7";
        public string AssignedControllerButtonText
        {
            get { return _assignedControllerButtonText; }
            set { Set(ref _assignedControllerButtonText, value); }
        }
        private string _assignedKeyboardButtonText = "L";
        public string AssignedKeyboardButtonText
        {
            get { return _assignedKeyboardButtonText; }
            set
            {
                Set(ref _assignedKeyboardButtonText, value);
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
        //public RelayCommand GetKeyboardButtonCommand { get; private set; }
        public RelayCommand StartCommand { get; private set; }

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand((parameter) =>
                { InitGameControllerList(); }
            );
            this.GetButtonIdCommand = new RelayCommand((parameter) =>
                { InitControllerButtonAssign(); }, (paramater) =>  true  // add switch to true if there is a selected Item to avoid exception.
            );
            //this.GetKeyboardButtonCommand = new RelayCommand((parameter) =>
            //{ }, (parameter) => true
            //);
            this.StartCommand = new RelayCommand((parameter) =>
                { StartLightService(); }, (parameter) => true
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

        private void InitControllerButtonAssign()
        {
            _deviceService.AssignButtonsToController(SelectedDevice, AssignedKeyboardButtonText);
            AssignedControllerButtonText = _deviceService.GameController.ControllerButton.DeviceButtonId.ToString();
        }

        private void StartLightService()
        {
            _deviceService.Delay = Delay;
            _deviceService.Repeats = Repeats;
            _deviceService.StartLightService(_selectedDevice, AssignedControllerButtonText, AssignedKeyboardButtonText);
        }
    }
}