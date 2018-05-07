using Strobify.Services.Interfaces;
using System;
using System.IO;
using System.Xml.Linq;

namespace Strobify.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly string _cfgFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private const string _cfgFolderName = "Strobify";
        private const string _cfgFileName = "StrobifyConfig.xml";

        private void CreateAppFolder()
        {
            var cfgFullFolderName = Path.Combine(_cfgFolderPath, _cfgFolderName);
            if (!Directory.Exists(cfgFullFolderName))
            {
                Directory.CreateDirectory(cfgFullFolderName);
            }
        }

        private void CreateConfigFile()
        {
            XDocument document = new XDocument(
                new XElement("Configuration",
                    new XElement("Time",
                        new XAttribute("delay", "100"),
                        new XAttribute("repeats", "20")),
                    new XElement("Mappings",
                        new XAttribute("controllerBtn", "12"),
                        new XAttribute("key", "H")),
                    new XElement("Info",
                        new XElement("Date",
                            new XAttribute("modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        )
                    )
                )
            );

            var cfgFullFileName = Path.Combine(_cfgFolderPath, _cfgFolderName, _cfgFileName);
            document.Save(cfgFullFileName);
        }

        public void SaveConfiguration()
        {
            CreateAppFolder();
            CreateConfigFile();
        }
    }
}
