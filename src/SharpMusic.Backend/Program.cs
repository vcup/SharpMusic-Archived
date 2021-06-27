using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SharpMusic.Information;
using SharpMusic.Play;

namespace SharpMusic
{
    class Program
    {
        static void Main(string[] args)
        {
            var musics = new Music[]
            {
                new("./Cache/1.mp3"),
                new("./Cache/2.flac"),
            };
            var playlist = new Playlist() {Name = "test", Description = "a Test Playlst"};
            playlist.Add(musics);
            var player = new Player(playlist);
            
            player.Play();
            Console.ReadKey();
            player.MoveNext();
            player.Play();
        }
    }
}