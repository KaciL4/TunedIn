using System;
using System.ComponentModel;
using Avalonia.Media.Imaging;

namespace TunedIn.Models
{
    public class Song : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Guid Id { get; set; } = Guid.NewGuid();

        private string _filePath = string.Empty;
        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(nameof(FilePath)); }
        }

        private string _title = "Unknown Title";
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _artist = "Unknown Artist";
        public string Artist
        {
            get => _artist;
            set { _artist = value; OnPropertyChanged(nameof(Artist)); }
        }

        private string _album = "Unknown Album";
        public string Album
        {
            get => _album;
            set { _album = value; OnPropertyChanged(nameof(Album)); }
        }

        private TimeSpan _duration = TimeSpan.Zero;
        public TimeSpan Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(nameof(Duration)); OnPropertyChanged(nameof(DurationString)); }
        }

        // Display helper
        public string DurationString => $"{Duration.Minutes:D2}:{Duration.Seconds:D2}";

        // Optional album artwork (may be null)
        private Bitmap? _artwork;
        public Bitmap? Artwork
        {
            get => _artwork;
            set { _artwork = value; OnPropertyChanged(nameof(Artwork)); }
        }
    }
}