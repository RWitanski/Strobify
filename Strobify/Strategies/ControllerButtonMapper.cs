namespace Strobify.Strategies
{
    using GalaSoft.MvvmLight.Messaging;
    using SlimDX.DirectInput;
    using Strategies.Interfaces;
    using Strobify.Messages;
    using Strobify.Model;
    using System;
    using System.Threading.Tasks;

    public class ControllerButtonMapper : IControllerButtonMapper
    {
        private readonly IMessenger _messenger;
        public ControllerButtonMapper(IMessenger messenger)
        {
            this._messenger = messenger;
        }

        public Joystick Joystick { get; private set; }
        public GameController GameController { get; private set; }
        public Boolean IsButtonSet { get; set; } = true;

        public async void AssignControllerButtonId(GameController gameController)
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
            await WaitForControllerButtonPress();
        }

        private async Task WaitForControllerButtonPress()
        {
            IsButtonSet = false;
            await Task.Factory.StartNew(() =>
            {
                while (!IsButtonSet)
                {
                    Joystick.Poll();
                    JoystickState currState = Joystick.GetCurrentState();
                    short buttonId = 0;
                    foreach (var buttonPressed in currState.GetButtons())
                    {
                        if (buttonPressed)
                        {
                            GameController.ControllerButton.DeviceButtonId = buttonId;
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
            }).ConfigureAwait(true);
        }
    }
}