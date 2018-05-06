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
        private Boolean _isButtonSet = false;

        public ControllerButtonMapper(IMessenger messenger)
        {
            this._messenger = messenger;
        }

        public Joystick Joystick { get; private set; }
        public GameController GameController { get; private set; }

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
            

            _messenger.Send(new ButtonChangedMessage
            {
                WheelButtonId = GameController.ControllerButton.DeviceButtonId,
                IsButtonSet = false
            });

            await Task.Run(() =>
            {
                while (!_isButtonSet)
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
                                WheelButtonId = buttonId,
                                IsButtonSet = true
                            });
                            _isButtonSet = true;
                            break;
                        }
                        buttonId++;
                    }
                }
            }).ConfigureAwait(true);
        }
    }
}