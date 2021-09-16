using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SharpMusic.UI.Views
{
    public partial class PlayingListView : UserControl
    {
        public PlayingListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}