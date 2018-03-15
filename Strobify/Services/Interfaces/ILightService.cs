namespace Strobify.Services.Interfaces
{
    using Strobify.Model;
    public interface ILightService
    {
        void StartTimer();
        GameController GameController { get; set; }
        short Delay { get; set; }
        short Repeats { get; set; }
    }
}