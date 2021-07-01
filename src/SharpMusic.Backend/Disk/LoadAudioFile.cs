using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManagedBass;
using SharpMusic.Backend.Information;

namespace SharpMusic.Backend.Disk
{
    public static class LoadAudioFile
    {
        #region Field

        public static readonly HashSet<string> FilePattern = Bass.SupportedFormats.Split(';').ToHashSet();

        #endregion

        public static IEnumerable<string> FromDirectory(string path) =>
            Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => FilePattern.Any(es => s.EndsWith(es.Remove(0, 1))));

        public static Music MakeMusicFormFilePath(string filePath)
        {
            Music music = new() {StreamUri = new Uri(Path.GetFullPath(filePath))};
            using var file = TagLib.File.Create(filePath);

            #region SetProperty

            var tag = file.Tag;
            music.Name = tag.Title + tag.Subtitle;
            music.TrackId = tag.TrackCount;
            music.PlayTime = file.Properties.Duration;

            #endregion

            #region SetAlbum

            if (tag.Album != string.Empty)
            {
                string albumName = tag.Album;
                var albums = Album.GetAllAlbums().Where(
                    x => x.Name == albumName);
                if (!albums.Any())
                {
                    albums = Album.GetAllAlbums().Where(
                        x => x.AliasName.Any(n => n == albumName));
                }

                if (!albums.Any())
                    music.Album = new Album() {Name = albumName, Tracks = new[] {music}};
                else
                    music.Album = albums.First();
            }

            #endregion

            #region SetArtists

            if (tag.Performers.Any())
            {
                var artistNames = tag.Performers;
                IEnumerable<Artist> artists = Artist.GetAllArtists()
                    .Where(a => artistNames.Any(n => n == a.Name));
                if (!artists.Any())
                {
                    artists = Artist.GetAllArtists()
                        .Where(a => a.NickNames.Any(
                                n => artistNames.Any(
                                    an => n == an
                                )
                            )
                        );
                }

                if (!artists.Any())
                {
                    foreach (var artistName in artistNames)
                    {
                        var artist = new Artist() {Name = artistName};
                        artist.Albums.Add(music.Album);
                        music.Artists.Add(artist);
                    }
                }
                else
                {
                    foreach (var artist in artists)
                        music.Artists.Add(artist);
                }
            }

            #endregion

            return music;
        }

        public static IEnumerable<Music> LoadMusicFrom(string path) =>
            FromDirectory(path).Select(MakeMusicFormFilePath);
    }
}
