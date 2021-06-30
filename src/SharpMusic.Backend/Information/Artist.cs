using System.Collections.Generic;
using System.Linq;

namespace SharpMusic.Backend.Information
{
    public class Artist : InformationBase
    {
        private string _name;
        private List<string> _nickNames;
        private List<Album> _albums;
        private List<Music> _musics;
        private static HashSet<Artist> _allArtists = new();

        public Artist()
        {
            _nickNames = new();
            _albums = new();
            _albums = new();
            
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

        public IList<string> NickNames
        {
            get => _nickNames;
        }
        public IList<Album> Albums
        {
            get => _albums;
        }


        public IList<Music> Musics
        {
            get => _musics;
        }

        public static IEnumerable<Artist> GetAllArtists() => _allArtists;
    }
}