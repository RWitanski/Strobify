namespace Strobify.Strategies
{
    using System;
    using Strobify.Model;    
    using SlimDX.DirectInput;
    using Strategies.Interfaces;
    using System.Windows.Threading;

    public class ControllerButtonMapper : IControllerButtonMapper
    {
        protected Joystick Joystick { get; private set; }
        protected GameController GameController { get; private set; }
        public int ControllerButtonId => GameController.ControllerButton.DeviceButtonId;



        public void AssignControllerButtonId(GameController gameController)
        {
            GameController = gameController;
            DirectInput directInput = new DirectInput();
            Joystick = new Joystick(directInput, gameController.DeviceGuid);
            Joystick.Properties.BufferSize = 128;
            Joystick.Acquire();

            StartTimer();
        }
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private void StartTimer()
        {
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Joystick.Poll();
            JoystickState currState = Joystick.GetCurrentState();
            int buttonId = 0;
            foreach (var buttonState in currState.GetButtons())
            {
                if (buttonState)
                {
                    GameController.ControllerButton.DeviceButtonId = buttonId;
                    dispatcherTimer.Stop();
                }
                buttonId++;
            }
        }
    }
}
