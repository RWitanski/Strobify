namespace Strobify.Services
{
    using GalaSoft.MvvmLight.Ioc;
    using Strobify.Model;
    using Strobify.Repositories.Interfaces;
    using Strobify.Services.Interfaces;
    using Strobify.Strategies.Interfaces;
    using System;
    using System.Collections.Generic;

    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepo = SimpleIoc.Default.GetInstance<IDeviceRepository>();
        private readonly IButtonMapperStrategy _buttonMapperStrategy = SimpleIoc.Default.GetInstance<IButtonMapperStrategy>();
        private readonly ILightService _lightService = SimpleIoc.Default.GetInstance<ILightService>();
        private short _delay;

        public short Delay
        {
            get { return _delay; }
            set { _delay  = _lightService.Delay = value; }
        }

        private short _repeats;

        public short Repeats
        {
            get { return _repeats; }
            set { _repeats = _lightService.Repeats = value; }
        }

        public DeviceService(IDeviceRepository deviceRepo, IButtonMapperStrategy buttonMapperStrategy, ILightService lightService)
        {
            this._deviceRepo = deviceRepo;
            this._buttonMapperStrategy = buttonMapperStrategy;
            this._lightService = lightService;
        }

        public GameController GameController { get; private set; }

        public void AssignButtonsToController(GameController gameController, string keyboardButton)
        {
            GameController = gameController;
            AssignControllerButton(gameController);
            AssignKeyboardButton(gameController, keyboardButton);
        }
        public void StartLightService(GameController gameController, string assignedControllerButtonId, string assignedKeyboardButton)
        {
            _lightService.GameController = gameController;
            _lightService.GameController.ControllerButton.DeviceButtonId = Convert.ToInt16(assignedControllerButtonId);
            _lightService.GameController.ControllerButton.KeyboardKeyCode = _buttonMapperStrategy.KeyboardButtonMapper.SetVirtualKeyCode(assignedKeyboardButton);
            _lightService.StartTimer();
        }

        private void AssignControllerButton(GameController gameController)
        {
            _buttonMapperStrategy.ControllerButtonMapper.AssignControllerButtonId(gameController);

        }

        private void AssignKeyboardButton(GameController gameController, string keyboardButton)
        {
            gameController.ControllerButton.KeyboardKeyCode = _buttonMapperStrategy.KeyboardButtonMapper.SetVirtualKeyCode(keyboardButton);
        }

        public IEnumerable<GameController> GetDevices()
        {
            return _deviceRepo.GetDevices();
        }
    }
}