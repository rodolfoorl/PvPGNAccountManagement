using System.Windows.Forms;

namespace PvPGNAccountManagement.Helper
{
    public static class FolderBrowserDialogHelper
    {
        public static string GetFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }

                return string.Empty;
            }
        }
    }
}
