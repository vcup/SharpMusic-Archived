using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SharpMusic.UI.ViewModels;

namespace SharpMusic.UI.Views
{
    public partial class AlbumsView : ReactiveUserControl<AlbumsViewModel>
    {
        public AlbumsView()
        {
            InitializeComponent();
            this.WhenActivated(_ =>
                {
                    var controls = ((ItemsControl) this.Get<ContentControl>("Controls").Content);
                    ViewModel!.Controls.Add(controls);
                }
            );
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}