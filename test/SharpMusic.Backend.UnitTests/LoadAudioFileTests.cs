using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SharpMusic.Backend.Disk;
using SharpMusic.Backend.Information;
using TagLib.Matroska;

namespace SharpMusic.Backend.UnitTests
{
    public class LoadAudioFileTests
    {
        private static void ClearAllInformation()
        {
            Music.AllMusics.Clear();
            Album.AllAlbums.Clear();
            Artist.AllArtists.Clear();
            Playlist.AllPlaylists.Clear();
            GC.Collect();
        }
        
        [Fact]
        public void CreateAlbumTest()
        {
            ClearAllInformation();
            var filePath = "./Samples/Hint.flac";
            var tag = TagLib.File.Create(filePath).Tag;
            var album = LoadAudioFile.MakeMusicFormFilePath(filePath).Album;
            
            Assert.Single(Album.AllAlbums, album);
            Assert.Equal(tag.Album, album.Name);
            Assert.Equal<Array>(tag.AlbumArtists, album.Artists.Select(a => a.Name).ToArray());
            
            ClearAllInformation();
            var existedAlbum = album;
            Album.AllAlbums.Add(existedAlbum);
            album = LoadAudioFile.MakeMusicFormFilePath(filePath).Album;
            Assert.Equal(existedAlbum.GetHashCode(), album.GetHashCode());
            
            ClearAllInformation();
            album = LoadAudioFile.MakeMusicFormFilePath(filePath).Album;
            Assert.NotEqual(existedAlbum.GetHashCode(), album.GetHashCode());
        }

        [Fact]
        public void CreateArtistTest()
        {
            ClearAllInformation();
            var filePath = "./Samples/bass.mp3";
            var tag = TagLib.File.Create(filePath).Tag;
            var music = LoadAudioFile.MakeMusicFormFilePath(filePath);

            Assert.Subset(Artist.AllArtists.Select(a => a.Name).ToHashSet(), tag.Performers.ToHashSet());
            Assert.Subset(Artist.AllArtists.Select(a => a.Name).ToHashSet(), tag.AlbumArtists.ToHashSet());
            Assert.Equal(tag.Performers.Union(tag.AlbumArtists).Count(), Artist.AllArtists.Count);
            
            ClearAllInformation();
            var existedArtist = new Artist() {Name = tag.Performers.First()};
            music = LoadAudioFile.MakeMusicFormFilePath(filePath);

            Assert.True(existedArtist.Musics.Contains(music));
        }

        [Fact]
        public void CreateMusicTest()
        {
            ClearAllInformation();
            var filePath = "./Samples/Hint.flac";
            var tag = TagLib.File.Create(filePath).Tag;
            var music = LoadAudioFile.MakeMusicFormFilePath(filePath);
            
            Assert.Single(Music.AllMusics, music);
            Assert.Equal(tag.Title, music.Name);
            Assert.Equal(tag.Subtitle, music.Alias[0]);
            Assert.Equal(tag.Track, music.TrackNo);
        }
    }
}