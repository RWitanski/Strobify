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
        public ModeType CurrentMode { get; set; }
        private CancellationTokenSource Canceller { get; set; }

        public LightService(IControllerButtonMapper controllerButtonMapper)
        {
            this._controllerButtonMapper = controllerButtonMapper;
        }

        private bool IsButtonPressed()
        {
            JoystickState currState = Joystick.GetCurrentState();
            return currState.IsPressed(GameController.ControllerButton.DeviceButtonId);
        }

        private void StickHandlingLogic()
        {
            while (!_controllerButtonMapper.IsMapperMode)
            {
                Thread.Sleep(50);
                Joystick.Poll();
                if (IsButtonPressed())
                {
                    Thread.Sleep(150);
                    Joystick.Poll();
                    if (!IsButtonPressed())
                    {
                        SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode);
                        continue;
                    }
                    switch (CurrentMode)
                    {
                        case ModeType.RaceCar:
                            {
                                for (short i = 0; i < Repeats; i++)
                                {
                                    DoubleControllerPress();
                                }
                                break;
                            }
                        case ModeType.SafetyCar:
                            {
                                Thread.Sleep(350);
                                while (!IsButtonPressed())
                                {
                                    Thread.Sleep(5);
                                    Joystick.Poll();
                                    DoubleControllerPress();
                                }
                                break;
                            }
                    }
                }
            }
        }

        private void DoubleControllerPress()
        {
            SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode);
            SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode);
        }

        private void SimulateKeyPress(VirtualKeyCode virtualKeyCode)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard
                .KeyDown(virtualKeyCode)
                .Sleep(Delay / 4)
                .KeyUp(virtualKeyCode)
                .Sleep(Delay / 4);
        }

        public void SimulateLightFlashes()
        {
            Canceller?.Cancel();

            var dinput = new DirectInput();
            Joystick = new Joystick(dinput, GameController.DeviceGuid);
            Joystick.Properties.BufferSize = 128;
            Joystick.Acquire();
            Canceller = new CancellationTokenSource();
            Task.Run(() =>
            {
                using (Canceller.Token.Register(Thread.CurrentThread.Abort))
                {
                    try
                    {
                        StickHandlingLogic();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                }
            }, Canceller.Token);
        }
    }
}