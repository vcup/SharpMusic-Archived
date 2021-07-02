using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpMusic.Backend.Information
{
    public class Album : InformationBase
    {
        private string _name;
        private List<string> _aliasNames;
        private List<Music> _tracks;
        private List<Artist> _artists;
        public static readonly HashSet<Album> AllAlbums = new();

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

        public IList<string> AliasName
        {
            get => _aliasNames;
        }

        [JsonIgnore]
        public IList<Music> Tracks => _tracks;

        public IList<Artist> Artists => _artists;

        public int Lenght => Tracks.Count();

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