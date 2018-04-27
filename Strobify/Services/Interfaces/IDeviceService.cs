namespace Strobify.Services.Interfaces
{
    using Strobify.Model;
    using System.Collections.Generic;

    public interface IDeviceService
    {
        IEnumerable<GameController> GetDevices();
        void AssignButtonsToController(GameController gameController, string keyboardButton);       
        void StartLightService(GameController gameController, string assignedControllerButtonId, string assignedKeyboardButton);
    }
}