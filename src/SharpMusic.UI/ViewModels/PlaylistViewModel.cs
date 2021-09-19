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
        private Playlist _playlist;
        
        public PlaylistViewModel() => Musics = new(_playlist = new() { Name = $"Playlist.{GetHashCode()}" });

        public PlaylistViewModel(Playlist playlist) => Musics = new(_playlist = playlist);

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
        
        #region Collection Operate

        public void AddMusic(Music item) => _playlist.Add(item);

        public void AddMusic(MusicViewModel item) => _playlist.Add(item.Music);

        public void AddMusics(IEnumerable<Music> items) => _playlist.Add(items);

        public void AddMusics(IEnumerable<MusicViewModel> items) => _playlist.Add(items.Select(m => m.Music));

        public void RemoveMusic(Music item) => _playlist.Remove(item);

        public void RemoveMusic(MusicViewModel item) => _playlist.Remove(item.Music);

        public void InsertMusic(int index, Music item) => _playlist.Insert(index, item);

        public void InsertMusic(int index, MusicViewModel item) => _playlist.Insert(index, item.Music);

        #endregion

        public ObservableCollection<IFourthViewModel> Items { get; set; } = new();
    }
}