using System;
using System.Collections.Generic;
using ManagedBass;
using SharpMusic.Backend.Information;
using SharpMusic.Backend.Information.PlayExtension;
using SharpMusic.Backend.Play.BassManaged;

namespace SharpMusic.Backend.Play
{
    public class Player
    {
        private Channel _channel = new();
        private readonly Playlist _playingList;
        private int _playingIndex = -1;
        private IEnumerator<Music> _musicEnumerator;
        private Playlist _playlist;
        
        public Player(Playlist playlist)
        {
            _musicEnumerator = playlist.RankPlay().GetEnumerator();
            _playlist = playlist;
            _playingList = new();
            MoveNext();
        }

        #region PlayFunc

        public void Play()
        {
            if (State == PlaybackState.Playing)
                Stop();
            else if (_playingList.Count is 0)
            {
                throw new ArgumentException("The playlist must be filled before it can start playing");
            }
            _channel.Sound = new Sound(PlayingMusic.StreamUri);
            _channel.Play();
            _channel.SetEndedEvent(() =>
                {
                    if (AutoNext)
                        PlayNext();
                }
            );

            PlayBackStartEvent?.Invoke(this, new());
        }

        public void Pause()
        {
            _channel.Pause();
            PlayBackPauseEvent?.Invoke(this, new());
        }

        public void Resume()
        {
            _channel.Resume();
            PlayBackResumeEvent?.Invoke(this, new());
        }

        public void Stop()
        {
            _channel.Stop();
            PlayBackStopEvent?.Invoke(this, new());
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
            if (_playlist.Count == 0)
                return;
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

        public PlaybackState State
        {
            get => _channel.State;
            set
            {
                if (_channel.State == value) return;
                
                switch (value)
                {
                    case PlaybackState.Playing:
                        if (_channel.State == PlaybackState.Paused) Resume();
                        else Play();
                        break;
                    case PlaybackState.Stopped:
                        Stop();
                        break;
                    case PlaybackState.Paused:
                        Pause();
                        break;
                    case PlaybackState.Stalled:
                        Pause(); //　
                        break;
                }
            }
        }

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

        public void AddMusicToPlaylist(Music music) => _playlist.Add(music);

        public void AddManyMusicToPlaylist(IEnumerable<Music> musics) => _playlist.Add(musics);

        #endregion

        #region Event

        public delegate void PlayBackEventHandler(Player sender, PlayBackEventArgs args);

        public class PlayBackEventArgs : EventArgs
        {
            public PlayBackEventArgs()
            {
                
            }
        }

        public event PlayBackEventHandler PlayBackStartEvent;
        public event PlayBackEventHandler PlayBackStopEvent;
        public event PlayBackEventHandler PlayBackPauseEvent;
        public event PlayBackEventHandler PlayBackResumeEvent;

        #endregion
    }
}