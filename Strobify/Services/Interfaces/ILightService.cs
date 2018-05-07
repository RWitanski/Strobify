namespace Strobify.Services.Interfaces
{
    using Strobify.Model;
    using Strobify.Strategies.Interfaces;

    public interface ILightService
    {
        void SimulateLightFlashes(short delay, short repeats);
        GameController GameController { get; set; }
    }
}