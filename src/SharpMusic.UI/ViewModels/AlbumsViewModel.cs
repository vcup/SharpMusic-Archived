using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;

namespace SharpMusic.UI.ViewModels
{
    public class AlbumsViewModel : ViewModelBase, IControlsViewModel, IViewModelConform<AlbumViewModel>
    {
        public AlbumsViewModel()
        {
        }

        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<AlbumViewModel> Items { get; set; } = new();
    }
}