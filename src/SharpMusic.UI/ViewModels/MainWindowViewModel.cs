using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;

namespace SharpMusic.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IControlsViewModel, IViewModelConform<ViewModelBase>
    {
        public MainWindowViewModel()
        {
            Items.CollectionChanged += (sender, _) =>
            {
                try
                {
                    var newItem = (IControlsViewModel)((ObservableCollection<ViewModelBase>) sender!).Last();
                    // May throw InvalidCastException
                    Controls.AddRange(newItem.Controls);
                    newItem.Controls.CollectionChanged += ((controls, _) =>
                        Controls.Add(((ObservableCollection<Control>)controls!).Last()));
                    
                    ViewModelsSwitchButtons.Add(new Button(){Content = newItem.GetType().ToString(), Command = ReactiveCommand.Create(() => SecondaryViewModel = (ViewModelBase) newItem)});
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException($"Element type {sender!.GetType()} must implement interface {nameof(IControlsViewModel)}");
                }
            };
            SecondaryViewModel = new MusicsViewModel();
            Items.Add(new AlbumsViewModel());
        }

        private int _secondaryViewModelIndex;
        public ViewModelBase SecondaryViewModel
        {
            get => Items[_secondaryViewModelIndex];
            set
            {
                Controls.Clear();
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
        public ObservableCollection<ViewModelBase> Items { get; set; } = new();
        public ObservableCollection<Control> ViewModelsSwitchButtons { get; set; } = new();
    }
}
