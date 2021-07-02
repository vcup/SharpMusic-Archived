using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpMusic.Backend.Information
{
    [Serializable]
    public class Playlist : InformationBase, ISerializable
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
            _musics = playlist.Musics.ToList();
            AllPlaylists.Add(this);
        }

        #region Serialization

        public Playlist(SerializationInfo info, StreamingContext context)
        {
            Name        = (string)      info.GetValue("Name"       , typeof(string)     );
            _musics     = (List<Music>) info.GetValue("Music"      , typeof(List<Music>));
            Description = (string)      info.GetValue("Description", typeof(string)     );
            
            AllPlaylists.Add(this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name"       , Name       , typeof(string)     );
            info.AddValue("Music"      , Musics     , typeof(List<Music>));
            info.AddValue("Description", Description, typeof(string)     );
        }

        #endregion
        
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<Music> Musics => _musics;

        public int Count => Musics.Count;

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