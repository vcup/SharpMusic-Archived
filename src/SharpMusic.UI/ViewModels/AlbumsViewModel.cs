using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;
using SharpMusic.Backend.Information;
using TagLib.Ape;

namespace SharpMusic.UI.ViewModels
{
    public class AlbumsViewModel : ViewModelBase, ISecondaryViewModel, IControlsViewModel
    {
        public AlbumsViewModel()
        {
            Items.AddRange(Album.AllAlbums.Select(a => new AlbumViewModel(a)));
            Album.AllAlbums.CollectionChanged += (sender, args) =>
            {
                Items.Clear();
                Items.Add(Album.AllAlbums.Select(a => new AlbumViewModel(a)));
            };
        }

        public ObservableCollection<Control> Controls { get; set; } = new();
        
        public ObservableCollection<ITertiaryViewModel> Items { get; set; } = new();
        
        public string SvgIconPath { get; set; } = "/Assets/AlbumsViewIcon.svg";
        
    }
}