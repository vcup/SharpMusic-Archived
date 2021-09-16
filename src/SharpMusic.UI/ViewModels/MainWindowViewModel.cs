using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using DynamicData;
using ManagedBass;
using ReactiveUI;
using SharpMusic.Backend.Disk;
using SharpMusic.Backend.Information;
using SharpMusic.Backend.Play;

namespace SharpMusic.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IControlsViewModel, IViewModelConform<ISecondaryViewModel>
    {
        public MainWindowViewModel()
        {
            Items.CollectionChanged += (sender, _) =>
            {
                var newItem = ((ObservableCollection<ISecondaryViewModel>) sender!).Last();
                Controls.AddRange(((IControlsViewModel)newItem).Controls); 
                ((IControlsViewModel)newItem).Controls.CollectionChanged += ((controls, _) =>
                {
                    Controls.Clear();
                    Controls.Add(((ObservableCollection<Control>) controls!).Last());   
                });
                newItem.SwitchToThisViewModel = ReactiveCommand.Create(() => SecondaryViewModel = newItem);
            };
            SecondaryViewModel = new MusicsViewModel();
            Items.Add(new AlbumsViewModel());
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
                        this.RaisePropertyChanged(nameof(Position));
                    }
                }
            );
        }

        private int _secondaryViewModelIndex;
        public ISecondaryViewModel SecondaryViewModel
        {
            get => Items[_secondaryViewModelIndex];
            set
            {
                if (Items.Contains(value))
                {
                    this.RaiseAndSetIfChanged(ref _secondaryViewModelIndex, Items.IndexOf(value));
                    return;
                }

                _secondaryViewModelIndex = Items.Count;
                Items.Add(value);
                this.RaisePropertyChanged(nameof(Items));
            }
        }

        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<ISecondaryViewModel> Items { get; set; } = new();
        
        #region PlayerModel
        
        private readonly Player _player;
        public Playlist PlayingList => _player.Playlist;
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

        public bool PlayinglistIsVisible { get; set; } = true;
        public PlaylistViewModel PlayingListViewModel { get; set; }
        public ICommand ShowPlaylist => ReactiveCommand.Create(() =>
            {
                PlayinglistIsVisible = !PlayinglistIsVisible;
                this.RaisePropertyChanged(nameof(PlayinglistIsVisible));
            }
        );

        public ICommand W => ReactiveCommand.Create(() =>
        {
            _player.AddManyMusicToPlaylist(LoadAudioFile.LoadMusicFrom("C:/CloudMusic"));
        });

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

        #endregion

    }
}
