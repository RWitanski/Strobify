namespace Strobify.Services
{
    using Strobify.Model;
    using GalaSoft.MvvmLight.Ioc;
    using System.Collections.Generic;
    using Strobify.Services.Interfaces;
    using Strobify.Repositories.Interfaces;
    using Strobify.Strategies;
    using Strobify.Strategies.Interfaces;

    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepo = SimpleIoc.Default.GetInstance<IDeviceRepository>();
        private readonly IButtonMapperStrategy _buttonMapperStrategy = SimpleIoc.Default.GetInstance<IButtonMapperStrategy>();

        public GameController GameController { get; private set; }

        public void AssignControllerButtonId(GameController gameController)
        {
            GameController = gameController;
            _buttonMapperStrategy.ControllerButtonMapper.AssignControllerButtonId(gameController);
        }

        public IEnumerable<GameController> GetDevices()
        {
            return _deviceRepo.GetDevices();
        }              
    }
}