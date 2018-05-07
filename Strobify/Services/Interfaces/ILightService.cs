namespace Strobify.Services.Interfaces
{
    using Strobify.Model;

    public interface ILightService
    {
        void SimulateLightFlashes();
        GameController GameController { get; set; }
        short Delay { get; set; }
        short Repeats { get; set; }
    }
}