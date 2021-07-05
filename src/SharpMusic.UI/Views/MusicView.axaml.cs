using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SharpMusic.UI.Views
{
    public partial class MusicView : UserControl
    {
        public MusicView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}