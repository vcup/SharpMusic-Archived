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
    }
}