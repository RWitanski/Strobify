namespace Strobify.Strategies
{    
    using Interfaces;
    using GalaSoft.MvvmLight.Ioc;

    public class ButtonMapperStrategy : IButtonMapperStrategy
    {
        private readonly ControllerButtonMapper _controllerButtonMapper;
        private readonly KeyboardButtonMapper _keyboardButtonMapper;
        public ButtonMapperStrategy()
        {
            _controllerButtonMapper = new ControllerButtonMapper();
            _keyboardButtonMapper = new KeyboardButtonMapper();
        }        
        public ControllerButtonMapper ControllerButtonMapper { get; private set; }        
        public KeyboardButtonMapper KeyboardButtonMapper { get; private set; }

        public readonly IControllerButtonMapper controllerButtonMapper = SimpleIoc.Default.GetInstance<IControllerButtonMapper>();
        public readonly IKeyboardButtonMapper keyboardButtonMapper = SimpleIoc.Default.GetInstance<IKeyboardButtonMapper>();

    }
}