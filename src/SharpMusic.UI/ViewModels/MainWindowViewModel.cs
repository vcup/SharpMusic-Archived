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
                var newItem = (IControlsViewModel)((ObservableCollection<ViewModelBase>) sender!).Last();
                Controls.AddRange(newItem.Controls);
                newItem.Controls.CollectionChanged += ((controls, _) =>
                    Controls.Add(((ObservableCollection<Control>)controls!).Last()));
            };
            _secondaryViewModel = new MusicsViewModel();
            Items.Add(_secondaryViewModel);
        }

        private ViewModelBase _secondaryViewModel;
        public ViewModelBase SecondaryViewModel
        {
            get => _secondaryViewModel;
            set => this.RaiseAndSetIfChanged(ref _secondaryViewModel, value);
        }

        public ObservableCollection<Control> Controls { get; set; } = new();
        public ObservableCollection<ViewModelBase> Items { get; set; } = new();
    }
}
