namespace Strobify.Strategies.Interfaces
{
    public interface IButtonMapperStrategy
    {
        ControllerButtonMapper ControllerButtonMapper { get; }
        KeyboardButtonMapper KeyboardButtonMapper { get; }
    }
}
