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
        private volatile bool isButtonPressed;

        public short Delay { get; set; }
        public short Repeats { get; set; }

        public GameController GameController { get; set; }
        protected Joystick Joystick { get; private set; }
        public ModeType CurrentMode { get; set; }
        private CancellationTokenSource ButtonSimulatorCanceller { get; set; }
        private CancellationTokenSource ButtonListenerCanceller { get; set; }
        private CancellationTokenSource SpecialModeCanceller { get; set; }

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
            Timer specialModeTimer = new Timer(ActivateSpecialMode, null, 350, 0);

            while (!_controllerButtonMapper.IsMapperMode)
            {
                Thread.Sleep(1);
                if (isButtonPressed)
                {
                    Thread.Sleep(50);
                    if (!isButtonPressed)
                    {
                        SingleControllerPress(null);
                    }
                }
                else
                {
                    specialModeTimer.Change(350, Timeout.Infinite);
                }
            }
        }

        private void ActivateSpecialMode(object state)
        {
            if (SpecialModeCanceller != null)
            {
                SpecialModeCanceller.Cancel();
            }
            else
            {
                SpecialModeCanceller = new CancellationTokenSource();

                Task.Run(() =>
                {
                    using (SpecialModeCanceller.Token.Register(Thread.CurrentThread.Abort))
                    {
                        try
                        {
                            switch (CurrentMode)
                            {
                                case ModeType.RaceCar:
                                    {
                                        RaceCarFlashing();
                                        SpecialModeCanceller = null;
                                        break;
                                    }
                                case ModeType.StroboRaceCar:
                                    {
                                        StroboRaceCarFlashing();
                                        SpecialModeCanceller = null;
                                        break;
                                    }
                                case ModeType.SafetyCar:
                                    {
                                        SafetyCarFlashing();
                                        break;
                                    }
                                case ModeType.StroboSafetyCar:
                                    {
                                        F1LightFlashing();
                                        break;
                                    }
                            }
                        }
                        catch (ThreadAbortException)
                        {
                            SpecialModeCanceller = null;
                        }
                    }
                }, SpecialModeCanceller.Token);
            }
        }

        private void RaceCarFlashing()
        {
            for (short i = 0; i < Repeats; i++)
            {
                DoubleControllerPress();
            }
            while (isButtonPressed)
            {
                DoubleControllerPress();
            }
        }

        private void StroboRaceCarFlashing()
        {
            for (short i = 0; i < Repeats; i++)
            {
                DoubleControllerPress();
                DoubleControllerPress();
                Thread.Sleep(Delay);
            }
            while (isButtonPressed)
            {
                DoubleControllerPress();
                DoubleControllerPress();
                Thread.Sleep(Delay);
            }
        }

        private void SafetyCarFlashing()
        {
            while (true)
            {
                DoubleControllerPress();
            }
        }

        private void F1LightFlashing()
        {
            while (true)
            {
                DoubleControllerPress();
                DoubleControllerPress();
                Thread.Sleep(Delay);
            }
        }

        private void ButtonPressListener()
        {
            while (true)
            {
                Thread.Sleep(1);
                Joystick.Poll();
                isButtonPressed = IsButtonPressed();
            }
        }

        private void DoubleControllerPress()
        {
            SingleControllerPress(null);
            SingleControllerPress(null);
        }

        private void SingleControllerPress(object state)
        {
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
            ButtonSimulatorCanceller?.Cancel();
            ButtonListenerCanceller?.Cancel();

            Joystick = new Joystick(new DirectInput(), GameController.DeviceGuid);
            Joystick.Properties.BufferSize = 128;
            Joystick.Acquire();
            ButtonListenerCanceller = new CancellationTokenSource();
            Task.Run(() =>
            {
                using (ButtonListenerCanceller.Token.Register(Thread.CurrentThread.Abort))
                {
                    try
                    {
                        ButtonPressListener();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                }
            }, ButtonListenerCanceller.Token);

            ButtonSimulatorCanceller = new CancellationTokenSource();
            Task.Run(() =>
            {
                using (ButtonSimulatorCanceller.Token.Register(Thread.CurrentThread.Abort))
                {
                    try
                    {
                        StickHandlingLogic();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                }
            }, ButtonSimulatorCanceller.Token);
        }
    }
}