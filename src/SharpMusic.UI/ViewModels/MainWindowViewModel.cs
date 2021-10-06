using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using DynamicData;
using ManagedBass;
using ReactiveUI;
using SharpMusic.Backend.Play;

namespace SharpMusic.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IControlsViewModel, IViewModelConform<ISecondaryViewModel>
    {
        public MainWindowViewModel()
        {
            List<ISecondaryViewModel> viewModels = new()
            {
                new MusicsViewModel(),
                new AlbumsViewModel(),
            };
            Items.CollectionChanged += (sender, _) =>
            {
                var newItem = ((ObservableCollection<ISecondaryViewModel>) sender!).Last();
                Controls.AddRange(((IControlsViewModel)newItem).Controls); 
                ((IControlsViewModel)newItem).Controls.CollectionChanged += ((controls, _) =>
                {
                    Controls.Clear();
                    Controls.Add(((ObservableCollection<Control>) controls!).Last());   
                });
                if (newItem.GetType() == typeof(MusicsViewModel))
                {
                    ((MusicsViewModel)newItem).Items.CollectionChanged += (_, args) =>
                    {
                        if (args.NewItems![0]!.GetType() != typeof(MusicViewModel)) return;
                        var mvm = (MusicViewModel) args.NewItems[0]!;
                        mvm.AddToPlaylist = ReactiveCommand.Create(() => _player!.AddMusicToPlaylist(mvm.Music));
                    };
                }
            };
            Items.AddRange(viewModels);
            
            _player = new(new() {Name = "NowPlaying", Description = "rt"});
            PlayingListViewModel = new(_player.Playlist);
            _player.PlayBackStartEvent += (sender, _) =>
            {
                var m = sender.PlayingMusic;
                PlayingMusicInfo = $"{m.Name} - {string.Join("/", m.Artists.Select(ma => ma.Name))}";
                this.RaisePropertyChanged(nameof(PlayTime));
            };
            Task.Run(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                        if (_player.State == PlaybackState.Stopped) continue;
                        this.RaisePropertyChanged(nameof(Position));
                        this.RaisePropertyChanged(nameof(PosAndTime));
                    }
                }
            );
        }

        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<ISecondaryViewModel> Items { get; set; } = new();
        
        #region PlayerModel
        
        private readonly Player _player;
        
        public ICommand PlayNext => ReactiveCommand.Create(_player.PlayNext);
        
        public ICommand PlayOrPause => ReactiveCommand.Create(() =>
            {
                if (_player.State == PlaybackState.Stopped) _player.Play();
                else if (_player.State == PlaybackState.Playing) _player.Pause();
                else if (_player.State == PlaybackState.Paused) _player.Resume();
                else _player.Play();
            }
        );
        
        public ICommand PlayPrev => ReactiveCommand.Create(_player.PlayPrev);
        
        public ICommand StopPlay => ReactiveCommand.Create(_player.Stop);

        private bool _playinglistIsHitTestVisible;
        public bool PlayinglistIsHitTestVisible {
            get => _playinglistIsHitTestVisible;
            set => this.RaiseAndSetIfChanged(ref _playinglistIsHitTestVisible, value, nameof(PlayinglistIsHitTestVisible));
        }
        
        public PlaylistViewModel PlayingListViewModel { get; set; }
        
        public ICommand ShowPlaylist => ReactiveCommand.Create(() => PlayinglistIsHitTestVisible = !PlayinglistIsHitTestVisible);

        public ICommand ShowScanMusicWindow => ((MusicsViewModel)Items[0]).ScanMusicCommand;

        #endregion

        #region PlayInfomationAndState

        private string _playingMusicInfo = "";
        
        public string PlayingMusicInfo
        {
            get => _playingMusicInfo;
            set => this.RaiseAndSetIfChanged(ref _playingMusicInfo, value);
        }

        public Double Position
        {
            get => _player.Position.TotalSeconds;
            set
            {
                _player.Position = TimeSpan.FromSeconds(value);
                this.RaisePropertyChanged(nameof(Position));
            }
        }

        public Double PlayTime => _player.PlayTime.TotalSeconds;

        public string PosAndTime => _player.State != PlaybackState.Stopped?
            $"{_player.Position.Minutes:00}:{_player.Position.Seconds:00}/{_player.PlayTime.Minutes:00}:{_player.PlayTime.Seconds:00} "
            : "";

        #endregion

    }
}
