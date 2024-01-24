using Launcher.Share.Commands;
using Newtonsoft.Json;
using PvPGNAccountManagement.Helper;
using PvPGNAccountManagement.Interface;
using System.IO;
using System.Windows.Input;

namespace PvPGNAccountManagement.ViewModels
{
    public class SetupViewModel : ViewModel
    {
        public SetupViewModel(MainWindowViewModel mainWindow, ISetupService setupService)
        {
            SaveCommand = new Command(() =>
                {
                    if (!Directory.Exists(PathCharInfo) || !Directory.Exists(PathUsers))
                    {
                        return;
                    }

                    setupService.Set(this);
                    mainWindow.NavigateCommand.Execute(MainWindowViewModel.Views.AccountManagement);
                }
            );

            SearchPathCharInfoCommand = new Command(() =>
            {
                PathCharInfo = FolderBrowserDialogHelper.GetFolder();
            });

            SearchPathUsersCommand = new Command(() =>
            {
                PathUsers = FolderBrowserDialogHelper.GetFolder();
            });
        }

        private string pathCharInfo;
		public string PathCharInfo
        {
			get { return pathCharInfo; }
            set
            {
                pathCharInfo = value;
                PropertyChangedInvoke(nameof(PathCharInfo));
            }
        }

        private string pathUsers;
        public string PathUsers
        {
            get { return pathUsers; }
            set
            {
                pathUsers = value;
                PropertyChangedInvoke(nameof(PathUsers));
            }
        }
        
        [JsonIgnore]
        public ICommand SaveCommand { get; private set; }
        [JsonIgnore]
        public ICommand SearchPathCharInfoCommand { get; private set; }
        [JsonIgnore]
        public ICommand SearchPathUsersCommand { get; private set; }
    }
}
