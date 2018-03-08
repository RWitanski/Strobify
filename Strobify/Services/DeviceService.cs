namespace Strobify.Services
{
    using Strobify.Model;
    using GalaSoft.MvvmLight.Ioc;
    using System.Collections.Generic;
    using Strobify.Services.Interfaces;
    using Strobify.Repositories.Interfaces;

    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepo = SimpleIoc.Default.GetInstance<IDeviceRepository>();

        public IEnumerable<GameController> GetDevices()
        {
            return _deviceRepo.GetDevices();
        }
    }
}