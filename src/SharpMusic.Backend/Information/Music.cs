using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization;

namespace SharpMusic.Information
{
    public class Music : IInformation
    {
        private string _name;
        private Album _album;
        private List<string> _alias = new();
        private List<Artist> _artists = new();
        private Uri _streamUri;
        private static readonly HashSet<Music> AllMusics = new();

        public Music()
        {
            _streamUri = new(String.Empty);
            AllMusics.Add(this);
        }

        public Music(string fileName)
        {
            _streamUri = new(Path.GetFullPath(fileName));
            
            AllMusics.Add(this);
        }

        public Music(Uri uri)
        {
            _streamUri = uri;
            AllMusics.Add(this);
        }
        
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        
        public IEnumerable<string> Alias
        {
            get => _alias;
            set => _alias = value.ToList();
        }
        public Album Album
        {
            get => _album;
            set => _album = value;
        }

        public int AlbumHashCode
        {
            get => Album.GetHashCode();
        }

        public IEnumerable<Artist> Artists
        {
            get => _artists;
            set => _artists = value.ToList();
        }

        public IEnumerable<int> ArtistHashcode
        {
            get => Artists.Select(x => x.GetHashCode());
        }
        public TimeSpan PlayTime { get; set; }

        public  Uri StreamUri
        {
            get => _streamUri;
        }

        public static IEnumerable<Music> GetAllMusics() => AllMusics;
    }
}