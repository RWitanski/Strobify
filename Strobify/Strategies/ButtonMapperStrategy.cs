namespace Strobify.Strategies
{
    using GalaSoft.MvvmLight.Messaging;
    using Interfaces;

    public class ButtonMapperStrategy : IButtonMapperStrategy
    {
        private readonly IMessenger _messenger;
        public ButtonMapperStrategy(IMessenger messenger)
        {
            _messenger = messenger;
            ControllerButtonMapper = new ControllerButtonMapper(messenger);
            KeyboardButtonMapper = new KeyboardButtonMapper();
        }
        
        public ControllerButtonMapper ControllerButtonMapper { get; }
        public KeyboardButtonMapper KeyboardButtonMapper { get; }
    }
}