using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMusic.Backend.Information
{
    public class Playlist : InformationBase
    {
        private string _name;
        private string _description;
        private List<Music> _musics = new();
        public static readonly HashSet<Playlist> AllPlaylists = new();

        public Playlist()
        {
            _name = _description = String.Empty;
            AllPlaylists.Add(this);
        }

        public Playlist(Playlist playlist)
        {
            Name = playlist.Name;
            Description = playlist.Description;
            Musics = playlist.Musics.ToList();
            AllPlaylists.Add(this);
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public IList<Music> Musics
        {
            get => _musics;
            set => _musics = value.ToList();
        }
        public int Count
        {
            get => Musics.Count;
        }

        public TimeSpan TotalPlaytime
        {
            get
            {
                TimeSpan totalTime = new TimeSpan();
                foreach (var playTime in Musics.Select(x => x.PlayTime))
                {
                    totalTime += playTime;
                }

                return totalTime;
            }
        }

        public void Add(Music music) => _musics.Add(music);
        public void Add(params Music[] musics) => _musics.AddRange(musics);
        public void Add(IEnumerable<Music> musics) => _musics.AddRange(musics);
    }
}