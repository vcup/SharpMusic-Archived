using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SharpMusic.UI.Views
{
    public partial class AlbumView : UserControl
    {
        public AlbumView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}