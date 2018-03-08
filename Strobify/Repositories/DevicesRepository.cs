namespace Strobify.Repositories
{
    using Strobify.Model;
    using SlimDX.DirectInput;     
    using System.Collections.Generic;
    using Strobify.Repositories.Interfaces;

    public class DeviceRepository : IDeviceRepository
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