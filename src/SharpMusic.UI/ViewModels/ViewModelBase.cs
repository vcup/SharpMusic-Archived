using System.Collections.ObjectModel;
using Avalonia.Controls;
using ReactiveUI;

namespace SharpMusic.UI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
    }

    public interface IControlsViewModel
    {
        public ObservableCollection<Control> Controls { get; set; }
    }

    public interface IViewModelConform<TViewModel> where TViewModel : ViewModelBase
    {
        public ObservableCollection<TViewModel> Items { get; set; }
    }
}
