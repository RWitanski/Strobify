namespace Strobify.Services
{
    using Strobify.Model;
    using Strobify.Services.Interfaces;
    using System;
    using System.IO;
    using System.Xml.Linq;

    public class ConfigurationService : IConfigurationService
    {
        private readonly string _cfgFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private const string _cfgFolderName = "Strobify";
        private const string _cfgFileName = "StrobifyConfig.xml";

        public Configuration Configuration { get; set; }

        public ConfigurationService()
        {
            Configuration = new Configuration();
        }

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
                        new XAttribute("delay", Configuration.Delay.ToString()),
                        new XAttribute("repeats", Configuration.Repeats.ToString())),
                    new XElement("Mappings",
                        new XAttribute("controllerBtn", Configuration.ControllerBtn),
                        new XAttribute("keyboardBtn", Configuration.KeyboardBtn.ToUpper())),
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

        private Configuration ReadConfigFile()
        {
            var cfgFullFileName = Path.Combine(_cfgFolderPath, _cfgFolderName, _cfgFileName);
            if (File.Exists(cfgFullFileName))
            {
                XDocument document = XDocument.Load(cfgFullFileName);
                Configuration.Delay = Convert.ToInt16(document.Root.Element("Time").Attribute("delay").Value);
                Configuration.Repeats = Convert.ToInt16(document.Root.Element("Time").Attribute("repeats").Value);
                Configuration.ControllerBtn = document.Root.Element("Mappings").Attribute("controllerBtn").Value;
                Configuration.KeyboardBtn = document.Root.Element("Mappings").Attribute("keyboardBtn").Value;

                return Configuration;
            }
            return new Configuration { Delay = 250, Repeats = 12, ControllerBtn = "7", KeyboardBtn = "L" };
        }

        public void SaveConfiguration()
        {
            CreateAppFolder();
            CreateConfigFile();
        }

        public Configuration ReadConfiguration()
        {
            return ReadConfigFile();
        }
    }
}
