namespace Strobify.Model
{
    using WindowsInput.Native;

    public class ControllerButton
    {
        public short DeviceButtonId { get; set; }
        public VirtualKeyCode KeyboardKeyCode { get; set; }
    }
}