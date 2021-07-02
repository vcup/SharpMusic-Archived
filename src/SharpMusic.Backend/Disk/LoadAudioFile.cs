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
            music.Name = tag.Title;
            music.Alias.Add(tag.Subtitle);
            music.TrackNo = tag.Track;
            music.PlayTime = file.Properties.Duration;

            #endregion

            #region SetAlbum

            if (tag.Album != string.Empty)
            {
                string albumName = tag.Album;
                var albums = Album.AllAlbums.Where(
                    x => x.Name == albumName);
                if (!albums.Any())
                {
                    albums = Album.AllAlbums.Where(
                        x => x.AliasName.Any(n => n == albumName));
                }

                if (!albums.Any())
                {
                    music.Album = new Album() {Name = albumName};
                    music.Album.Tracks.Add(music);
                }
                else
                    music.Album = albums.First();
            }

            #endregion

            #region SetAlbumArtists
            
            if (tag.AlbumArtists.Any())
            {
                var artistNames = tag.AlbumArtists;
                IEnumerable<Artist> artists = Artist.AllArtists
                    .Where(a => artistNames.Any(n => n == a.Name));
                if (artists.Count() != artistNames.Length)
                {
                    foreach (var artist in artists)
                    {
                        artist.Albums.Add(music.Album);
                        music.Album.Artists.Add(artist);
                    }
                    
                    artists = Artist.AllArtists
                            .Where(a => !music.Artists.Contains(a))
                            .Where(a => artistNames.Any(n => a.NickNames.Any(nn => nn == n)));
                }

                if (artists.Count() + music.Artists.Count() != artistNames.Length)
                {
                    foreach (var artist in artists)
                    {
                        artist.Albums.Add(music.Album);
                        music.Album.Artists.Add(artist);
                    }
                    
                    foreach (var artistName in artistNames)
                    {
                        var artist = new Artist() {Name = artistName};
                        artist.Albums.Add(music.Album);
                        music.Album.Artists.Add(artist);
                    }
                }
                else
                {
                    foreach (var artist in artists)
                    {
                        music.Album.Artists.Add(artist);
                        artist.Albums.Add(music.Album);
                    }
                }
            }

            #endregion

            #region SetArtists

            if (tag.Performers.Any())
            {
                var artistNames = tag.Performers;
                IEnumerable<Artist> artists = Artist.AllArtists
                    .Where(a => artistNames.Any(n => n == a.Name));
                if (artists.Count() != artistNames.Length)
                {
                    foreach (var artist in artists)
                    {
                        artist.Musics.Add(music);
                        music.Artists.Add(artist);
                    }
                    
                    artists = Artist.AllArtists
                            .Where(a => !music.Artists.Contains(a))
                            .Where(a => artistNames.Any(n => a.NickNames.Any(nn => nn == n)));
                }

                if (artists.Count() + music.Artists.Count() != artistNames.Length)
                {
                    foreach (var artist in artists)
                    {
                        artist.Musics.Add(music);
                        music.Artists.Add(artist);
                    }

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
                    {
                        artist.Musics.Add(music);
                        music.Artists.Add(artist);
                    }
                }
            }

            #endregion

            return music;
        }

        public static IEnumerable<Music> LoadMusicFrom(string path) =>
            FromDirectory(path).Select(MakeMusicFormFilePath);
    }
}
