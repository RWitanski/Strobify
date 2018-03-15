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
            var dinput = new DirectInput();
            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                var controllerButton = new ControllerButton();
                var dev = new GameController(controllerButton)
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