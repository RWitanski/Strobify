namespace Strobify.Strategies
{
    using System;
    using Strobify.Model;
    using SlimDX.DirectInput;
    using Strategies.Interfaces;
    using System.Windows.Threading;

    public class ControllerButtonMapper : IControllerButtonMapper
    {
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public Joystick Joystick { get; private set; }
        public GameController GameController { get; private set; }

        public void AssignControllerButtonId(GameController gameController)
        {
            GameController = gameController;
            var directInput = new DirectInput();
            try
            {
                Joystick = new Joystick(directInput, gameController.DeviceGuid);
            }
            catch (Exception e)
            {
                throw;
            }      
            Joystick.Properties.BufferSize = 128;
            Joystick.Acquire();

            StartTimer();
        }

        private void StartTimer()
        {
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            _dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Joystick.Poll();
            JoystickState currState = Joystick.GetCurrentState();
            short buttonId = 0;
            foreach (var buttonState in currState.GetButtons())
            {
                if (buttonState)
                {
                    GameController.ControllerButton.DeviceButtonId = buttonId;
                    _dispatcherTimer.Stop();
                }
                buttonId++;
            }
        }
    }
}