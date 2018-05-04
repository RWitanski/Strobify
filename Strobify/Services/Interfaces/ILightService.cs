namespace Strobify.Services.Interfaces
{
    using Strobify.Model;
    public interface ILightService
    {
        void SimulateLightFlashes(short delay, short repeats);
        GameController GameController { get; set; }
    }
}