namespace Strobify.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;
    using Strobify.Helpers;
    using Strobify.Messages;
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using Strobify.Strategies.Interfaces;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; } = new ObservableCollection<GameController>();
        private readonly IDeviceService _deviceService;
        private readonly ILightService _lightService;
        private readonly IButtonMapperStrategy _buttonMapperStrategy;
        private readonly IMessenger _messenger;

        public GameControllerViewModel(IDeviceService deviceService, ILightService lightService, IButtonMapperStrategy buttonMapperStrategy, IMessenger messenger)
        {
            this._deviceService = deviceService;
            this._lightService = lightService;
            this._buttonMapperStrategy = buttonMapperStrategy;
            this._messenger = messenger;
            _messenger.Register<ButtonChangedMessage>(this, this.HandleMessage);
            InitCommands();
            InitGameControllerList(null);
            StartLightService(null);
        }
        
        private GameController _selectedDevice;
        private string _controllerButtonText = "7";
        private string _keyboardButtonText = "L";
        private short _delay = 150;
        private short _repeats = 12;
        private bool _isControllerButtonEnabled = true;

        #region Properties

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

        public string ControllerButtonText
        {
            get { return _controllerButtonText; }
            set
            {
                Set(ref _controllerButtonText, value);
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

        public string KeyboardButtonText
        {
            get { return _keyboardButtonText; }
            set
            {
                Set(ref _keyboardButtonText, value);
            }
        }

        public short Delay
        {
            get { return _delay; }
            set { Set(ref _delay, value); }
        }
    
        public short Repeats
        {
            get { return _repeats; }
            set { Set(ref _repeats, value); }
        }

        public RelayCommand GetDevicesCommand { get; set; }

        public RelayCommand GetButtonIdCommand{ get; set; }

        public RelayCommand StartCommand { get; set; }

        #endregion

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand(InitGameControllerList);
            this.GetButtonIdCommand = new RelayCommand(InitControllerButtonAssign);
            this.StartCommand = new RelayCommand(StartLightService);
        }

        private void HandleMessage(ButtonChangedMessage buttonChangedMessage)
        {
            this.ControllerButtonText = buttonChangedMessage.WheelButtonId.ToString();
            this.IsControllerButtonEnabled = buttonChangedMessage.IsButtonSet;
        }

        public void InitGameControllerList(object param)
        {
            GameControllers.Clear();
            foreach (var device in _deviceService.GetDevices())
            {
                GameControllers.Add(device);
            }
            SelectedDevice = GameControllers.FirstOrDefault();
            IsControllerButtonEnabled = GameControllers.Any();
            StartLightService(null);
        }

        private void InitControllerButtonAssign(object param)
        {
            _deviceService.AssignButtonsToController(SelectedDevice, KeyboardButtonText);
        }

        private void StartLightService(object param)
        {
            if (SelectedDevice != null && IsControllerButtonEnabled)
            {
                _deviceService.StartLightService(SelectedDevice, ControllerButtonText, KeyboardButtonText, Delay, Repeats);
            }
        }
    }
}