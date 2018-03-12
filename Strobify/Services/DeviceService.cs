namespace Strobify.Services
{
    using GalaSoft.MvvmLight.Ioc;
    using Strobify.Model;
    using Strobify.Repositories.Interfaces;
    using Strobify.Services.Interfaces;
    using Strobify.Strategies.Interfaces;
    using System.Collections.Generic;

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