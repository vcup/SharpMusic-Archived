using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;

namespace SharpMusic.UI.ViewModels
{
    public class AlbumsViewModel : ViewModelBase, ISecondaryViewModel
    {
        public AlbumsViewModel()
        {
            SwitchToThisViewModel = ReactiveCommand.Create(() => { });
        }

        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<ITertiaryViewModel> Items { get; set; } = new();
        public string SvgIconPath { get; set; } = "/Assets/AlbumsViewIcon.svg";
        public ICommand SwitchToThisViewModel { get; set; }
    }
}