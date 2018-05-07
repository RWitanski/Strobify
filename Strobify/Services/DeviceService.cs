namespace Strobify.Services
{
    using Strobify.Model;
    using Strobify.Repositories.Interfaces;
    using Strobify.Services.Interfaces;
    using Strobify.Strategies.Interfaces;
    using System;
    using System.Collections.Generic;

    public class DeviceService : IDeviceService
    {
        public DeviceService(IDeviceRepository deviceRepo, IButtonMapperStrategy buttonMapperStrategy, ILightService lightService)
        {
            this._deviceRepo = deviceRepo;
            this._buttonMapperStrategy = buttonMapperStrategy;
            this._lightService = lightService;
        }

        private readonly IDeviceRepository _deviceRepo;
        private readonly IButtonMapperStrategy _buttonMapperStrategy;
        private readonly ILightService _lightService;

        private void AssignControllerButton(GameController gameController)
        {
            _buttonMapperStrategy.ControllerButtonMapper.AssignControllerButtonId(gameController);
        }

        public void AssignButtonsToController(GameController gameController, string keyboardButton)
        {
            AssignControllerButton(gameController);
            AssignKeyboardButton(gameController, keyboardButton);
        }

        public void StartLightService(GameController gameController, string assignedControllerButtonId, string assignedKeyboardButton)
        {
            _lightService.GameController = gameController;
            _lightService.GameController.ControllerButton.DeviceButtonId = Convert.ToInt16(assignedControllerButtonId);
            _lightService.GameController.ControllerButton.KeyboardKeyCode = _buttonMapperStrategy.KeyboardButtonMapper.SetVirtualKeyCode(assignedKeyboardButton);
            _lightService.SimulateLightFlashes();
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