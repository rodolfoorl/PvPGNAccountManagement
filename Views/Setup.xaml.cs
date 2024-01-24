using PvPGNAccountManagement.ViewModels;
using System.Windows.Controls;

namespace PvPGNAccountManagement.Views
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : UserControl
    {
        public Setup(SetupViewModel setup)
        {
            InitializeComponent();
            DataContext = setup;
        }
    }
}
