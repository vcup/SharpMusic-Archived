using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ManagedBass;
using SharpMusic.Backend.Information;
using SharpMusic.Backend.Information.PlayExtension;
using SharpMusic.Backend.Play.BassManaged;

namespace SharpMusic.Backend.Play
{
    public class Player
    {
        private Channel _channel = new();
        private readonly List<Music> _playingList = new();
        private int _playingIndex = -1;
        private IEnumerator<Music> _musicEnumerator;
        private Playlist _playlist;
        
        public Player(Playlist playlist)
        {
            _musicEnumerator = playlist.RankPlay().GetEnumerator();
            _playlist = new Playlist(playlist);
            MoveNext();
        }

        #region PlayFunc

        public void Play()
        {
            if (State == PlaybackState.Playing)
                Stop();
            _channel.Sound = new Sound(PlayingMusic.StreamUri);
            _channel.Play();
            _channel.SetEndedEvent(() =>
                {
                    if (AutoNext)
                        PlayNext();
                }
            );
        }

        public void Pause() => _channel.Pause();

        public void Resume() => _channel.Resume();

        public void Stop() => _channel.Stop();

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

        public void MovePrev()
        {
            if (_playingIndex == 0)
            {
                _playingIndex = _playingList.Count - 1;
            }
            else
            {
                _playingIndex--;
            }
        }

        public void MoveNext()
        {
            if (_playingIndex == _playingList.Count - 1)
            {
                if (!_musicEnumerator.MoveNext())
                    return;
                _playingList.Add(_musicEnumerator.Current);
                _playingIndex++;
            }
            else
            {
                _playingIndex++;
            }
        }

        public Music PlayingMusic
        {
            get => _playingList[_playingIndex];
            set
            {
                if (_playingList.Contains(value))
                {
                    _playingIndex = _playingList.IndexOf(value);
                    return;
                }

                _playingIndex = _playingList.Count;
                _playingList.Add(value);
            }
        }

        #endregion

        #region PlayState

        public PlaybackState State => _channel.State;

        public TimeSpan PlayTime => _channel.PlayTime;

        public TimeSpan Position
        {
            get => _channel.Position;
            set => _channel.Position = value;
        }

        public double Volume
        {
            get => _channel.Volume;
            set => _channel.Volume = value;
        }

        public bool AutoNext { get; set; } = true;

        #endregion
        
        #region SetPlayerState

        public void ClearUp()
        {
            _playingList.Clear();
            _playingIndex = 0;
        }

        public void UseRankMode()
        { 
            ClearUp();
            _musicEnumerator = _playlist.RankPlay().GetEnumerator();
        }

        public void UseRandomMode()
        {
            ClearUp();
            _musicEnumerator = _playlist.RandomPlay().GetEnumerator();
        }

        public void UseLoopMode()
        {
            _musicEnumerator = _playlist.SingleLoopPlay().GetEnumerator();
        }
        
        public void UseAssignPlayMode(IEnumerable<Music> assign)
        {
            ClearUp();
            _musicEnumerator = assign.GetEnumerator();
        }

        public void AddMusicToPlaying(Music music) =>
            _playingList.Insert(_playingIndex + 1, music);

        public void AddManyMusicToPlaying(IEnumerable<Music> musics) =>
            _playingList.InsertRange(_playingIndex + 1, musics);

        public Playlist Playlist
        {
            get => _playlist;
            set
            {
                ClearUp();
                _playlist = value;
            }
        }
        
        #endregion
        
    }
}