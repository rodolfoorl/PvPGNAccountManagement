using CharacterEditor;
using System.IO;

namespace PvPGNAccountManagement.ViewModels
{
    public class CharacterViewModel : ViewModel
    {
        public CharacterViewModel(string charinfo)
        {
            if (File.Exists(charinfo))
            {
                try
                {
                    var data = File.ReadAllBytes(charinfo);
                    var character = new CharInfo();
                    character.Read(data);

                    Account = character.UserName;
                    Name = character.Name;
                    Realm = character.RealmName;
                    Expansion = character.Expansion ? "Expansion" : "Classic";
                    Ladder = character.Ladder ? "Ladder" : "Non-Ladder";
                    Mode = character.Hardcore ? "Hardcore" : "Softcore";
                    Class = character.Class.ToString();
                    Level = character.Level;
                    Difficulty = character.DifficultyTitle;
                }
                catch
                {
                    //TODO: log
                }
            }
        }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Realm { get; set; }
        public string Expansion { get; set; }
        public string Ladder {  get; set; }
        public string Mode {  get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public string Difficulty { get; set; }
    }
}
