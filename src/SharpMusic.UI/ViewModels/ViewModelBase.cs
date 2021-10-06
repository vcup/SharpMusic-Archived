using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
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

    public interface IViewModelConform<TViewModel>
    {
        public ObservableCollection<TViewModel> Items { get; set; }
    }
    
    public interface ISecondaryViewModel : IViewModelConform<ITertiaryViewModel>
    {
        public string SvgIconPath { get; set; }
    }

    public interface ITertiaryViewModel : IViewModelConform<IFourthViewModel>
    {
    }

    public interface IFourthViewModel : IViewModelConform<IFifthViewModel>
    {
    }

    public interface IFifthViewModel : IViewModelConform<object>
    {
    }
}
