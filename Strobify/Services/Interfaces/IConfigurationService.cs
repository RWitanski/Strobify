namespace Strobify.Services.Interfaces
{
    using Strobify.Model;

    public interface IConfigurationService
    {
        void SaveConfiguration();
        Configuration ReadConfiguration();
        Configuration Configuration { get; set; }
    }
}