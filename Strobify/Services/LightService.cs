namespace Strobify.Services
{
    using SlimDX.DirectInput;
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using Strobify.Strategies.Interfaces;
    using System.Threading;
    using System.Threading.Tasks;
    using WindowsInput;
    using WindowsInput.Native;

    public class LightService : ILightService
    {
        private readonly IControllerButtonMapper _controllerButtonMapper;

        public short Delay { get; set; }
        public short Repeats { get; set; }

        public GameController GameController { get; set; }
        protected Joystick Joystick { get; private set; }        

        public LightService(IControllerButtonMapper controllerButtonMapper)
        {
            this._controllerButtonMapper = controllerButtonMapper;
        }        

        private bool IsButtonPressed(Joystick stick, short controllerButtonId)
        {
            JoystickState currState = stick.GetCurrentState();
            return currState.IsPressed(controllerButtonId);
        }

        private async Task StickHandlingLogic(Joystick stick)
        {
            await Task.Run(() =>
            {
                while (!_controllerButtonMapper.IsMapperMode)
                {
                    stick.Poll();
                    if (IsButtonPressed(stick, GameController.ControllerButton.DeviceButtonId))
                    {
                        for (short i = 0; i < Repeats; i++)
                        {
                            DoubleControllerPress();
                        }
                    }
                    while (IsButtonPressed(stick, GameController.ControllerButton.DeviceButtonId))
                    {
                        DoubleControllerPress();
                    }
                    Thread.Sleep(50);
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
            Thread.Sleep(Delay / 4);
            inputSimulator.Keyboard.KeyUp(virtualKeyCode);
            Thread.Sleep(Delay / 4);
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