namespace Strobify.Messages
{
    public class ButtonChangedMessage
    {
        public short WheelButtonId { get; set; }
        public bool IsButtonSet { get; set; }
    }
}
