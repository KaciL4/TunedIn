using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace TunedIn
{
    public partial class TuneInForm : Form
    {
        // NAudio Components for Playback
        private IWavePlayer waveOut;
        private AudioFileReader audioFile;
        private Song currentSong = null;

        // Playback State Management
        private int currentSongIndex = -1;
        private bool isPlaying = false;
        private bool isRepeatOn = false;

        public List<Song> currentQueue = new List<Song>();
        public List<Song> MusicLibrary = new List<Song>();
        public List<Playlist> Playlists = new List<Playlist>();
        private string currentSortColumn = "Title";
        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        public TuneInForm()
        {
            InitializeComponent();
            ShowView("library");
            dgvMusicLibrary.ColumnHeaderMouseClick += dgvMusicLibrary_ColumnHeaderMouseClick;

            // Set up the playback timer
            playbackTimer.Interval = 1000; // Update every 1000ms (1 second)
            playbackTimer.Tick += playbackTimer_Tick;

            // Hook up the FormClosing event for proper resource cleanup
            this.FormClosing += TuneInForm_FormClosing;
        }
        // to show the panel based on button clicked
        private void ShowView(string viewName)
        {
            musicLibraryViewPanel.Visible = false;
            playlistsViewPanel.Visible = false;
            nowPlayingViewPanel.Visible = false;


            switch (viewName.ToLower())
            {
                case "library":
                    musicLibraryViewPanel.Visible = true;
                    break;

                case "playlists":
                    playlistsViewPanel.Visible = true;
                    break;

                case "nowplaying":
                    nowPlayingViewPanel.Visible = true;
                    break;
            }
        }
        private void musicLibraryBtn_Click(object sender, EventArgs e)
        {
            ShowView("library");
        }
        private void playlistBtn_Click(object sender, EventArgs e)
        {
            ShowView("playlists");
        }
        private void nowPlayingBtn_Click(object sender, EventArgs e)
        {
            ShowView("nowplaying");
        }
        private void TuneInForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopPlayback();
        }
        // Cleanup method for NAudio components
        private void StopPlayback()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
            playbackTimer.Stop();
            isPlaying = false;
        }
        public void PlaySong(int index)
        {
            if (index < 0 || index >= currentQueue.Count)
            {
                StopPlayback();
                currentSong = null;
                currentSongIndex = -1;
                songInfoLabel.Text = "No Song Playing";
                playPauseButton.Text = "Play"; // Update button icon/text
                return;
            }

            StopPlayback(); // Stop any currently playing track

            try
            {
                currentSongIndex = index;
                currentSong = currentQueue[index];
                audioFile = new AudioFileReader(currentSong.FilePath);
                waveOut = new WaveOutEvent();
                waveOut.Init(audioFile);
                waveOut.PlaybackStopped += WaveOut_PlaybackStopped; // Hook up the event for song end

                // Apply volume setting
                waveOut.Volume = volumeTrackBar.Value / 100f;
                noMusicPlayingPanel.Visible = false;
                // Start playback
                waveOut.Play();
                isPlaying = true;
                //get the audio cover imgage
                Image albumArt = GetAlbumArt(currentSong.FilePath);
                if (albumArt != null)
                {
                    // ASSUMING PictureBox is named 'albumArtPictureBox'
                    albumArtPictureBox.Image = albumArt;
                }
                else
                {
                    // Clear the image or display a default "No Art" icon
                    albumArtPictureBox.Image = null;
                }

                // Update UI
                songInfoLabel.Text = $"{currentSong.Title} - {currentSong.Artist}";
                songInfoLabel1.Text = $"{currentSong.Title} - {currentSong.Artist}";
                playPauseButton.Text = "❚❚";//pause icon

                // Set up progress bar
                progressBarTrackBar.Maximum = (int)audioFile.TotalTime.TotalSeconds;
                timeTotalLabel.Text = currentSong.Duration;
                playbackTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing song: {ex.Message}", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopPlayback();
            }
        }

        // Logic for when a song finishes playing
        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            // Important: StopPlayback is called internally by WaveOutEvent
            // Check if the stopping was due to an error
            if (e.Exception != null)
            {
                MessageBox.Show($"Playback error: {e.Exception.Message}");
                return;
            }

            if (isRepeatOn)
            {
                // Restart the current song
                PlaySong(currentSongIndex);
            }
            else
            {
                // Play the next song in the queue
                currentSongIndex++;
                if (currentSongIndex < currentQueue.Count)
                {
                    PlaySong(currentSongIndex);
                }
                else
                {
                    // End of queue
                    StopPlayback();
                    songInfoLabel.Text = "Queue Finished";
                }
            }
        }
        private Song GetSongMetadata(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine($"File not found at: {filePath}");
                    return null;
                }

                // Create TagLib File object
                var tFile = TagLib.File.Create(filePath);
                TimeSpan duration = tFile.Properties.Duration;

                return new Song
                {
                    FilePath = filePath,
                    // Use ID3 Tag Title, but fall back to the filename if the tag is empty
                    Title = string.IsNullOrWhiteSpace(tFile.Tag.Title) ?
                            Path.GetFileNameWithoutExtension(filePath) :
                            tFile.Tag.Title,

                    // Fall back to "Unknown Artist"
                    Artist = tFile.Tag.Performers.Length > 0 ? string.Join(", ", tFile.Tag.Performers) : "Unknown Artist",
                    Album = tFile.Tag.Album,

                    // Format duration as M:SS (e.g., 2:05)
                    Duration = $"{(int)duration.TotalMinutes}:{duration.Seconds:D2}",
                    DurationTimeSpan = duration
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not read metadata for file: {Path.GetFileName(filePath)}. Error: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }
        private void UpdateMusicLibraryDisplay()
        {
            // Update the song count label
            musicLibraryCountLabel.Text = MusicLibrary.Count.ToString();
            musicLibraryCountLabel1.Text = MusicLibrary.Count.ToString();
            // Set up the DataGridView columns (only necessary the first time)
            if (dgvMusicLibrary.Columns.Count == 0)
            {
                dgvMusicLibrary.AutoGenerateColumns = false;
                dgvMusicLibrary.RowHeadersVisible = false;
                dgvMusicLibrary.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Add columns matching the Song properties
                dgvMusicLibrary.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Title",
                    HeaderText = "Title",
                    Width = 200
                });
                dgvMusicLibrary.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Artist",
                    HeaderText = "Artist",
                    Width = 150
                });
                dgvMusicLibrary.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Album",
                    HeaderText = "Album",
                    Width = 150
                });
                dgvMusicLibrary.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Duration",
                    HeaderText = "Duration",
                    Width = 70
                });
                // Hidden column for numeric sorting
                dgvMusicLibrary.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "DurationTimeSpan",
                    Visible = false
                });
            }

            // 3. Bind the list to the DataGridView
            dgvMusicLibrary.DataSource = null;
            dgvMusicLibrary.DataSource = MusicLibrary.ToList();
        }

        private void importSongBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Music Files|*.mp3;*.wav;*.flac;*.ogg|All Files|*.*";
                openFileDialog.Title = "Select Music Files to Import into TunedIn";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        // Prevent duplicate entries
                        if (!MusicLibrary.Any(s => s.FilePath == filePath))
                        {
                            Song song = GetSongMetadata(filePath);
                            if (song != null)
                            {
                                MusicLibrary.Add(song);
                            }
                        }
                    }

                    // Apply sorting and refresh the view
                    ApplyCurrentSort();
                    UpdateMusicLibraryDisplay();
                }
            }
        }
        // Helper method to apply the current sort state
        private void ApplyCurrentSort()
        {
            IEnumerable<Song> sortedList = MusicLibrary;

            if (currentSortColumn == "DurationTimeSpan" || currentSortColumn == "Duration")
            {
                // Sort by the TimeSpan property
                if (sortDirection == ListSortDirection.Ascending)
                    sortedList = MusicLibrary.OrderBy(s => s.DurationTimeSpan);
                else
                    sortedList = MusicLibrary.OrderByDescending(s => s.DurationTimeSpan);
            }
            else // Sort by string properties (Title, Artist, Album)
            {
                // Use Reflection to sort by the dynamic column name
                if (sortDirection == ListSortDirection.Ascending)
                    sortedList = MusicLibrary.OrderBy(s => s.GetType().GetProperty(currentSortColumn)?.GetValue(s, null));
                else
                    sortedList = MusicLibrary.OrderByDescending(s => s.GetType().GetProperty(currentSortColumn)?.GetValue(s, null));
            }

            MusicLibrary = sortedList.ToList();
        }

        // Event handler for clicking a column header
        private void dgvMusicLibrary_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Use the DataPropertyName for internal sorting logic
            string clickedColumnDataProperty = dgvMusicLibrary.Columns[e.ColumnIndex].DataPropertyName;

            // 1. Determine the new sort direction
            if (clickedColumnDataProperty == currentSortColumn)
            {
                // Toggle direction if the same column is clicked
                sortDirection = (sortDirection == ListSortDirection.Ascending) ?
                                ListSortDirection.Descending :
                                ListSortDirection.Ascending;
            }
            else
            {
                // New column clicked: reset to ascending
                currentSortColumn = clickedColumnDataProperty;
                sortDirection = ListSortDirection.Ascending;
            }
            // Apply the sorting
            ApplyCurrentSort();

            // Rebind the DataGridView with the newly sorted list
            UpdateMusicLibraryDisplay();

            // Update the visual sort arrow (glyph)
            foreach (DataGridViewColumn col in dgvMusicLibrary.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            dgvMusicLibrary.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection =
                (sortDirection == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
        }
        //-------------- Player Control Panel Methods----------------
        private void playPauseButton_Click(object sender, EventArgs e)
        {
            if (currentSong == null && MusicLibrary.Any())
            {
                // If nothing is loaded, set the queue to the full library and start at index 0
                currentQueue = MusicLibrary.ToList();
                PlaySong(0);
            }
            else if (waveOut != null)
            {
                if (isPlaying)
                {
                    waveOut.Pause();
                    isPlaying = false;
                    playbackTimer.Stop();
                    playPauseButton.Text = "▶";//play icon
                }
                else
                {
                    waveOut.Play();
                    isPlaying = true;
                    playbackTimer.Start();
                    playPauseButton.Text = "❚❚";//pause icon
                }
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (currentSong == null || currentSongIndex == -1) return;

            // Stop playback immediately if we're at the end of the queue
            if (currentSongIndex >= currentQueue.Count - 1)
            {
                StopPlayback();
                songInfoLabel.Text = "Queue Finished";
                return;
            }

            PlaySong(currentSongIndex + 1);
        }

        private void repeatButton_Click(object sender, EventArgs e)
        {
            isRepeatOn = !isRepeatOn;
            // Visually indicate repeat is on (e.g., change button color/icon)
            repeatButton.BackColor = isRepeatOn ? Color.MediumPurple : SystemColors.Control;
        }

        private void volumeTrackBar_Scroll(object sender, EventArgs e)
        {
            float volume = volumeTrackBar.Value / 100f; // TrackBar max is 100, NAudio wants 0.0 to 1.0
            if (waveOut != null)
            {
                waveOut.Volume = volume;
            }
        }

        private void playbackTimer_Tick(object sender, EventArgs e)
        {
            if (audioFile != null && waveOut != null && isPlaying)
            {
                int currentSeconds = (int)audioFile.CurrentTime.TotalSeconds;

                // Update TrackBar position
                progressBarTrackBar.Value = Math.Min(currentSeconds, progressBarTrackBar.Maximum);

                // Update time elapsed label (format M:SS)
                TimeSpan currentTime = audioFile.CurrentTime;
                timeElapsedLabel.Text = $"{(int)currentTime.TotalMinutes}:{currentTime.Seconds:D2}";
            }
        }

        private void progressBarTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            playbackTimer.Stop();
        }

        private void progressBarTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (audioFile != null)
            {
                // Set the audio file position based on the TrackBar value (in seconds)
                audioFile.CurrentTime = TimeSpan.FromSeconds(progressBarTrackBar.Value);
            }
            if (isPlaying)
            {
                playbackTimer.Start();
            }
        }
        //--------------Playlist Panel Methods----------------
        private void UpdatePlaylistsListBox()
        {
            playlistsListBox.DataSource = null;
            playlistsListBox.DisplayMember = "Name";
            playlistsListBox.DataSource = Playlists;
            playlistCountLabel.Text = Playlists.Count.ToString();
        }
        private void newPlaylistButton_Click(object sender, EventArgs e)
        {
            using (var form = new CreatePlaylistForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string newName = form.PlaylistName;

                    // Check for duplicate names
                    if (Playlists.Any(p => p.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("A playlist with this name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Create and add the new playlist
                    Playlist newPlaylist = new Playlist(newName);
                    Playlists.Add(newPlaylist);

                    // Refresh the ListBox
                    UpdatePlaylistsListBox();

                    // Select the newly created playlist
                    playlistsListBox.SelectedItem = newPlaylist;
                }
            }
        }
       
        //method to display songs of selected playlist
        private void playlistsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playlistsListBox.SelectedItem is Playlist selectedPlaylist)
            {
                //Hide the "No Playlist Selected" message
                noPlaylistSelectedPanel.Visible= false;

                //Update the header labels
                playlistNameLabel.Text = selectedPlaylist.Name;

                // Bind the selected playlist's song list to the DataGridView
                dgvPlaylistSongs.DataSource =null;
                dgvPlaylistSongs.DataSource =selectedPlaylist.Songs;

                // Set up the DataGridView columns (similar to dgvMusicLibrary)
                if (dgvPlaylistSongs.Columns.Count == 0)
                {
                    dgvPlaylistSongs.AutoGenerateColumns = false;
                    dgvPlaylistSongs.RowHeadersVisible = false;
                    dgvPlaylistSongs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Add columns matching the Song properties
                    dgvPlaylistSongs.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Title",
                        HeaderText = "Title",
                        Width = 200
                    });
                    dgvPlaylistSongs.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Artist",
                        HeaderText = "Artist",
                        Width = 150
                    });
                    dgvPlaylistSongs.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Duration",
                        HeaderText = "Duration",
                        Width = 70
                    });
                }
            }
            else
            {
                // Show the "No Playlist Selected" message if the selection is cleared
                noPlaylistSelectedPanel.Visible = true;
            }
        }

        private void playAllButton_Click(object sender, EventArgs e)
        {
            if (playlistsListBox.SelectedItem is Playlist selectedPlaylist)
            {
                if (selectedPlaylist.Songs.Any())
                {
                    currentQueue = selectedPlaylist.Songs.ToList();
                    PlaySong(0);// Start playing from the first song
                }
                else
                {
                    MessageBox.Show("This playlist is empty. Please add songs first.", "Cannot Play", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void addSongsToPlaylistButton_Click(object sender, EventArgs e)
        {
            if (playlistsListBox.SelectedItem is Playlist selectedPlaylist)
            {
                using (var form = new SelectSongsForm(this.MusicLibrary))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int songsAdded = 0;

                        foreach (Song songToAdd in form.SelectedSongs)
                        {
                            // Check if the song is ALREADY in the playlist before adding
                            if (!selectedPlaylist.Songs.Any(s => s.FilePath == songToAdd.FilePath))
                            {
                                selectedPlaylist.Songs.Add(songToAdd);
                                songsAdded++;
                            }
                        }

                        if (songsAdded > 0)
                        {
                            // Rebind the DataGridView to show the new songs
                            dgvPlaylistSongs.DataSource = null;
                            dgvPlaylistSongs.DataSource = selectedPlaylist.Songs;

                            // Optional: Give feedback to the user
                            MessageBox.Show($"{songsAdded} song(s) added to '{selectedPlaylist.Name}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No new songs were selected or all selected songs were duplicates.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a playlist first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //-------------- Now Playing Panel Methods----------------
        private Image GetAlbumArt(string filePath)
        {
            try
            {
                using (var tFile = TagLib.File.Create(filePath))
                {
                    // Check if there are any embedded pictures (album art)
                    if (tFile.Tag.Pictures.Length > 0)
                    {
                        var picture = tFile.Tag.Pictures[0]; // Get the first picture

                        // Use MemoryStream to convert the byte data to a System.Drawing.Image
                        using (var ms = new MemoryStream(picture.Data.Data))
                        {
                            // We return a new Bitmap to ensure the image stream is not locked 
                            // after the MemoryStream is disposed.
                            return new Bitmap(ms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle any error during image extraction
                System.Diagnostics.Debug.WriteLine($"Error retrieving album art for {Path.GetFileName(filePath)}: {ex.Message}");
            }

            // Return null if no art is found or an error occurred
            return Properties.Resources.audio; ;
        }

        private void languageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var changeLanguage = new ChangeLanguage();
            switch (languageComboBox.SelectedIndex)
            {
                case 0: changeLanguage.UpdateConfig("language", "en");
                    Application.Restart();
                    break;
                case 1:
                    changeLanguage.UpdateConfig("language", "fr-FR");
                    Application.Restart();
                    break;
                case 2:
                    changeLanguage.UpdateConfig("language", "es-ES");
                    Application.Restart();
                    break;
            }
        }
    }
}
