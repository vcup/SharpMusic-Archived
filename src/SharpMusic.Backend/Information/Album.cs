
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Threading;

namespace SharpMusic.Information
{
    public class Album : IInformation
    {
        private string _name;
        private HashSet<string> _aliasNames;
        private HashSet<Music> _tracks;
        private HashSet<Artist> _artists;
        private static readonly HashSet<Album> AllAlbums = new();

        public Album()
        {
            (_aliasNames, _tracks, _artists) = (new(), new(), new());
            AllAlbums.Add(this);
        }
        
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public IEnumerable<string> AliasName
        {
            get => _aliasNames;
            set => _aliasNames = value.ToHashSet();
        }

        [JsonIgnore]
        public IEnumerable<Music> Tracks
        {
            get => _tracks;
            set => _tracks = value.ToHashSet();
        }

        public IEnumerable<int> TracksHashCodes
        {
            get => Tracks.Select(x => x.GetHashCode());
        }

        [JsonIgnore]
        public IEnumerable<Artist> Artists
        {
            get => _artists;
        }

        public IEnumerable<int> ArtistHashCodes
        {
            get => Artists.Select(x => x.GetHashCode());
        }

        public int Lenght
        {
            get => Tracks.Count();
        }

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

        public static IEnumerable<Album> GetAllAlbums() => AllAlbums;
    }
}