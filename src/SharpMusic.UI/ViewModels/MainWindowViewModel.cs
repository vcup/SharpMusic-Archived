using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;

namespace SharpMusic.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IControlsViewModel, IViewModelConform<ISecondaryViewModel>
    {
        public MainWindowViewModel()
        {
            Items.CollectionChanged += (sender, _) =>
            {
                var newItem = ((ObservableCollection<ISecondaryViewModel>) sender!).Last();
                Controls.AddRange(newItem.Controls); 
                newItem.Controls.CollectionChanged += ((controls, _) =>
                {
                    Controls.Clear();
                    Controls.Add(((ObservableCollection<Control>) controls!).Last());   
                });
                newItem.SwitchToThisViewModel = ReactiveCommand.Create(() => SecondaryViewModel = newItem);
            };
            SecondaryViewModel = new MusicsViewModel();
            Items.Add(new AlbumsViewModel());
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

        public ObservableCollection<Control> ViewModelsSwitchButtons => new(); 
    }
}
