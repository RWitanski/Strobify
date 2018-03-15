namespace Strobify.Strategies.Interfaces
{
    using WindowsInput.Native;
    public interface IKeyboardButtonMapper
    {
        VirtualKeyCode SetVirtualKeyCode(string buttonKey);
    }
}
