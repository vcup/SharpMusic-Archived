using System;
using System.Collections.Generic;

namespace SharpMusic.Backend.Information.PlayExtension
{
    public static class PlaylistExtension
    {
        public static readonly Dictionary<Playlist, int> PlayingIndex = new();
        
        public static IEnumerable<Music> RankPlay(this Playlist playlist)
        {
            PlayingIndex.TryAdd(playlist, 0);
            while (true)
            {
                foreach (var music in playlist)
                {
                    PlayingIndex[playlist] = playlist.IndexOf(music);
                    yield return music;
                }
            }
        }

        delegate int Rand();

        private static Random _random = new();

        public static IEnumerable<Music> RandomPlay(this Playlist playlist)
        { 
            Rand rand = () => _random.Next(0, playlist.Count - 1);
            PlayingIndex.TryAdd(playlist, rand());
            while (true)
            {
                var musicIndex = rand();
                PlayingIndex[playlist] = musicIndex;
                yield return playlist[musicIndex];
            }
        }

        public static IEnumerable<Music> SingleLoopPlay(this Playlist playlist)
        {
            var music = playlist[PlayingIndex[playlist]];
            while (true)
            {
                yield return music;
            }
        }

        public static Music PlayingMusic(this Playlist playlist)
        {
            return playlist[PlayingIndex[playlist]];
        }
    }
}