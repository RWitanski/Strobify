namespace Strobify.Model
{
    using System;

    public class Configuration
    {
        public Guid DeviceGuid { get; set; }
        public string ControllerBtn { get; set; }
        public string KeyboardBtn { get; set; }
        public short Delay { get; set; }
        public short Repeats { get; set; }
    }
}