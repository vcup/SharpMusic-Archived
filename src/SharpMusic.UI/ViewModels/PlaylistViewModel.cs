using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DynamicData;
using ReactiveUI;
using SharpMusic.Backend.Information;

namespace SharpMusic.UI.ViewModels
{
    public sealed class PlaylistViewModel : ViewModelBase, ITertiaryViewModel
    {
        public PlaylistViewModel() => Musics = new(new() { Name = $"Playlist.{GetHashCode()}" });

        public PlaylistViewModel(Playlist playlist) => Musics = new(playlist);

        public MusicViewModels Musics { get; }
        
        public class MusicViewModels : INotifyCollectionChanged, IEnumerable<MusicViewModel>
        {
            private readonly Playlist _playlist;

            public MusicViewModels(Playlist playlist)
            {
                _playlist = playlist;
                playlist.CollectionChanged += (_, args) =>
                {
                    args = new(args.Action, new MusicViewModel((Music)(args.NewItems![0])!));
                    CollectionChanged?.Invoke(this, args);
                };
            }
            
            public event NotifyCollectionChangedEventHandler? CollectionChanged;
            public IEnumerator<MusicViewModel> GetEnumerator() => _playlist.Select(m => new MusicViewModel(m)).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public ObservableCollection<IFourthViewModel> Items { get; set; } = new();
    }
}