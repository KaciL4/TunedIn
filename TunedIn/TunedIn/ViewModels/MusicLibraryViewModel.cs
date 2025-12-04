using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using ReactiveUI;
using TagLib;
using TagLibFile = TagLib.File;
using TunedIn.Models;
using Avalonia.Platform.Storage;

namespace TunedIn.ViewModels
{
    public class MusicLibraryViewModel : ViewModelBase
    {
        private const string AppFolderName = "TunedIn";
        private const string SongsFileName = "songs.json";
        private const string SettingsFileName = "settings.json";

        public ObservableCollection<Song> Songs { get; } = new ObservableCollection<Song>();
        public ObservableCollection<Playlist> Playlists { get; } = new ObservableCollection<Playlist>();

        public ReactiveCommand<Unit, Unit> ImportSongsCommand { get; }
        public ReactiveCommand<Unit, Unit> PlayPauseCommand { get; }
        public ReactiveCommand<Unit, Unit> NextCommand { get; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleRepeatCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleMuteCommand { get; }

        private Song? _selectedSong;
        public Song? SelectedSong
        {
            get => _selectedSong;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSong, value);
                this.RaisePropertyChanged(nameof(NowPlayingTitle));
                this.RaisePropertyChanged(nameof(NowPlayingSubtitle));
                this.RaisePropertyChanged(nameof(NowPlayingDurationTotalSeconds));
                this.RaisePropertyChanged(nameof(DurationString));
            }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                this.RaiseAndSetIfChanged(ref _isPlaying, value);
                this.RaisePropertyChanged(nameof(PlayPauseIcon));
            }
        }

        private bool _repeatActive;
        public bool RepeatActive
        {
            get => _repeatActive;
            set => this.RaiseAndSetIfChanged(ref _repeatActive, value);
        }

        private TimeSpan _position = TimeSpan.Zero;
        public TimeSpan Position
        {
            get => _position;
            private set
            {
                this.RaiseAndSetIfChanged(ref _position, value);
                this.RaisePropertyChanged(nameof(PositionString));
                this.RaisePropertyChanged(nameof(PositionSeconds));
            }
        }

        private double _volume = 0.6;
        private double _previousVolume = 0.6;
        private bool _isMuted = false;

        public double Volume
        {
            get => _volume;
            set
            {
                var v = Math.Clamp(value, 0.0, 1.0);
                if (Math.Abs(v - _volume) < 0.0001)
                    return;

                this.RaiseAndSetIfChanged(ref _volume, v);

                if (v > 0 && IsMuted)
                {
                    IsMuted = false;
                }

                SaveSettings();
            }
        }

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                if (value == _isMuted) return;

                if (value)
                {
                    _previousVolume = _volume;
                    Volume = 0;
                }
                else
                {
                    Volume = _previousVolume > 0 ? _previousVolume : 0.6;
                }

                this.RaiseAndSetIfChanged(ref _isMuted, value);
                SaveSettings();
            }
        }

        public double PositionSeconds
        {
            get => Position.TotalSeconds;
            set
            {
                var dur = SelectedSong?.Duration.TotalSeconds ?? 1;
                var seconds = Math.Max(0, Math.Min(value, dur));
                Position = TimeSpan.FromSeconds(seconds);
            }
        }

        public string PositionString => $"{(int)Position.TotalMinutes:D2}:{Position.Seconds:D2}";
        public double NowPlayingDurationTotalSeconds => SelectedSong?.Duration.TotalSeconds ?? 1;
        public string DurationString => SelectedSong?.Duration is { } d ? $"{(int)d.TotalMinutes:D2}:{d.Seconds:D2}" : "00:00";

        public string NowPlayingTitle => SelectedSong?.Title ?? "No song selected";
        public string NowPlayingSubtitle => SelectedSong != null ? (SelectedSong.Artist ?? "") : "Select a song to start playing";
        public string PlayPauseIcon => IsPlaying ? "⏸" : "▶";

        private CancellationTokenSource? _playbackCts;
        private readonly List<Song> _playQueue = new List<Song>();
        private int _playIndex = -1;

        public MusicLibraryViewModel()
        {
            // Create commands with MainThreadScheduler to avoid threading issues
            ImportSongsCommand = ReactiveCommand.CreateFromTask(
                ImportSongsAsync,
                outputScheduler: RxApp.MainThreadScheduler);

            PlayPauseCommand = ReactiveCommand.CreateFromTask(
                PlayPauseAsync,
                outputScheduler: RxApp.MainThreadScheduler);

            NextCommand = ReactiveCommand.CreateFromTask(
                NextAsync,
                outputScheduler: RxApp.MainThreadScheduler);

            PreviousCommand = ReactiveCommand.CreateFromTask(
                PreviousAsync,
                outputScheduler: RxApp.MainThreadScheduler);

            ToggleRepeatCommand = ReactiveCommand.Create(
                () => { RepeatActive = !RepeatActive; },
                outputScheduler: RxApp.MainThreadScheduler);

            ToggleMuteCommand = ReactiveCommand.Create(
                () => { IsMuted = !IsMuted; },
                outputScheduler: RxApp.MainThreadScheduler);

            // Load on UI thread
            Dispatcher.UIThread.Post(() =>
            {
                LoadSettings();
                LoadSavedSongs();
            }, DispatcherPriority.Background);
        }

        private string GetAppDataPath()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appPath = Path.Combine(basePath, AppFolderName);
            Directory.CreateDirectory(appPath);
            return appPath;
        }

        private string SongsFilePath => Path.Combine(GetAppDataPath(), SongsFileName);
        private string SettingsFilePath => Path.Combine(GetAppDataPath(), SettingsFileName);

        private void SaveSongs()
        {
            try
            {
                var persistent = Songs.Select(s => new PersistedSong { Id = s.Id, FilePath = s.FilePath }).ToList();
                var json = JsonSerializer.Serialize(persistent, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(SongsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving songs: {ex.Message}");
            }
        }

        private void LoadSavedSongs()
        {
            try
            {
                if (!System.IO.File.Exists(SongsFilePath))
                    return;

                var json = System.IO.File.ReadAllText(SongsFilePath);
                var persisted = JsonSerializer.Deserialize<List<PersistedSong>>(json) ?? new List<PersistedSong>();

                foreach (var p in persisted)
                {
                    if (System.IO.File.Exists(p.FilePath))
                    {
                        try
                        {
                            using var fs = System.IO.File.Open(p.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                            TagLib.File.IFileAbstraction abstraction = new AvaloniaFileAbstraction(p.FilePath, fs);
                            using var tfile = TagLibFile.Create(abstraction, ReadStyle.Average);
                            var song = new Song
                            {
                                Id = p.Id,
                                FilePath = p.FilePath,
                                Title = string.IsNullOrWhiteSpace(tfile.Tag.Title) ? Path.GetFileNameWithoutExtension(p.FilePath) : tfile.Tag.Title,
                                Artist = tfile.Tag.Performers?.FirstOrDefault() ?? "Unknown Artist",
                                Album = tfile.Tag.Album ?? "Unknown Album",
                                Duration = tfile.Properties.Duration
                            };

                            // Ensure we're on UI thread when adding to collection
                            if (Dispatcher.UIThread.CheckAccess())
                            {
                                Songs.Add(song);
                            }
                            else
                            {
                                Dispatcher.UIThread.Post(() => Songs.Add(song), DispatcherPriority.Background);
                            }
                        }
                        catch
                        {
                            var song = new Song
                            {
                                Id = p.Id,
                                FilePath = p.FilePath,
                                Title = Path.GetFileNameWithoutExtension(p.FilePath),
                                Artist = "Unknown Artist"
                            };

                            if (Dispatcher.UIThread.CheckAccess())
                            {
                                Songs.Add(song);
                            }
                            else
                            {
                                Dispatcher.UIThread.Post(() => Songs.Add(song), DispatcherPriority.Background);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading saved songs: {ex.Message}");
            }
        }

        private async Task ImportSongsAsync()
        {
            try
            {
                var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                var mainWindow = lifetime?.MainWindow;
                if (mainWindow == null)
                {
                    Console.WriteLine("MainWindow is null");
                    return;
                }

                // Ensure we're on the UI thread
                IReadOnlyList<IStorageFile>? files = null;

                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    try
                    {
                        files = await mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                        {
                            Title = "Select Music Files to Import",
                            AllowMultiple = true,
                            FileTypeFilter = new List<FilePickerFileType>
                            {
                                new FilePickerFileType("Audio Files")
                                {
                                    Patterns = new[] { "*.mp3", "*.flac", "*.ogg", "*.wav", "*.m4a" }
                                },
                                FilePickerFileTypes.All
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"FilePicker error: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
                });

                if (files == null || files.Count == 0)
                {
                    Console.WriteLine("No files selected");
                    return;
                }

                Console.WriteLine($"Selected {files.Count} file(s)");

                var filePaths = new List<string>();
                foreach (var file in files)
                {
                    try
                    {
                        var path = file.TryGetLocalPath();
                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            filePaths.Add(path);
                            Console.WriteLine($"Added file: {path}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error accessing file path: {ex.Message}");
                    }
                }

                if (filePaths.Count > 0)
                {
                    await ImportFromFilesAsync(filePaths);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ImportSongsAsync fatal error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        public async Task ImportFromFilesAsync(IEnumerable<string>? filePaths)
        {
            if (filePaths == null) return;

            var imported = 0;

            foreach (var filePath in filePaths)
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    continue;

                try
                {
                    if (Songs.Any(s => string.Equals(s.FilePath, filePath, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    using var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    TagLib.File.IFileAbstraction abstraction = new AvaloniaFileAbstraction(filePath, stream);
                    using var tFile = TagLibFile.Create(abstraction, ReadStyle.Average);

                    var newSong = new Song
                    {
                        FilePath = filePath,
                        Title = string.IsNullOrWhiteSpace(tFile.Tag.Title) ? Path.GetFileNameWithoutExtension(filePath) : tFile.Tag.Title,
                        Artist = tFile.Tag.Performers?.FirstOrDefault() ?? "Unknown Artist",
                        Album = tFile.Tag.Album ?? "Unknown Album",
                        Duration = tFile.Properties.Duration
                    };

                    // Add song on UI thread
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Songs.Add(newSong);
                    });

                    imported++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error importing file {filePath}: {ex.Message}");
                }
            }

            if (imported > 0)
            {
                SaveSongs();
                Console.WriteLine($"Added {imported} song(s) to library.");
            }

            await Task.CompletedTask;
        }

        public async Task PlaySongAsync(Song song)
        {
            if (song == null)
                return;

            _playQueue.Clear();
            _playQueue.AddRange(Songs);
            _playIndex = _playQueue.IndexOf(song);

            SelectedSong = song;
            Position = TimeSpan.Zero;
            await StartPlaybackAsync();
        }

        private async Task PlayPauseAsync()
        {
            if (IsPlaying)
            {
                StopPlayback();
            }
            else
            {
                if (SelectedSong == null && Songs.Any())
                {
                    await PlaySongAsync(Songs.First());
                    return;
                }

                await StartPlaybackAsync();
            }
        }

        private async Task StartPlaybackAsync()
        {
            if (SelectedSong == null)
                return;

            IsPlaying = true;
            _playbackCts?.Cancel();
            _playbackCts = new CancellationTokenSource();

            var ct = _playbackCts.Token;
            var duration = SelectedSong.Duration;
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    await Task.Delay(1000, ct);
                    if (ct.IsCancellationRequested) break;

                    Position = Position + TimeSpan.FromSeconds(1);
                    if (Position >= duration)
                    {
                        if (RepeatActive)
                        {
                            Position = TimeSpan.Zero;
                            continue;
                        }
                        else
                        {
                            await NextAsync();
                            break;
                        }
                    }
                }
            }
            catch (TaskCanceledException) { }
        }

        private void StopPlayback()
        {
            _playbackCts?.Cancel();
            IsPlaying = false;
        }

        private Task NextAsync()
        {
            if (_playQueue.Count == 0)
                return Task.CompletedTask;

            _playIndex++;
            if (_playIndex >= _playQueue.Count)
            {
                _playIndex = 0;
                StopPlayback();
                SelectedSong = _playQueue.FirstOrDefault();
                Position = TimeSpan.Zero;
                return Task.CompletedTask;
            }

            SelectedSong = _playQueue[_playIndex];
            Position = TimeSpan.Zero;
            return StartPlaybackAsync();
        }

        private Task PreviousAsync()
        {
            if (_playQueue.Count == 0)
                return Task.CompletedTask;

            if (Position.TotalSeconds > 3)
            {
                Position = TimeSpan.Zero;
                return Task.CompletedTask;
            }

            _playIndex--;
            if (_playIndex < 0)
                _playIndex = 0;

            SelectedSong = _playQueue[_playIndex];
            Position = TimeSpan.Zero;
            return StartPlaybackAsync();
        }

        private void SaveSettings()
        {
            try
            {
                var settings = new AppSettings
                {
                    Volume = _volume,
                    IsMuted = _isMuted,
                    RepeatActive = _repeatActive
                };
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        private void LoadSettings()
        {
            try
            {
                if (!System.IO.File.Exists(SettingsFilePath))
                    return;

                var json = System.IO.File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);

                if (settings != null)
                {
                    _volume = Math.Clamp(settings.Volume, 0.0, 1.0);
                    _isMuted = settings.IsMuted;
                    _repeatActive = settings.RepeatActive;

                    this.RaisePropertyChanged(nameof(Volume));
                    this.RaisePropertyChanged(nameof(IsMuted));
                    this.RaisePropertyChanged(nameof(RepeatActive));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
        }

        private class PersistedSong
        {
            public Guid Id { get; set; }
            public string FilePath { get; set; } = string.Empty;
        }

        private class AppSettings
        {
            public double Volume { get; set; } = 0.6;
            public bool IsMuted { get; set; }
            public bool RepeatActive { get; set; }
        }
    }
}