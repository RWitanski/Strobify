namespace Strobify.Repositories.Interfaces
{
    using Strobify.Model;
    using System.Collections.Generic;

    public interface IDeviceRepository
    {
        IEnumerable<GameController> GetDevices();
    }
}