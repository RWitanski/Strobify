namespace Strobify.Services
{
    using SlimDX.DirectInput;
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using System;
    using System.Threading;
    using System.Windows.Threading;
    using WindowsInput;
    using WindowsInput.Native;

    public class LightService : ILightService
    {
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public short Delay { get; set; }
        public short Repeats { get; set; }
        public GameController GameController { get; set; }
        protected Joystick Joystick { get; private set; }

        private bool IsButtonPressed(Joystick stick, short controllerButtonId)
        {
            JoystickState currState = stick.GetCurrentState();
            return currState.IsPressed(controllerButtonId);
        }

        private void StickHandlingLogic(Joystick stick)
        {
            stick.Poll();
            //if (IsButtonPressed(stick, GameController.ControllerButton.DeviceButtonId))
            //{
            //    for (short i = 0; i < Repeats; i++)
            //    {
            //        SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode, Delay);
            //        SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode, Delay);
            //    }
            //}
                
            while (IsButtonPressed(stick, GameController.ControllerButton.DeviceButtonId))
            {
                for (short i = 0; i < Repeats; i++)
                {
                    SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode, Delay);
                    SimulateKeyPress(GameController.ControllerButton.KeyboardKeyCode, Delay);
                }
            }
        }

        private void SimulateKeyPress(VirtualKeyCode virtualKeyCode, short delay)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(virtualKeyCode);
            Thread.Sleep(delay / 4);
            inputSimulator.Keyboard.KeyUp(virtualKeyCode);
            Thread.Sleep(delay / 4);
        }

        public void StartTimer()
        {
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);

            var dinput = new DirectInput();
            Joystick = new Joystick(dinput, GameController.DeviceGuid);
            Joystick.Properties.BufferSize = 128;
            Joystick.Acquire();
            _dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            StickHandlingLogic(Joystick);
        }
    }
}