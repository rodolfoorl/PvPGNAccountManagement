using Launcher.Share.Commands;
using PvPGNAccountManagement.Interface;
using PvPGNAccountManagement.Service;
using PvPGNAccountManagement.Views;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace PvPGNAccountManagement.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        static ISetupService setupService;

        public MainWindowViewModel()
        {
            setupService = new SetupService(AppContext.BaseDirectory);

            var setup = setupService.Get(this);

            if(setup == null)
            {
                setup = new SetupViewModel(this, setupService);
                View = new Setup(setup);
            }
            else
            {
                var accountManagement = new AccountManagementViewModel(this, setup);
                View = new AccountManagement(accountManagement);
            }

            NavigateCommand = new Command<object>((view) =>
            {
                var setupViewModel = setupService.Get(this);

                switch (view)
                {
                    case Views.AccountManagement:
                        var accountManagement = new AccountManagementViewModel(this, setupViewModel);
                        View = new AccountManagement(accountManagement);
                        break;
                    case Views.Setup:
                        View = new Setup(setupViewModel);
                        break;
                }
            });
        }

        UserControl view;
        public UserControl View
        {
            get { return view; }
            set
            {
                view = value;
                PropertyChangedInvoke(nameof(View));
            }
        }

        public ICommand NavigateCommand { get; private set; }

        public enum Views
        {
            AccountManagement,
            Setup
        }
    }
}