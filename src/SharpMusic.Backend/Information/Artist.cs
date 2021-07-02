using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpMusic.Backend.Information
{
    [Serializable]
    public class Artist : InformationBase, ISerializable
    {
        private List<string> _nickNames;
        private List<Album> _albums;
        private List<Music> _musics;
        public static readonly HashSet<Artist> AllArtists = new();

        public Artist()
        {
            _nickNames = new();
            _albums = new();
            _musics = new();
            
            AllArtists.Add(this);
            foreach (var album in Albums)
            foreach (var music in album.Tracks.Where(x => x.Artists.Contains(this)))
                _musics.Add(music);
        }

        #region Serializeble

        public Artist(SerializationInfo info, StreamingContext context)
        {
            Name       = (string)      info.GetValue("Name"    , typeof(string))      ;
            _albums    = (List<Album>) info.GetValue("Albums"  , typeof(List<Album>)) ;
            _nickNames = (List<string>)info.GetValue("NickName", typeof(List<string>));

            AllArtists.Add(this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name"     , Name     , typeof(string)      );
            info.AddValue("Albums"   , Albums   , typeof(List<Album>) );
            info.AddValue("NickNames", NickNames, typeof(List<string>));
        }

        #endregion

        public string Name { get; set; }

        public IList<string> NickNames => _nickNames;
        
        public IList<Album> Albums => _albums;

        public IList<Music> Musics => _musics;
    }
}