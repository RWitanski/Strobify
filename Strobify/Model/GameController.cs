namespace Strobify.Model
{
    using System;
    public class GameController
    {
        public GameController(ControllerButton controllerButton)
        {
            this.ControllerButton = controllerButton;
        }
        public string Name { get; set; }
        public Guid DeviceGuid { get; set; }
        public ControllerButton ControllerButton { get; set; }
    }
}