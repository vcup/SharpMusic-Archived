using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpMusic.Backend.Information
{
    [Serializable]
    public class Album : InformationBase, ISerializable
    {
        private List<string> _aliasNames;
        private List<Music> _tracks;
        private List<Artist> _artists;
        public static readonly HashSet<Album> AllAlbums = new();

        public Album()
        {
            (_aliasNames, _tracks, _artists) = (new(), new(), new());
            AllAlbums.Add(this);
        }

        public Album(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            _aliasNames = (List<string>) info.GetValue("AliasName", typeof(List<string>));
            _tracks = (List<Music>) info.GetValue("Track", typeof(List<Music>));
            _artists = (List<Artist>) info.GetValue("Artists", typeof(List<Artist>));
            ReleaseDate = (DateTime) info.GetValue("ReleaseDate", typeof(DateTime))!;

            AllAlbums.Add(this);
        }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name"       , Name       , typeof(string));
            info.AddValue("ReleaseDate", ReleaseDate, typeof(DateTime));
            info.AddValue("Track"      , Tracks     , typeof(List<Music>));
            info.AddValue("Artists"    , Artists    , typeof(List<Artist>));
            info.AddValue("AliasName"  , AliasName  , typeof(List<string>));
        }
        
        public string Name { get; set; }

        public IList<string> AliasName => _aliasNames;

        public IList<Music> Tracks => _tracks;

        public IList<Artist> Artists => _artists;

        public int Lenght => Tracks.Count;

        public TimeSpan TotalTime
        {
            get
            {
                TimeSpan totalTime = new TimeSpan();
                foreach (var playTime in Tracks.Select(x => x.PlayTime))
                {
                    totalTime += playTime;
                }

                return totalTime;
            }
        }

        public DateTime ReleaseDate { get; set; }
    }
}