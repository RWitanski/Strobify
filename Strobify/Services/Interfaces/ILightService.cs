namespace Strobify.Services.Interfaces
{
    using Strobify.Model;
    using System.Threading.Tasks;

    public interface ILightService
    {
        void SimulateLightFlashes();
        GameController GameController { get; set; }
        short Delay { get; set; }
        short Repeats { get; set; }
        ModeType CurrentMode { get; set; }
    }
}