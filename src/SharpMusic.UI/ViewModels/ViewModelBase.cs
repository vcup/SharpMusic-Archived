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
    
    public interface ISecondaryViewModel : IControlsViewModel, IViewModelConform<ITertiaryViewModel>
    {
        public string SvgIconPath { get; set; }
        
        public  ICommand SwitchToThisViewModel { get; set; }
    }

    public interface ITertiaryViewModel : IControlsViewModel, IViewModelConform<IFourthViewModel>
    {
    }

    public interface IFourthViewModel : IControlsViewModel, IViewModelConform<IFifthViewModel>
    {
    }

    public interface IFifthViewModel : IControlsViewModel, IViewModelConform<object>
    {
    }
}
