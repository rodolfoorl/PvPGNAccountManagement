using PvPGNAccountManagement.ViewModels;

namespace PvPGNAccountManagement.Interface
{
    public interface ISetupService
    {
        SetupViewModel? Get(MainWindowViewModel mainWindow);
        void Set(SetupViewModel setup);
    }
}