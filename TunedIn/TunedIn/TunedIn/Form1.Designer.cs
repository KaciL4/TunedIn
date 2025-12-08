namespace TunedIn
{
    partial class TuneInForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TuneInForm));
            this.sideBarPanel = new System.Windows.Forms.Panel();
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.playlistCountLabel = new System.Windows.Forms.Label();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.musicLibraryCountLabel = new System.Windows.Forms.Label();
            this.commentlabel = new System.Windows.Forms.Label();
            this.nowPlayingBtn = new System.Windows.Forms.Button();
            this.playlistBtn = new System.Windows.Forms.Button();
            this.musicLibraryBtn = new System.Windows.Forms.Button();
            this.playerControlPanel = new System.Windows.Forms.Panel();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.timeElapsedLabel = new System.Windows.Forms.Label();
            this.timeTotalLabel = new System.Windows.Forms.Label();
            this.progressBarTrackBar = new System.Windows.Forms.TrackBar();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.nextButton = new System.Windows.Forms.Button();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.repeatButton = new System.Windows.Forms.Button();
            this.songInfoLabel = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.nowPlayingViewPanel = new System.Windows.Forms.Panel();
            this.noMusicPlayingPanel = new System.Windows.Forms.Panel();
            this.noMusicLabel = new System.Windows.Forms.Label();
            this.songInfoLabel1 = new System.Windows.Forms.Label();
            this.albumArtPictureBox = new System.Windows.Forms.PictureBox();
            this.playlistsViewPanel = new System.Windows.Forms.Panel();
            this.playlistDetailPanel = new System.Windows.Forms.Panel();
            this.noPlaylistSelectedPanel = new System.Windows.Forms.Panel();
            this.noSelectedPlaylistLb = new System.Windows.Forms.Label();
            this.dgvPlaylistSongs = new System.Windows.Forms.DataGridView();
            this.addSongsToPlaylistButton = new System.Windows.Forms.Button();
            this.playAllButton = new System.Windows.Forms.Button();
            this.playlistNameLabel = new System.Windows.Forms.Label();
            this.playlistListPanel = new System.Windows.Forms.Panel();
            this.playlistsListBox = new System.Windows.Forms.ListBox();
            this.newPlaylistButton = new System.Windows.Forms.Button();
            this.playlistTitleLabel = new System.Windows.Forms.Label();
            this.musicLibraryViewPanel = new System.Windows.Forms.Panel();
            this.infoForSongCountLabel = new System.Windows.Forms.Label();
            this.musicLibraryCountLabel1 = new System.Windows.Forms.Label();
            this.musicLibLabel = new System.Windows.Forms.Label();
            this.importSongBtn = new System.Windows.Forms.Button();
            this.dgvMusicLibrary = new System.Windows.Forms.DataGridView();
            this.playbackTimer = new System.Windows.Forms.Timer(this.components);
            this.sideBarPanel.SuspendLayout();
            this.playerControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.nowPlayingViewPanel.SuspendLayout();
            this.noMusicPlayingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPictureBox)).BeginInit();
            this.playlistsViewPanel.SuspendLayout();
            this.playlistDetailPanel.SuspendLayout();
            this.noPlaylistSelectedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistSongs)).BeginInit();
            this.playlistListPanel.SuspendLayout();
            this.musicLibraryViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusicLibrary)).BeginInit();
            this.SuspendLayout();
            // 
            // sideBarPanel
            // 
            this.sideBarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.sideBarPanel.Controls.Add(this.languageLabel);
            this.sideBarPanel.Controls.Add(this.languageComboBox);
            this.sideBarPanel.Controls.Add(this.playlistCountLabel);
            this.sideBarPanel.Controls.Add(this.subtitleLabel);
            this.sideBarPanel.Controls.Add(this.titleLabel);
            this.sideBarPanel.Controls.Add(this.musicLibraryCountLabel);
            this.sideBarPanel.Controls.Add(this.commentlabel);
            this.sideBarPanel.Controls.Add(this.nowPlayingBtn);
            this.sideBarPanel.Controls.Add(this.playlistBtn);
            this.sideBarPanel.Controls.Add(this.musicLibraryBtn);
            resources.ApplyResources(this.sideBarPanel, "sideBarPanel");
            this.sideBarPanel.Name = "sideBarPanel";
            // 
            // languageLabel
            // 
            resources.ApplyResources(this.languageLabel, "languageLabel");
            this.languageLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.languageLabel.Name = "languageLabel";
            // 
            // languageComboBox
            // 
            this.languageComboBox.BackColor = System.Drawing.Color.White;
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.languageComboBox, "languageComboBox");
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Items.AddRange(new object[] {
            resources.GetString("languageComboBox.Items"),
            resources.GetString("languageComboBox.Items1"),
            resources.GetString("languageComboBox.Items2")});
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.languageComboBox_SelectedIndexChanged);
            // 
            // playlistCountLabel
            // 
            resources.ApplyResources(this.playlistCountLabel, "playlistCountLabel");
            this.playlistCountLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.playlistCountLabel.Name = "playlistCountLabel";
            // 
            // subtitleLabel
            // 
            resources.ApplyResources(this.subtitleLabel, "subtitleLabel");
            this.subtitleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.subtitleLabel.Name = "subtitleLabel";
            // 
            // titleLabel
            // 
            resources.ApplyResources(this.titleLabel, "titleLabel");
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.titleLabel.Name = "titleLabel";
            // 
            // musicLibraryCountLabel
            // 
            resources.ApplyResources(this.musicLibraryCountLabel, "musicLibraryCountLabel");
            this.musicLibraryCountLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.musicLibraryCountLabel.Name = "musicLibraryCountLabel";
            // 
            // commentlabel
            // 
            resources.ApplyResources(this.commentlabel, "commentlabel");
            this.commentlabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.commentlabel.Name = "commentlabel";
            // 
            // nowPlayingBtn
            // 
            resources.ApplyResources(this.nowPlayingBtn, "nowPlayingBtn");
            this.nowPlayingBtn.BackColor = System.Drawing.Color.Black;
            this.nowPlayingBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.nowPlayingBtn.Name = "nowPlayingBtn";
            this.nowPlayingBtn.UseVisualStyleBackColor = false;
            this.nowPlayingBtn.Click += new System.EventHandler(this.nowPlayingBtn_Click);
            // 
            // playlistBtn
            // 
            resources.ApplyResources(this.playlistBtn, "playlistBtn");
            this.playlistBtn.BackColor = System.Drawing.Color.Black;
            this.playlistBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.playlistBtn.Name = "playlistBtn";
            this.playlistBtn.UseVisualStyleBackColor = false;
            this.playlistBtn.Click += new System.EventHandler(this.playlistBtn_Click);
            // 
            // musicLibraryBtn
            // 
            resources.ApplyResources(this.musicLibraryBtn, "musicLibraryBtn");
            this.musicLibraryBtn.BackColor = System.Drawing.Color.Black;
            this.musicLibraryBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.musicLibraryBtn.Name = "musicLibraryBtn";
            this.musicLibraryBtn.UseVisualStyleBackColor = false;
            this.musicLibraryBtn.Click += new System.EventHandler(this.musicLibraryBtn_Click);
            // 
            // playerControlPanel
            // 
            this.playerControlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.playerControlPanel.Controls.Add(this.volumeLabel);
            this.playerControlPanel.Controls.Add(this.timeElapsedLabel);
            this.playerControlPanel.Controls.Add(this.timeTotalLabel);
            this.playerControlPanel.Controls.Add(this.progressBarTrackBar);
            this.playerControlPanel.Controls.Add(this.volumeTrackBar);
            this.playerControlPanel.Controls.Add(this.nextButton);
            this.playerControlPanel.Controls.Add(this.playPauseButton);
            this.playerControlPanel.Controls.Add(this.previousButton);
            this.playerControlPanel.Controls.Add(this.repeatButton);
            this.playerControlPanel.Controls.Add(this.songInfoLabel);
            resources.ApplyResources(this.playerControlPanel, "playerControlPanel");
            this.playerControlPanel.Name = "playerControlPanel";
            // 
            // volumeLabel
            // 
            resources.ApplyResources(this.volumeLabel, "volumeLabel");
            this.volumeLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.volumeLabel.Name = "volumeLabel";
            // 
            // timeElapsedLabel
            // 
            resources.ApplyResources(this.timeElapsedLabel, "timeElapsedLabel");
            this.timeElapsedLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.timeElapsedLabel.Name = "timeElapsedLabel";
            // 
            // timeTotalLabel
            // 
            resources.ApplyResources(this.timeTotalLabel, "timeTotalLabel");
            this.timeTotalLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.timeTotalLabel.Name = "timeTotalLabel";
            // 
            // progressBarTrackBar
            // 
            resources.ApplyResources(this.progressBarTrackBar, "progressBarTrackBar");
            this.progressBarTrackBar.Name = "progressBarTrackBar";
            this.progressBarTrackBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.progressBarTrackBar_MouseDown);
            this.progressBarTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.progressBarTrackBar_MouseUp);
            // 
            // volumeTrackBar
            // 
            resources.ApplyResources(this.volumeTrackBar, "volumeTrackBar");
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Scroll += new System.EventHandler(this.volumeTrackBar_Scroll);
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.nextButton, "nextButton");
            this.nextButton.Name = "nextButton";
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // playPauseButton
            // 
            this.playPauseButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.playPauseButton, "playPauseButton");
            this.playPauseButton.Name = "playPauseButton";
            this.playPauseButton.UseVisualStyleBackColor = false;
            this.playPauseButton.Click += new System.EventHandler(this.playPauseButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.previousButton, "previousButton");
            this.previousButton.Name = "previousButton";
            this.previousButton.UseVisualStyleBackColor = false;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // repeatButton
            // 
            this.repeatButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.repeatButton, "repeatButton");
            this.repeatButton.Name = "repeatButton";
            this.repeatButton.UseVisualStyleBackColor = false;
            this.repeatButton.Click += new System.EventHandler(this.repeatButton_Click);
            // 
            // songInfoLabel
            // 
            resources.ApplyResources(this.songInfoLabel, "songInfoLabel");
            this.songInfoLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.songInfoLabel.Name = "songInfoLabel";
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.nowPlayingViewPanel);
            this.contentPanel.Controls.Add(this.playlistsViewPanel);
            this.contentPanel.Controls.Add(this.musicLibraryViewPanel);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            this.contentPanel.Name = "contentPanel";
            // 
            // nowPlayingViewPanel
            // 
            this.nowPlayingViewPanel.Controls.Add(this.noMusicPlayingPanel);
            this.nowPlayingViewPanel.Controls.Add(this.songInfoLabel1);
            this.nowPlayingViewPanel.Controls.Add(this.albumArtPictureBox);
            resources.ApplyResources(this.nowPlayingViewPanel, "nowPlayingViewPanel");
            this.nowPlayingViewPanel.Name = "nowPlayingViewPanel";
            // 
            // noMusicPlayingPanel
            // 
            this.noMusicPlayingPanel.Controls.Add(this.noMusicLabel);
            resources.ApplyResources(this.noMusicPlayingPanel, "noMusicPlayingPanel");
            this.noMusicPlayingPanel.Name = "noMusicPlayingPanel";
            // 
            // noMusicLabel
            // 
            resources.ApplyResources(this.noMusicLabel, "noMusicLabel");
            this.noMusicLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.noMusicLabel.Name = "noMusicLabel";
            // 
            // songInfoLabel1
            // 
            resources.ApplyResources(this.songInfoLabel1, "songInfoLabel1");
            this.songInfoLabel1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.songInfoLabel1.Name = "songInfoLabel1";
            // 
            // albumArtPictureBox
            // 
            resources.ApplyResources(this.albumArtPictureBox, "albumArtPictureBox");
            this.albumArtPictureBox.Name = "albumArtPictureBox";
            this.albumArtPictureBox.TabStop = false;
            // 
            // playlistsViewPanel
            // 
            this.playlistsViewPanel.Controls.Add(this.playlistDetailPanel);
            this.playlistsViewPanel.Controls.Add(this.playlistListPanel);
            resources.ApplyResources(this.playlistsViewPanel, "playlistsViewPanel");
            this.playlistsViewPanel.Name = "playlistsViewPanel";
            // 
            // playlistDetailPanel
            // 
            this.playlistDetailPanel.Controls.Add(this.noPlaylistSelectedPanel);
            this.playlistDetailPanel.Controls.Add(this.dgvPlaylistSongs);
            this.playlistDetailPanel.Controls.Add(this.addSongsToPlaylistButton);
            this.playlistDetailPanel.Controls.Add(this.playAllButton);
            this.playlistDetailPanel.Controls.Add(this.playlistNameLabel);
            resources.ApplyResources(this.playlistDetailPanel, "playlistDetailPanel");
            this.playlistDetailPanel.Name = "playlistDetailPanel";
            // 
            // noPlaylistSelectedPanel
            // 
            this.noPlaylistSelectedPanel.Controls.Add(this.noSelectedPlaylistLb);
            resources.ApplyResources(this.noPlaylistSelectedPanel, "noPlaylistSelectedPanel");
            this.noPlaylistSelectedPanel.Name = "noPlaylistSelectedPanel";
            // 
            // noSelectedPlaylistLb
            // 
            resources.ApplyResources(this.noSelectedPlaylistLb, "noSelectedPlaylistLb");
            this.noSelectedPlaylistLb.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.noSelectedPlaylistLb.Name = "noSelectedPlaylistLb";
            // 
            // dgvPlaylistSongs
            // 
            this.dgvPlaylistSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvPlaylistSongs, "dgvPlaylistSongs");
            this.dgvPlaylistSongs.Name = "dgvPlaylistSongs";
            this.dgvPlaylistSongs.RowTemplate.Height = 24;
            this.dgvPlaylistSongs.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlaylistSongs_CellContentDoubleClick);
            // 
            // addSongsToPlaylistButton
            // 
            resources.ApplyResources(this.addSongsToPlaylistButton, "addSongsToPlaylistButton");
            this.addSongsToPlaylistButton.Name = "addSongsToPlaylistButton";
            this.addSongsToPlaylistButton.UseVisualStyleBackColor = true;
            this.addSongsToPlaylistButton.Click += new System.EventHandler(this.addSongsToPlaylistButton_Click);
            // 
            // playAllButton
            // 
            resources.ApplyResources(this.playAllButton, "playAllButton");
            this.playAllButton.Name = "playAllButton";
            this.playAllButton.UseVisualStyleBackColor = true;
            this.playAllButton.Click += new System.EventHandler(this.playAllButton_Click);
            // 
            // playlistNameLabel
            // 
            resources.ApplyResources(this.playlistNameLabel, "playlistNameLabel");
            this.playlistNameLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.playlistNameLabel.Name = "playlistNameLabel";
            // 
            // playlistListPanel
            // 
            this.playlistListPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.playlistListPanel.Controls.Add(this.playlistsListBox);
            this.playlistListPanel.Controls.Add(this.newPlaylistButton);
            this.playlistListPanel.Controls.Add(this.playlistTitleLabel);
            resources.ApplyResources(this.playlistListPanel, "playlistListPanel");
            this.playlistListPanel.Name = "playlistListPanel";
            // 
            // playlistsListBox
            // 
            this.playlistsListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.playlistsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.playlistsListBox, "playlistsListBox");
            this.playlistsListBox.ForeColor = System.Drawing.Color.White;
            this.playlistsListBox.FormattingEnabled = true;
            this.playlistsListBox.Name = "playlistsListBox";
            this.playlistsListBox.SelectedIndexChanged += new System.EventHandler(this.playlistsListBox_SelectedIndexChanged);
            // 
            // newPlaylistButton
            // 
            resources.ApplyResources(this.newPlaylistButton, "newPlaylistButton");
            this.newPlaylistButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.newPlaylistButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.newPlaylistButton.Name = "newPlaylistButton";
            this.newPlaylistButton.UseVisualStyleBackColor = false;
            this.newPlaylistButton.Click += new System.EventHandler(this.newPlaylistButton_Click);
            // 
            // playlistTitleLabel
            // 
            resources.ApplyResources(this.playlistTitleLabel, "playlistTitleLabel");
            this.playlistTitleLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.playlistTitleLabel.Name = "playlistTitleLabel";
            // 
            // musicLibraryViewPanel
            // 
            this.musicLibraryViewPanel.Controls.Add(this.infoForSongCountLabel);
            this.musicLibraryViewPanel.Controls.Add(this.musicLibraryCountLabel1);
            this.musicLibraryViewPanel.Controls.Add(this.musicLibLabel);
            this.musicLibraryViewPanel.Controls.Add(this.importSongBtn);
            this.musicLibraryViewPanel.Controls.Add(this.dgvMusicLibrary);
            resources.ApplyResources(this.musicLibraryViewPanel, "musicLibraryViewPanel");
            this.musicLibraryViewPanel.Name = "musicLibraryViewPanel";
            // 
            // infoForSongCountLabel
            // 
            resources.ApplyResources(this.infoForSongCountLabel, "infoForSongCountLabel");
            this.infoForSongCountLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.infoForSongCountLabel.Name = "infoForSongCountLabel";
            // 
            // musicLibraryCountLabel1
            // 
            resources.ApplyResources(this.musicLibraryCountLabel1, "musicLibraryCountLabel1");
            this.musicLibraryCountLabel1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.musicLibraryCountLabel1.Name = "musicLibraryCountLabel1";
            // 
            // musicLibLabel
            // 
            resources.ApplyResources(this.musicLibLabel, "musicLibLabel");
            this.musicLibLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.musicLibLabel.Name = "musicLibLabel";
            // 
            // importSongBtn
            // 
            resources.ApplyResources(this.importSongBtn, "importSongBtn");
            this.importSongBtn.BackColor = System.Drawing.Color.Black;
            this.importSongBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.importSongBtn.Name = "importSongBtn";
            this.importSongBtn.UseVisualStyleBackColor = false;
            this.importSongBtn.Click += new System.EventHandler(this.importSongBtn_Click);
            // 
            // dgvMusicLibrary
            // 
            this.dgvMusicLibrary.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvMusicLibrary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvMusicLibrary, "dgvMusicLibrary");
            this.dgvMusicLibrary.Name = "dgvMusicLibrary";
            this.dgvMusicLibrary.RowTemplate.Height = 24;
            // 
            // playbackTimer
            // 
            this.playbackTimer.Tick += new System.EventHandler(this.playbackTimer_Tick);
            // 
            // TuneInForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.playerControlPanel);
            this.Controls.Add(this.sideBarPanel);
            this.Name = "TuneInForm";
            this.sideBarPanel.ResumeLayout(false);
            this.sideBarPanel.PerformLayout();
            this.playerControlPanel.ResumeLayout(false);
            this.playerControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.nowPlayingViewPanel.ResumeLayout(false);
            this.nowPlayingViewPanel.PerformLayout();
            this.noMusicPlayingPanel.ResumeLayout(false);
            this.noMusicPlayingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPictureBox)).EndInit();
            this.playlistsViewPanel.ResumeLayout(false);
            this.playlistDetailPanel.ResumeLayout(false);
            this.playlistDetailPanel.PerformLayout();
            this.noPlaylistSelectedPanel.ResumeLayout(false);
            this.noPlaylistSelectedPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistSongs)).EndInit();
            this.playlistListPanel.ResumeLayout(false);
            this.playlistListPanel.PerformLayout();
            this.musicLibraryViewPanel.ResumeLayout(false);
            this.musicLibraryViewPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusicLibrary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel sideBarPanel;
        private System.Windows.Forms.Button nowPlayingBtn;
        private System.Windows.Forms.Button playlistBtn;
        private System.Windows.Forms.Button musicLibraryBtn;
        private System.Windows.Forms.Panel playerControlPanel;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button playPauseButton;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button repeatButton;
        private System.Windows.Forms.Label songInfoLabel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Label commentlabel;
        private System.Windows.Forms.Label musicLibraryCountLabel;
        private System.Windows.Forms.TrackBar progressBarTrackBar;
        private System.Windows.Forms.Timer playbackTimer;
        private System.Windows.Forms.Label timeTotalLabel;
        private System.Windows.Forms.Label timeElapsedLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.Panel musicLibraryViewPanel;
        private System.Windows.Forms.Label infoForSongCountLabel;
        private System.Windows.Forms.Label musicLibraryCountLabel1;
        private System.Windows.Forms.Label musicLibLabel;
        private System.Windows.Forms.Button importSongBtn;
        private System.Windows.Forms.DataGridView dgvMusicLibrary;
        private System.Windows.Forms.Panel playlistsViewPanel;
        private System.Windows.Forms.Panel playlistDetailPanel;
        private System.Windows.Forms.Panel playlistListPanel;
        private System.Windows.Forms.Button newPlaylistButton;
        private System.Windows.Forms.Label playlistTitleLabel;
        private System.Windows.Forms.Label playlistNameLabel;
        private System.Windows.Forms.ListBox playlistsListBox;
        private System.Windows.Forms.DataGridView dgvPlaylistSongs;
        private System.Windows.Forms.Button addSongsToPlaylistButton;
        private System.Windows.Forms.Button playAllButton;
        private System.Windows.Forms.Panel noPlaylistSelectedPanel;
        private System.Windows.Forms.Label noSelectedPlaylistLb;
        private System.Windows.Forms.Label playlistCountLabel;
        private System.Windows.Forms.Panel nowPlayingViewPanel;
        private System.Windows.Forms.PictureBox albumArtPictureBox;
        private System.Windows.Forms.Panel noMusicPlayingPanel;
        private System.Windows.Forms.Label noMusicLabel;
        private System.Windows.Forms.Label songInfoLabel1;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Label languageLabel;
    }
}

