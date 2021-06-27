using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpMusic.Information
{
    public class Artist : IInformation
    {
        private string _name;
        private List<string> _nickNames;
        private List<Album> _albums;
        private List<Music> _musics;
        private static HashSet<Artist> _allArtists = new();

        public Artist()
        {
            _allArtists.Add(this);
            foreach (var album in Albums)
            foreach (var music in album.Tracks.Where(x => x.Artists.Contains(this)))
                _musics.Add(music);
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public IEnumerable<string> NickNames
        {
            get => _nickNames;
        }
        [JsonIgnore]
        public IEnumerable<Album> Albums
        {
            get => _albums;
        }

        public IEnumerable<int> AlbumHashCodes
        {
            get => Albums.Select(x => x.GetHashCode());
        }

        [JsonIgnore]
        public IEnumerable<Music> Musics
        {
            get => _musics;
        }

        public static IEnumerable<Artist> GetAllArtists() => _allArtists;
    }
}