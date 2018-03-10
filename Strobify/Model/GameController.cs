namespace Strobify.Model
{
    using System;
    public class GameController
    {
        public string Name { get; set; }
        public Guid DeviceGuid { get; set; }
        public ControllerButton ControllerButton = new ControllerButton();
    }
}