using System;
using System.Collections.Generic;
using CSCore.Codecs.RAW;

namespace SharpMusic.Information.PlayExtension
{
    public static class PlaylistExtension
    {
        public static Dictionary<Playlist, int> PlayingIndexs = new();
        
        public static IEnumerable<Music> RankPlay(this Playlist playlist)
        {
            PlayingIndexs.TryAdd(playlist, 0);
            while (true)
            {
                foreach (var music in playlist.Musics)
                {
                    PlayingIndexs[playlist] = playlist.Musics.IndexOf(music);
                    yield return music;
                }
            }
        }

        delegate int rand();

        private static Random _random = new();

        public static IEnumerable<Music> RandomPlay(this Playlist playlist)
        { 
            rand rand = () => _random.Next(0, playlist.Count - 1);
            PlayingIndexs.TryAdd(playlist, rand());
            while (true)
            {
                var musicIndex = rand();
                PlayingIndexs[playlist] = musicIndex;
                yield return playlist.Musics[musicIndex];
            }
        }

        public static IEnumerable<Music> SingleLoopPlay(this Playlist playlist)
        {
            var music = playlist.Musics[PlayingIndexs[playlist]];
            while (true)
            {
                yield return music;
            }
        }

        public static Music PlayingMusic(this Playlist playlist)
        {
            return playlist.Musics[PlayingIndexs[playlist]];
        }
    }
}