using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SharpMusic.UI.ViewModels;

namespace SharpMusic.UI.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(b => b(ViewModel!.ShowScanMusic.RegisterHandler(DoShowScanMusicWindow)));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task DoShowScanMusicWindow(InteractionContext<ScanMusicViewModel, IEnumerable<MusicViewModel>?> interaction)
        {
            var dialog = new ScanMusicWindow() {DataContext = interaction.Input};
            var result = await dialog.ShowDialog<IEnumerable<MusicViewModel>>(this);
            interaction.SetOutput(result);
        }
    }
}