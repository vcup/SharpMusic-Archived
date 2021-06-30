using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagLib;
using File = TagLib.File;

namespace SharpMusic.Backend.Information
{
    public class Music : InformationBase
    {
        private List<string> _alias = new();
        private List<Artist> _artists = new();
        private static readonly HashSet<Music> AllMusics = new();
        private Uri _streamUri;

        public Music()
        {
            AllMusics.Add(this);
        }

        public Music(string fileName)
        {
            fileName = Path.GetFullPath(fileName);
            using (var file = File.Create(fileName))
            {
                SetInfoWithId3(file.Tag);
            }

            _streamUri = new(fileName);
            
            AllMusics.Add(this);
        }

        public Music(Uri uri)
        {
            _streamUri = uri;
            AllMusics.Add(this);
        }

        public void SetInfoWithId3(Tag id3)
        {
            #region SetTextPro
            
            Name = id3.Title + id3.Subtitle;

            #endregion

            #region SetAlbum

            if (id3.Album != string.Empty)
            {
                string albumName = id3.Album;
                var albums = Album.GetAllAlbums().Where(
                    x => x.Name == albumName);
                if (!albums.Any())
                {
                    albums = Album.GetAllAlbums().Where(
                        x => x.AliasName.Any(n => n == albumName));
                }

                if (!albums.Any())
                    Album = new Album() {Name = albumName, Tracks = new[] {this}};
                else
                    Album = albums.First();
            }

            #endregion

            #region SetArtists

            if (id3.Artists.Any())
            {
                var artistNames = id3.Artists;
                List<Artist> artists = Artist.GetAllArtists()
                    .Where(a => artistNames.Any(n => n == a.Name))
                    .ToList();
                if (!artists.Any())
                {
                    artists = Artist.GetAllArtists()
                        .Where(a => a.NickNames.Any(
                            n => artistNames.Any(
                                an => n == an
                                )
                            )
                        )
                        .ToList();
                    
                }

                if (!artists.Any())
                {
                    foreach (var artistName in artistNames)
                    {
                        var artist = new Artist() {Name = artistName};
                        artist.Albums.Add(Album);
                    }
                }
                else
                {
                    foreach (var artist in artists)
                        Artists.Add(artist);
                }
            }

            #endregion
        }

        public string Name { get; set; }

        public IList<string> Alias
        {
            get => _alias;
        }
        public Album Album { get; set; }

        public int AlbumHashCode
        {
            get => Album.GetHashCode();
        }

        public IList<Artist> Artists
        {
            get => _artists;
        }

        public Uri StreamUri
        {
            get => _streamUri;
        }

        public TimeSpan PlayTime { get; set; }

        public static IEnumerable<Music> GetAllMusics() => AllMusics;
    }
}