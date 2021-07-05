using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using SharpMusic.Backend.Disk;

namespace SharpMusic.UI.ViewModels
{
    public class ScanMusicViewModel : ViewModelBase
    {
        public ScanMusicViewModel()
        {
            Path = String.Empty;
            Enter = ReactiveCommand.Create(() =>
            {
                if (Path == string.Empty)
                    return null;
                return LoadAudioFile.LoadMusicFrom(Path).Select(m => new MusicViewModel(m));
            });
        }
        
        public string Path { get; set; }
        public ReactiveCommand<Unit, IEnumerable<MusicViewModel>?> Enter { get;}
    }
}