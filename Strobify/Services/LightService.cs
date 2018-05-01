namespace Strobify.Services
{
    using SlimDX.DirectInput;
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using System.Threading;
    using System.Threading.Tasks;
    using WindowsInput;
    using WindowsInput.Native;

    public class LightService : ILightService
    {

        public short Delay { get; set; }
        public short Repeats { get; set; }
        public GameController GameController { get; set; }
        protected Joystick Joystick { get; private set; }

        private bool IsButtonPressed(Joystick stick, short controllerButtonId)
        {
            JoystickState currState = stick.GetCurrentState();
            return currState.IsPressed(controllerButtonId);
        }

        private async Task StickHandlingLogic(Joystick stick)
        {
            await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    stick.Poll();
                    while (IsButtonPressed(stick, GameController.ControllerButton.DeviceButtonId))
                    {
                        for (short i = 0; i < Repeats; i++)
                        {
                            SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode, Delay);
                            SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode, Delay);
                        }
                    }
                }
            });
        }

        private void SimulateKeyPress(VirtualKeyCode virtualKeyCode, short delay)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(virtualKeyCode);
            Thread.Sleep(delay / 4);
            inputSimulator.Keyboard.KeyUp(virtualKeyCode);
            Thread.Sleep(delay / 4);
        }

        public async void SimulateLightFlashes()
        {         
            var dinput = new DirectInput();
            Joystick = new Joystick(dinput, GameController.DeviceGuid);
            Joystick.Properties.BufferSize = 128;
            Joystick.Acquire();
            await StickHandlingLogic(Joystick);
        }
    }
}