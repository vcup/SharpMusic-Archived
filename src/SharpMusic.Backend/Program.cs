using System;
using System.Linq;
using System.Threading.Tasks;
using ManagedBass;
using SharpMusic.Backend.Information;
using SharpMusic.Backend.Play;
using SharpMusic.Backend.Play.BassManaged;

namespace SharpMusic.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var devices = Device.DeviceEnumerable().ToList(); // 这行代码好像没什么用，但是删掉的话就跑不起来

            var musics = new Music[]
            {
                new("./Musics/1.mp3"),
                new("./Musics/2.flac"),
            };
            var playlist = new Playlist() {Name = "test", Description = "a Test Playlst"};
            playlist.Add(musics);
            var player = new Player(playlist);

            player.Play();
            Console.ReadKey();
            Console.WriteLine(player.State == PlaybackState.Playing);
            player.Play();
            Console.ReadKey();
            player.PlayNext();
            Console.ReadKey();
            MediaPlayer mmp = new();
        }
    }
}