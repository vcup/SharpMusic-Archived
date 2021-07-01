using System;
using System.Linq;
using System.Threading.Tasks;
using ManagedBass;
using SharpMusic.Backend.Disk;
using SharpMusic.Backend.Information;
using SharpMusic.Backend.Play;
using SharpMusic.Backend.Play.BassManaged;

namespace SharpMusic.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            int pid = Bass.PluginLoad("./Plugin/Bass/bassflac.dll");
            string.Join(';', Bass.PluginGetInfo(pid).Formats.Select(f => f.FileExtensions))
                .Split(';').Select(LoadAudioFile.FilePattern.Add).ToArray();

            var musics = LoadAudioFile.FromDirectory("./Musics").Select(LoadAudioFile.MakeMusicFormFilePath);
            var playlist = new Playlist() {Name = "test", Description = "a Test Playlist"};
            playlist.Add(musics);
            var player = new Player(playlist);
            
            player.Play();
            Console.ReadKey();
        }
    }
}