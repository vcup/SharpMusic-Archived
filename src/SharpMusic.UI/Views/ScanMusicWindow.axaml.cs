using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SharpMusic.UI.ViewModels;

namespace SharpMusic.UI.Views
{
    public partial class ScanMusicWindow : ReactiveWindow<ScanMusicViewModel>
    {
        public ScanMusicWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(b => b(ViewModel!.Enter.Subscribe(Close)));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}