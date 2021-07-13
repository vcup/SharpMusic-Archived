using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;

namespace SharpMusic.UI.ViewModels
{
    public class MusicsViewModel : ViewModelBase, IControlsViewModel, IViewModelConform<MusicViewModel>
    {
        public MusicsViewModel()
        {
            ScanMusicCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    var vvm = new ScanMusicViewModel();
                    var result = await ShowScanMusic.Handle(vvm);
                    if (result != null) Items.AddRange(result);
                }
            );
        }

        public ICommand ScanMusicCommand { get; set; }
        public Interaction<ScanMusicViewModel, IEnumerable<MusicViewModel>?> ShowScanMusic { get; } = new();
        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<MusicViewModel> Items { get; set; } = new();
    }
}