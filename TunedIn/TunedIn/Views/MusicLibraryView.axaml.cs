using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TunedIn.Views
{
    public partial class MusicLibraryView : UserControl
    {
        public MusicLibraryView()
        {
            InitializeComponent();
            // Do NOT set DataContext here — DataTemplate provides the VM.
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}