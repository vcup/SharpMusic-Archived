using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SharpMusic.UI.Views
{
    public partial class PlaylistView : UserControl
    {
        public PlaylistView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}