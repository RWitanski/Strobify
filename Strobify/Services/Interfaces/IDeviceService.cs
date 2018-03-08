namespace Strobify.Services.Interfaces
{    
    using Strobify.Model;
    using System.Collections.Generic;

    public interface IDeviceService
    {
        IEnumerable<GameController> GetDevices();
    }
}
