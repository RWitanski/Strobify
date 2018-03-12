namespace Strobify.ViewModel
{   
    using Strobify.Model;
    using Strobify.Helpers;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using Strobify.Services.Interfaces;
    using System.Collections.ObjectModel;

    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; } = new ObservableCollection<GameController>();
        private readonly IDeviceService _deviceService = SimpleIoc.Default.GetInstance<IDeviceService>();

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

        private string _assignedControllerButtonText;
        public string AssignedControllerButtonText
        {
            get { return _assignedControllerButtonText; }
            set { Set(ref _assignedControllerButtonText, value); }
        }
        private string _assignedKeyboardButtonText;
        public string AssignedKeyboardButtonText
        {
            get { return _assignedKeyboardButtonText; }
            set { Set(ref _assignedKeyboardButtonText, value); }
        }

        public RelayCommand GetDevicesCommand { get; set; }
        public RelayCommand GetButtonIdCommand { get; set; }
        public RelayCommand GetKeyboardButtonCommand { get; set; }

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand((parameter) =>
                { InitGameControllerList(); }
            );
            this.GetButtonIdCommand = new RelayCommand((parameter) =>
                { InitControllerButtonAssign(); }, (paramater) =>  true  // add switch to true if there is a selected Item to avoid exception.
            );
            this.GetKeyboardButtonCommand = new RelayCommand((parameter) =>
            { }, (parameter) => true
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
            _deviceService.AssignControllerButtonId(_selectedDevice);
            AssignedControllerButtonText = _deviceService.GameController.ControllerButton.DeviceButtonId.ToString();
        }
        private void InitKeyboardButtonAssing()
        {

        }

        public GameControllerViewModel()
        {
            InitCommands();
        }
    }
}