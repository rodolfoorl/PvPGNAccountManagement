using PvPGNAccountManagement.ViewModels;
using System.Windows.Controls;

namespace PvPGNAccountManagement.Views
{
    /// <summary>
    /// Interaction logic for AccountManagement.xaml
    /// </summary>
    public partial class AccountManagement : UserControl
    {
        public AccountManagement(AccountManagementViewModel accountManagement)
        {
            InitializeComponent();
            DataContext = accountManagement;
        }
    }
}
