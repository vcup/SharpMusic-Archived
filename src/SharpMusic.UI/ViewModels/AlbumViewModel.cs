using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using SharpMusic.Backend.Information;

namespace SharpMusic.UI.ViewModels
{
    public class AlbumViewModel : ViewModelBase,  ITertiaryViewModel
    {
        private readonly Album _album;
        
        public AlbumViewModel(Album album)
        {
            _album = album;
        }

        public Bitmap Cover => new Bitmap(new MemoryStream(_album.Cover.Data.Data));
        public string Title => _album.Name;
        public string Artists => string.Join(@"\", _album.TracksArtists.Select(a => a.Name));
        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<IFourthViewModel> Items { get; set; } = new();
    }
}