using opogsr_common;
using opogsr_launcher.Helpers;
using opogsr_launcher.Other.RuntimeResource;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;

namespace opogsr_launcher.ViewModels
{
    public class SaveFile
    {
        public string FileName { get; set; }
        public string Text { get; set; }
        public string WriteTime { get; set; }
    }

    public partial class LoadViewModel : ViewModelBase
    {
        [Reactive] public partial ObservableCollection<SaveFile> Saves { get; set; }

        [Reactive] public partial SaveFile SelectedSave { get; set; }

        [Reactive] public partial ReactiveCommand<Unit, Unit> LoadSaveCommand { get; set; }

        [Reactive] public partial RuntimeResourceString LoadSaveStr { get; set; }


        private void LoadSavesList()
        {
            if (Directory.Exists(StaticGlobals.Locations.Saves))
            {
                DirectoryInfo di = new DirectoryInfo(StaticGlobals.Locations.Saves);
                FileInfo[] fi = di.GetFiles("*" + StaticGlobals.Extensions.Save);
                var sorted = fi.OrderByDescending(x => x.LastWriteTime);

                foreach (FileInfo t in sorted)
                {
                    string str = t.Name.Replace(StaticGlobals.Extensions.Save, "");

                    Saves.Add(new SaveFile
                    {
                        FileName = t.Name,
                        Text = str,
                        WriteTime = t.LastWriteTime.ToString()
                    });
                }
            }
        }

        private void LoadSave()
        {
            LauncherHelper.LaunchGame(args: ["-start server(" + SelectedSave.FileName.Replace(StaticGlobals.Extensions.Save, "") + "/single/alife/load) client(localhost)"]);
            ApplicationHelper.MainWindow.Close();
        }

        public LoadViewModel()
        {
            Saves = [];

            LoadSaveStr = new("LoadSave");

            LoadSaveCommand = ReactiveCommand.Create(LoadSave);

            LoadSavesList();
        }
    }
}
