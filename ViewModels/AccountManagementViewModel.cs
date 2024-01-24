using Launcher.Share.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PvPGNAccountManagement.ViewModels
{
    public class AccountManagementViewModel : ViewModel
    {
        public AccountManagementViewModel(MainWindowViewModel mainWindow, SetupViewModel setup)
        {
            AccountsCommand = new Command(() =>
            {
                AccountsTableVisibility = Visibility.Visible;
                CharactersTableVisibility = Visibility.Collapsed;
            });

            CharactersCommand = new Command(() =>
            {
                AccountsTableVisibility = Visibility.Collapsed;
                CharactersTableVisibility = Visibility.Visible;
            });

            SetupCommand = new Command(() =>
            {
                mainWindow.NavigateCommand.Execute(MainWindowViewModel.Views.Setup);
            });

            SearchCommand = new AsyncCommand<string>(async (search) =>
            {
                await GetData(setup, search);
            });

            GetData(setup, string.Empty).ConfigureAwait(false);
        }

        public Task GetData(SetupViewModel setup, string search)
        {
            return Task.Run(() =>
            {
                TableLoadingVisibility = Visibility.Visible;

                if (accountsTemp == null)
                {

                    var accountFiles = Directory.GetFiles(setup.PathUsers).ToList();

                    accountsTemp = accountFiles
                        .Select(x => new AccountViewModel(x))
                        .Where(x => x.Id > 0)
                        .OrderBy(x => x.Id)
                        .ToList();

                    accountsTemp.ForEach(x =>
                    {
                        var pathCharInfo = $"{setup.PathCharInfo}\\{x.Account!.ToLower()}";
                        x.Characters = new ObservableCollection<CharacterViewModel>();

                        if (Directory.Exists(pathCharInfo))
                        {
                            var charFiles = Directory.GetFiles(pathCharInfo).ToList();

                            if (charFiles.Count > 0)
                            {
                                var characters = charFiles
                                    .Select(x => new CharacterViewModel(x))
                                    .ToList();

                                x.Characters = new ObservableCollection<CharacterViewModel>(characters);
                            }
                        }
                    });

                    charactersTemp = accountsTemp.SelectMany(x => x.Characters).ToList();
                }

                var ignoreCase = StringComparison.InvariantCultureIgnoreCase;

                var characters = charactersTemp
                    .Where(x =>
                        x.Name.ToString().Contains(search, ignoreCase) ||
                        x.Account.Contains(search, ignoreCase) ||
                        x.Ladder.Contains(search, ignoreCase) ||
                        x.Expansion.ToString().Contains(search, ignoreCase) ||
                        x.Mode.Contains(search, ignoreCase) ||
                        x.Class.Contains(search, ignoreCase) ||
                        x.Difficulty.Contains(search, ignoreCase) ||
                        x.Level.ToString().Contains(search, ignoreCase)
                    ).ToList();

                var accounts = accountsTemp
                    .Where(x =>
                        x.Id.ToString().Contains(search, ignoreCase) ||
                        x.Account.Contains(search, ignoreCase) ||
                        x.Email.Contains(search, ignoreCase) ||
                        x.IP.Contains(search, ignoreCase)
                    ).ToList();

                Accounts = new ObservableCollection<AccountViewModel>(accounts);
                Characters = new ObservableCollection<CharacterViewModel>(characters);

                TableLoadingVisibility = Visibility.Collapsed;
            });
        }

        private string search;
        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                PropertyChangedInvoke(nameof(Search));
            }
        }

        public int? TotalAccounts
        {
            get
            {
                return Accounts?.Count;
            }
        }

        public int? TotalAccountsWithoutEmail
        {
            get
            {
                return Accounts?.Count(x => string.IsNullOrWhiteSpace(x.Email));
            }
        }

        public int? TotalAccountsWithoutLogin
        {
            get
            {
                return Accounts?.Count(x => string.IsNullOrWhiteSpace(x.IP));
            }
        }

        public int? TotalAccountsWithDuplicateEmail
        {
            get
            {
                return Accounts?
                    .Where(x => !string.IsNullOrWhiteSpace(x.Email))
                    .GroupBy(x => x.Email)
                    .Where(x => x.Count() > 1)
                    .Sum(x => x.Count());
            }
        }

        public int? TotalCharacters
        {
            get
            {
                return Characters?.Count;
            }
        }

        public int? TotalExpansion
        {
            get
            {
                return Characters?.Count(x => x.Expansion == "Expansion");
            }
        }
        public int? TotalClassic
        {
            get
            {
                return Characters?.Count - TotalExpansion;
            }
        }

        public int? TotalLadder
        {
            get
            {
                return Characters?.Count(x => x.Ladder == "Ladder");
            }
        }
        public int? TotalNonLadder
        {
            get
            {
                return Characters?.Count - TotalLadder;
            }
        }

        public int? TotalHardcore
        {
            get
            {
                return Characters?.Count(x => x.Mode == "Hardcore");
            }
        }
        public int? TotalSoftcore
        {
            get
            {
                return Characters?.Count - TotalHardcore;
            }
        }

        public IEnumerable<dynamic>? TotalClass
        {
            get
            {
                return Characters?
                    .GroupBy(x => x.Class)
                    .OrderByDescending(x => x.Count())
                    .Select(x => new { x.Key, Count = x.Count() });
            }
        }

        private List<AccountViewModel> accountsTemp;
        private ObservableCollection<AccountViewModel> accounts;
        public ObservableCollection<AccountViewModel> Accounts
        {
            get { return accounts; }
            set
            {
                accounts = value;
                PropertyChangedInvoke(nameof(Accounts));
                PropertyChangedInvoke(nameof(TotalAccounts));
                PropertyChangedInvoke(nameof(TotalAccountsWithoutEmail));
                PropertyChangedInvoke(nameof(TotalAccountsWithoutLogin));
                PropertyChangedInvoke(nameof(TotalAccountsWithDuplicateEmail));
            }
        }

        private List<CharacterViewModel> charactersTemp;
        private ObservableCollection<CharacterViewModel> characters;
        public ObservableCollection<CharacterViewModel> Characters
        {
            get { return characters; }
            set
            {
                characters = value;
                PropertyChangedInvoke(nameof(Characters));
                PropertyChangedInvoke(nameof(TotalCharacters));
                PropertyChangedInvoke(nameof(TotalExpansion));
                PropertyChangedInvoke(nameof(TotalClassic));
                PropertyChangedInvoke(nameof(TotalLadder));
                PropertyChangedInvoke(nameof(TotalNonLadder));
                PropertyChangedInvoke(nameof(TotalHardcore));
                PropertyChangedInvoke(nameof(TotalSoftcore));
                PropertyChangedInvoke(nameof(TotalClass));
            }
        }

        private Visibility charactersTableVisibility = Visibility.Collapsed;
        public Visibility CharactersTableVisibility
        {
            get { return charactersTableVisibility; }
            set
            {
                charactersTableVisibility = value;
                PropertyChangedInvoke(nameof(CharactersTableVisibility));
            }
        }

        private Visibility accountsTableVisibility;
        public Visibility AccountsTableVisibility
        {
            get { return accountsTableVisibility; }
            set
            {
                accountsTableVisibility = value;
                PropertyChangedInvoke(nameof(AccountsTableVisibility));
            }
        }

        private Visibility tableLoadingVisibility;
        public Visibility TableLoadingVisibility
        {
            get { return tableLoadingVisibility; }
            set
            {
                tableLoadingVisibility = value;
                PropertyChangedInvoke(nameof(TableLoadingVisibility));
            }
        }

        public ICommand AccountsCommand { get; private set; }
        public ICommand CharactersCommand { get; private set; }
        public ICommand SetupCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
    }
}
