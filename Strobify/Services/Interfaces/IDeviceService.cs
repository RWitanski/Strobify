namespace Strobify.Services.Interfaces
{    
    using Strobify.Model;
    using System.Collections.Generic;

    public interface IDeviceService
    {
        IEnumerable<GameController> GetDevices();
        void AssignControllerButtonId(GameController gameController);
        int ControllerButtonId { get; }
    }
}