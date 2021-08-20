using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using SharpMusic.Backend.Information;

namespace SharpMusic.UI.ViewModels
{
    public class MusicViewModel : ViewModelBase, ITertiaryViewModel

    {
        private readonly Music _music;

        public MusicViewModel()
        {
            _music = new Music();
        }
        
        public MusicViewModel(Music music)
        {
            _music = music;
        }

        public string Title => _music.Name + _music.Alias[0];
        public string Artist => _music.Artists[0].Name;
        public Bitmap Cover => new Bitmap(new MemoryStream(_music.Album.Cover.Data.Data));
        
        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<IFourthViewModel> Items { get; set; } = new();
    }
}