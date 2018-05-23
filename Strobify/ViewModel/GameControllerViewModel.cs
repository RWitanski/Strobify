namespace Strobify.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;
    using Strobify.Helpers;
    using Strobify.Messages;
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using Strobify.Strategies.Interfaces;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; } = new ObservableCollection<GameController>();

        public ObservableCollection<Mode> Modes { get; } = new ObservableCollection<Mode>{
            new Mode{ Name = "Race car", ModeType =  ModeType.RaceCar },
            new Mode{ Name = "Strobify race car", ModeType =  ModeType.StroboRaceCar },
            new Mode { Name = "Safety car", ModeType = ModeType.SafetyCar },
            new Mode { Name = "Strobify safety car", ModeType = ModeType.StroboSafetyCar }
        };

        private readonly IDeviceService _deviceService;
        private readonly ILightService _lightService;
        private readonly IButtonMapperStrategy _buttonMapperStrategy;
        private readonly IMessenger _messenger;
        private readonly IConfigurationService _configurationService;

        private GameController _selectedDevice;
        private Mode _selectedMode;
        private string _controllerButtonText;
        private string _keyboardButtonText;
        private short _delay;
        private short _repeats;
        private bool _isControllerButtonEnabled = true;
        private string _modeListVisibility = "Hidden";
        private Configuration _configuration;

        public GameControllerViewModel(IDeviceService deviceService, ILightService lightService, IButtonMapperStrategy buttonMapperStrategy, IMessenger messenger, IConfigurationService configurationService)
        {
            this._deviceService = deviceService;
            this._lightService = lightService;
            this._buttonMapperStrategy = buttonMapperStrategy;
            this._messenger = messenger;
            this._configurationService = configurationService;
            _messenger.Register<ButtonChangedMessage>(this, this.HandleButtonMessage);
            InitCommands();
            Configuration = configurationService.ReadConfiguration();
            InitGameControllerList(null);
        }

        #region Properties

        protected Configuration Configuration
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
                AssignConfiguration();
            }
        }

        public GameController SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                Set(ref _selectedDevice, value);
                if (_selectedDevice != null)
                {
                    _configurationService.Configuration.DeviceGuid = _selectedDevice.DeviceGuid;
                }
            }
        }

        public Mode SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                Set(ref _selectedMode, value);
                _lightService.CurrentMode = SelectedMode.ModeType;
            }
        }

        public bool IsControllerButtonEnabled
        {
            get { return _isControllerButtonEnabled; }
            set
            {
                Set(ref _isControllerButtonEnabled, value);
                RaisePropertyChanged();
            }
        }

        public string ModeListVisibility
        {
            get { return _modeListVisibility; }
            set { Set(ref _modeListVisibility, value); }
        }

        public string ControllerButtonText
        {
            get { return _controllerButtonText; }
            set
            {
                Set(ref _controllerButtonText, value);
                _configurationService.Configuration.ControllerBtn = _controllerButtonText;
            }
        }

        public string KeyboardButtonText
        {
            get { return _keyboardButtonText; }
            set
            {
                Set(ref _keyboardButtonText, value);
                _configurationService.Configuration.KeyboardBtn = _keyboardButtonText;
            }
        }

        public short Delay
        {
            get { return _delay; }
            set
            {
                Set(ref _delay, value);
                _lightService.Delay = _delay;
                _configurationService.Configuration.Delay = _delay;
            }
        }

        public short Repeats
        {
            get { return _repeats; }
            set
            {
                Set(ref _repeats, value);
                _lightService.Repeats = _repeats;
                _configurationService.Configuration.Repeats = _repeats;
            }
        }

        public RelayCommand GetDevicesCommand { get; set; }
        public RelayCommand GetButtonIdCommand{ get; set; }
        public RelayCommand ModeSelectedCommand { get; set; }
        public RelayCommand DeviceChangedCommand { get; set; }
        public RelayCommand DonateCommand { get; private set; }
        public RelayCommand ShowModesCommand { get; set; }

        #endregion

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand(InitGameControllerList);
            this.GetButtonIdCommand = new RelayCommand(InitControllerButtonAssign);
            this.ShowModesCommand = new RelayCommand(SwitchModeListVisibility);
            this.DeviceChangedCommand = new RelayCommand(ChangeDevice);
            this.ModeSelectedCommand = new RelayCommand(ChangeMode);
            this.DonateCommand = new RelayCommand(LaunchBrowserWithPayPal);
        }

        private void ChangeDevice(object obj)
        {
            StartLightService();
        }

        private void LaunchBrowserWithPayPal(object obj)
        {
            const string payPalUrl = "https://www.paypal.me/RWitanski";
            System.Diagnostics.Process.Start(payPalUrl);
        }

        private void ChangeMode(object param)
        {
            _lightService.CurrentMode = SelectedMode.ModeType;
        }

        private void HandleButtonMessage(ButtonChangedMessage buttonChangedMessage)
        {
            this.ControllerButtonText = buttonChangedMessage.WheelButtonId.ToString();
            this.IsControllerButtonEnabled = buttonChangedMessage.IsButtonSet;
        }

        private void AssignConfiguration()
        {
            Delay = _configuration.Delay;
            Repeats = _configuration.Repeats;
            ControllerButtonText = _configuration.ControllerBtn;
            KeyboardButtonText = _configuration.KeyboardBtn;
        }

        private void SwitchModeListVisibility(object param)
        {
            ModeListVisibility = ModeListVisibility.Equals("Visible", StringComparison.OrdinalIgnoreCase) ? "Hidden" : "Visible";
        }

        private void InitControllerButtonAssign(object param)
        {
            _deviceService.AssignButtonsToController(SelectedDevice, KeyboardButtonText);
        }

        private void StartLightService()
        {
            if (SelectedDevice != null && IsControllerButtonEnabled)
            {
                _deviceService.StartLightService(SelectedDevice, ControllerButtonText, KeyboardButtonText);
            }
        }

        public void InitGameControllerList(object param)
        {
            GameControllers.Clear();
            foreach (var device in _deviceService.GetDevices())
            {
                GameControllers.Add(device);
            }
            SelectedDevice = GameControllers.Any(g => g.DeviceGuid == Configuration.DeviceGuid) ?
                GameControllers.FirstOrDefault(g => g.DeviceGuid == Configuration.DeviceGuid) :
                GameControllers.FirstOrDefault();

            IsControllerButtonEnabled = GameControllers.Any();
            StartLightService();
        }
    }
}