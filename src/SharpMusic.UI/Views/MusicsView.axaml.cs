using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SharpMusic.UI.ViewModels;

namespace SharpMusic.UI.Views
{
    public partial class MusicsView : ReactiveUserControl<MusicsViewModel>
    {
        public MusicsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}