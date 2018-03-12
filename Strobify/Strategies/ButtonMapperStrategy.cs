namespace Strobify.Strategies
{
    using Interfaces;

    public class ButtonMapperStrategy : IButtonMapperStrategy
    {
        public ButtonMapperStrategy()
        {
            ControllerButtonMapper = new ControllerButtonMapper();
            KeyboardButtonMapper = new KeyboardButtonMapper();
        }
        
        public ControllerButtonMapper ControllerButtonMapper { get; }
        public KeyboardButtonMapper KeyboardButtonMapper { get; }
    }
}