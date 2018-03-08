using System.Collections.Generic;
using Strobify.Model;

namespace Strobify.Services.Interfaces
{
    public interface IDeviceService
    {
        IEnumerable<GameController> GetDevices();
    }
}
