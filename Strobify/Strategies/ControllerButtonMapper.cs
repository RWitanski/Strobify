namespace Strobify.Strategies
{
    using System;
    using Strobify.Model;
    using System.Threading;
    using SlimDX.DirectInput;
    using Strategies.Interfaces;
    using System.Windows.Threading;
    using GalaSoft.MvvmLight.Messaging;
    using Strobify.Messages;

    public class ControllerButtonMapper : IControllerButtonMapper
    {
        private readonly IMessenger _messenger;
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        public ControllerButtonMapper(IMessenger messenger)
        {
            this._messenger = messenger;
        }

        public Joystick Joystick { get; private set; }
        public GameController GameController { get; private set; }

        public Boolean IsButtonSet { get; set; } = true;
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
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);
            _dispatcherTimer.Start();
            IsButtonSet = false;
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
                    Thread.Sleep(2000);
                    _dispatcherTimer.Stop();
                    _messenger.Send(new ButtonChangedMessage
                    {
                        WheelButtonId = buttonId
                    });
                IsButtonSet = true;
                    break;
                }
                buttonId++;
            }
        }
    }
}