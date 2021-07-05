using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;

namespace SharpMusic.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Musics = new();
            ShowScanMusic = new();
            ScanMusicCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    var vvm = new ScanMusicViewModel();
                    var result = await ShowScanMusic.Handle(vvm);
                    if (result != null) Musics.AddRange(result);
                }
            );
        }

        public ObservableCollection<MusicViewModel> Musics { get; set; }
        public ICommand ScanMusicCommand { get; set; }
        public Interaction<ScanMusicViewModel, IEnumerable<MusicViewModel>?> ShowScanMusic { get; }
    }
}
