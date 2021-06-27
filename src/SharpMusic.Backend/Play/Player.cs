using System;
using System.Collections.Generic;
using System.Linq;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using SharpMusic.Information;
using SharpMusic.Information.PlayExtension;

namespace SharpMusic.Play
{
    public class Player
    {
        private ISoundOut _waveOut;
        private MMDevice _device;
        private List<Music> PlayingList = new();
        private int PlayingIndex = -1;
        private IEnumerator<Music> musicEnumerator;
        public Playlist Playlist;

        public Player(Playlist playlist)
        {
            var devices = new MMDeviceEnumerator();
            _device = devices.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

            _waveOut = new WasapiOut() {Device = _device, Latency = 100};
            musicEnumerator = playlist.RankPlay().GetEnumerator();
            Playlist = new Playlist(playlist);
            MoveNext();
        }

        #region PlayFunc
        public void Play()
        {
            if (State == PlaybackState.Playing)
                Stop();
            _waveOut.Initialize(PlayingMusic.GetWaveSource());
            _waveOut.Play();
        }

        public void Pause() => _waveOut.Pause();
        
        public void Resume() => _waveOut.Resume();

        public void Stop()
        {
            _waveOut.Stop();
        }

        public void PlayPrev()
        {
            MovePrev();
            Play();
        }
        public void PlayNext()
        {
            MoveNext();
            Play();
        }
        #endregion


        #region Playing
        public Music MovePrev() => PlayingList[
            PlayingIndex == 0 ? PlayingIndex = (PlayingList.Count - 1) : PlayingIndex--
        ];

        public Music MoveNext()
        {
            if (!musicEnumerator.MoveNext())
                musicEnumerator.Reset();
            PlayingList.Add(musicEnumerator.Current);
            return PlayingList[++PlayingIndex];
        }

        public Music PlayingMusic
        {
            get => PlayingList[PlayingIndex];
            set
            {
                if (PlayingList.Contains(value))
                {
                    PlayingIndex = PlayingList.IndexOf(value);
                    return;
                }
                
                PlayingIndex = PlayingList.Count;
                PlayingList.Add(value);
            }
        }
        public PlaybackState State
        {
            get => _waveOut.PlaybackState;
        }
        #endregion

        public void ClearUp()
        {
            PlayingList.Clear();
            PlayingIndex = 0;
        }

        public void UseRankMode()
        { 
            ClearUp();
            musicEnumerator = Playlist.RankPlay().GetEnumerator();
        }

        public void UseRandomMode()
        {
            ClearUp();
            musicEnumerator = Playlist.RandomPlay().GetEnumerator();
        }

        public void UseLoopMode()
        {
            musicEnumerator = Playlist.SingleLoopPlay().GetEnumerator();
        }
        
        public void UseAssignPlayMode(IEnumerable<Music> assign)
        {
            ClearUp();
            musicEnumerator = assign.GetEnumerator();
        }

        public void AddMusicToPlaying(Music music) =>
            PlayingList.Insert(PlayingIndex + 1, music);

        public void AddManyMusicToPlaying(IEnumerable<Music> musics) =>
            PlayingList.InsertRange(PlayingIndex + 1, musics);
    }
}