#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpMusic.Backend.Information
{
    [Serializable]
    public class Playlist : InformationBase, IList<Music>, INotifyCollectionChanged
    {
        private List<Music> _musics = new();
        public static readonly HashSet<Playlist> AllPlaylists = new();

        public Playlist()
        {
            AllPlaylists.Add(this);
        }

        public Playlist(Playlist playlist)
        {
            Name = playlist.Name;
            Description = playlist.Description;
            _musics = playlist.ToList();
            AllPlaylists.Add(this);
        }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";


        public TimeSpan TotalPlaytime
        {
            get
            {
                TimeSpan totalTime = new TimeSpan();
                foreach (var playTime in _musics.Select(x => x.PlayTime))
                {
                    totalTime += playTime;
                }

                return totalTime;
            }
        }

        #region Implemented IList
        
        public int Count => _musics.Count;
        
        public bool IsReadOnly => false;
        
        public void Add(Music music)
        {
            _musics.Add(music);
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, music, _musics.Count-1));
        }

        public void Add(params Music[] musics)
        {
            foreach (var music in musics)
                Add(music);
        }

        public void Add(IEnumerable<Music> musics)
        {
            foreach (var music in musics)
                Add(music);
        }
        
        public void Clear()
        {
            _musics.Clear();
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(Music item) => _musics.Contains(item);

        public void CopyTo(Music[] array, int arrayIndex) => _musics.CopyTo(array, arrayIndex);

        public IEnumerator<Music> GetEnumerator() => _musics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(Music item) => _musics.IndexOf(item);

        public void Insert(int index, Music item)
        {
            _musics.Insert(index, item);
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, item, index));
        }

        public void InsertRange(int index, IEnumerable<Music> collection)
        {
            _musics.InsertRange(index, collection);
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, collection, index));
        }

        public bool Remove(Music item)
        {
            if (_musics.Remove(item))
            {
                CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, _musics));
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            _musics.RemoveAt(index);
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, _musics, index));
        }

        public Music this[int index]
        {
            get => _musics[index];
            set
            {
                _musics[index] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, _musics, index));
            }
        }

        #endregion
        
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
    }
}