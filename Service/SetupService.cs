using Newtonsoft.Json;
using PvPGNAccountManagement.Interface;
using PvPGNAccountManagement.ViewModels;
using System.IO;

namespace PvPGNAccountManagement.Service
{
    public class SetupService : ISetupService
    {
        string pathJsonConfig;

        public SetupService(string pathAppBaseDirectory)
        {
            this.pathJsonConfig = $"{pathAppBaseDirectory}config.json";
        }

        public SetupViewModel? Get(MainWindowViewModel mainWindow)
        {
            if (!File.Exists(pathJsonConfig))
            {
                return null;
            }

            var jsonText = File.ReadAllText(pathJsonConfig);
            var setup = JsonConvert.DeserializeObject<SetupViewModel>(jsonText);

            return new SetupViewModel(mainWindow, this)
            {
                PathUsers = setup.PathUsers,
                PathCharInfo = setup.PathCharInfo,
            };
        }

        public void Set(SetupViewModel setup)
        {
            var jsonText = JsonConvert.SerializeObject(setup);
            File.WriteAllText(pathJsonConfig, jsonText);
        }
    }
}
