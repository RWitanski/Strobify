namespace Strobify.ViewModel
{
    using System.Linq;
    using Strobify.Model;
    using Strobify.Helpers;
    using GalaSoft.MvvmLight;    
    using Strobify.Services.Interfaces;
    using Strobify.Strategies.Interfaces;
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight.Messaging;
    using Strobify.Messages;

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
            InitGameControllerList();
            StartLightService();
        }
        
        private GameController _selectedDevice;
        private string _controllerButtonText = "7";
        private string _keyboardButtonText = "L";
        private short _delay = 150;
        private short _repeats = 12;

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

                public RelayCommand GetDevicesCommand { get; private set; }
                private RelayCommand _getButtonIdCommand;
                public RelayCommand GetButtonIdCommand
                {
                    get { return _getButtonIdCommand; }
                    private set
                    {
                        _getButtonIdCommand = value;
                        RaisePropertyChanged();
                    }
                }

                public RelayCommand StartCommand { get; private set; }

                #endregion

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand((parameter) => InitGameControllerList()
            );
            this.GetButtonIdCommand = new RelayCommand((parameter) => InitControllerButtonAssign(),
                (parameter) => _buttonMapperStrategy.ControllerButtonMapper.IsButtonSet
            );
            this.StartCommand = new RelayCommand((parameter) => StartLightService(), (parameter) => true
            );
        }

        private void HandleMessage(ButtonChangedMessage buttonChangedMessage)
        {
            this.ControllerButtonText = buttonChangedMessage.WheelButtonId.ToString();
        }

        public void InitGameControllerList()
        {
            GameControllers.Clear();
            foreach (var device in _deviceService.GetDevices())
            {
                GameControllers.Add(device);
            }
            SelectedDevice = GameControllers.FirstOrDefault();
            _buttonMapperStrategy.ControllerButtonMapper.IsButtonSet =  GameControllers.Any();
            StartLightService();
        }

        private void InitControllerButtonAssign()
        {
            _deviceService.AssignButtonsToController(SelectedDevice, KeyboardButtonText);
        }

        private void StartLightService()
        {
            if (SelectedDevice != null && _buttonMapperStrategy.ControllerButtonMapper.IsButtonSet)
            {
                _deviceService.StartLightService(SelectedDevice, ControllerButtonText, KeyboardButtonText, Delay, Repeats);
            }
        }
    }
}