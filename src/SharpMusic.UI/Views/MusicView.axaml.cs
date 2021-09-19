using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SharpMusic.UI.ViewModels;

namespace SharpMusic.UI.Views
{
    public partial class MusicView : ReactiveUserControl<MusicViewModel>
    {
        private Border _addButton;
        public MusicView()
        {
            InitializeComponent();
            _addButton = this.Get<Border>("AddButton");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnPointerEnterButton(object? sender, PointerEventArgs e)
        {
            _addButton.IsVisible = true;
            _addButton.Background = Brushes.Gray;
            _addButton.Opacity = 0.8;
        }

        private void OnPointerLeaveButton(object? sender, PointerEventArgs e)
        {
            _addButton.Background = Brushes.Black;
            _addButton.Opacity = 0.5;
        }

        private void OnPointerEnter(object? sender, PointerEventArgs e) => _addButton.IsVisible = true;

        private void OnPointerLeave(object? sender, PointerEventArgs e) => _addButton.IsVisible = false;
    }
}