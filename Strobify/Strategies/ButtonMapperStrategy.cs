namespace Strobify.Strategies
{
    using GalaSoft.MvvmLight.Messaging;
    using Interfaces;

    public class ButtonMapperStrategy : IButtonMapperStrategy
    {
        private readonly IMessenger _messenger;

        private readonly IControllerButtonMapper _controllerButtonMapper;
        private readonly IKeyboardButtonMapper _keyboardButtonMapper;
        public ButtonMapperStrategy(IMessenger messenger, IControllerButtonMapper controllerButtonMapper, IKeyboardButtonMapper keyboardButtonMapper)
        {
            this._messenger = messenger;
            this._controllerButtonMapper = controllerButtonMapper;
            this._keyboardButtonMapper = keyboardButtonMapper;
            //ControllerButtonMapper = new ControllerButtonMapper(messenger);
            //KeyboardButtonMapper = new KeyboardButtonMapper();
        }

        //public ControllerButtonMapper ControllerButtonMapper { get; }
        //public KeyboardButtonMapper KeyboardButtonMapper { get; }

        public void MapKeyboardButton()
        {

        }

        public void MapControllerButton()
        {

        }
    }
}