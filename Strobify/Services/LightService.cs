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
        private short _delay;
        private short _repeats;

        public GameController GameController { get; set; }
        protected Joystick Joystick { get; private set; }

        private bool IsButtonPressed(Joystick stick, short controllerButtonId)
        {
            JoystickState currState = stick.GetCurrentState();
            return currState.IsPressed(controllerButtonId);
        }

        private async Task StickHandlingLogic(Joystick stick)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    stick.Poll();
                    if (IsButtonPressed(stick, GameController.ControllerButton.DeviceButtonId))
                    {
                        for (short i = 0; i < _repeats; i++)
                        {
                            DoubleControllerPress();
                        }
                    }
                    while (IsButtonPressed(stick, GameController.ControllerButton.DeviceButtonId))
                    {
                        DoubleControllerPress();
                    }
                }
            }).ConfigureAwait(false);
        }

        private void DoubleControllerPress()
        {
            SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode);
            SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode);
        }

        private void SimulateKeyPress(VirtualKeyCode virtualKeyCode)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(virtualKeyCode);
            Thread.Sleep(_delay / 4);
            inputSimulator.Keyboard.KeyUp(virtualKeyCode);
            Thread.Sleep(_delay / 4);
        }

        public async void SimulateLightFlashes(short delay, short repeats)
        {
            _delay = delay;
            _repeats = repeats;
            var dinput = new DirectInput();
            Joystick = new Joystick(dinput, GameController.DeviceGuid);
            Joystick.Properties.BufferSize = 128;
            Joystick.Acquire();
            await StickHandlingLogic(Joystick);
        }
    }
}