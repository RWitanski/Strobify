namespace Strobify.Services
{
    using Strobify.Model;
    using GalaSoft.MvvmLight.Ioc;
    using System.Collections.Generic;
    using Strobify.Services.Interfaces;
    using Strobify.Repositories.Interfaces;
    using SlimDX.DirectInput;
    using System;
    using System.Windows.Threading;

    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepo = SimpleIoc.Default.GetInstance<IDeviceRepository>();

        protected Joystick Joystick { get; private set; }
        protected GameController GameController { get; private set; }
        
        public IEnumerable<GameController> GetDevices()
        {
            return _deviceRepo.GetDevices();
        }

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

        public int GetControllerButtonId()
        {
            return GameController.ControllerButton.DeviceButtonId;
        }
    }
}