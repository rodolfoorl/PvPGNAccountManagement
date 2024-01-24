using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace PvPGNAccountManagement.ViewModels
{
    public class AccountViewModel : ViewModel
    {
        public AccountViewModel(string plain)
        {
            if (File.Exists(plain))
            {
                try
                {
                    var plainObject = File.ReadAllLines(plain)
                    .Where(x => !string.IsNullOrWhiteSpace(plain))
                    .Select(x =>
                    {
                        var line = x.Split("\"=\"");
                        return new
                        {
                            key = line.First().Replace("\"", string.Empty),
                            value = line.Last().Replace("\"", string.Empty),
                        };
                    });

                    Id = int.Parse(plainObject.FirstOrDefault(x => x.key.EndsWith("\\\\userid"))?.value ?? "0");
                    Account = plainObject.FirstOrDefault(x => x.key.EndsWith("\\\\username"))?.value ?? string.Empty;
                    Email = plainObject.FirstOrDefault(x => x.key.EndsWith("\\\\email"))?.value ?? string.Empty;
                    IP = plainObject.FirstOrDefault(x => x.key.EndsWith("\\\\lastlogin_ip"))?.value ?? string.Empty;
                }
                catch
                {
                    //TODO: log
                }
            }
        }

        public long Id { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }
        public ObservableCollection<CharacterViewModel> Characters { get; set; }
    }
}
