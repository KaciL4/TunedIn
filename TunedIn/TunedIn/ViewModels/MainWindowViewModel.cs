using ReactiveUI;

namespace TunedIn.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase? _contentViewModel;

        public MainWindowViewModel()
        {
            ContentViewModel = new MusicLibraryViewModel();
        }

        public ViewModelBase? ContentViewModel
        {
            get => _contentViewModel;
            set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }
    }
}