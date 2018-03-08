using System.Collections.Generic;
using SlimDX.DirectInput;
using Strobify.Model;
using Strobify.Services.Interfaces;

namespace Strobify.Services
{
    public class DeviceService : IDeviceService
    {
        public IEnumerable<GameController> GetDevices()
        {
            IList<GameController> availableDevicesList = new List<GameController>();
            DirectInput dinput = new DirectInput();
            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.All, DeviceEnumerationFlags.AttachedOnly))
            {
                GameController dev = new GameController
                {
                    DeviceGuid = di.InstanceGuid,
                    Name = di.InstanceName
                };
                availableDevicesList.Add(dev);
            }
            return availableDevicesList;
        }
    }
}