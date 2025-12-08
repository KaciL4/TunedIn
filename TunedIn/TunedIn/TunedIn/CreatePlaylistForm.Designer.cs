namespace TunedIn
{
    partial class CreatePlaylistForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePlaylistForm));
            this.createPlaylistLabel = new System.Windows.Forms.Label();
            this.createPlaylistSubtitleLb = new System.Windows.Forms.Label();
            this.playlistnameLb = new System.Windows.Forms.Label();
            this.playlistNameTextBox = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createPlaylistLabel
            // 
            resources.ApplyResources(this.createPlaylistLabel, "createPlaylistLabel");
            this.createPlaylistLabel.Name = "createPlaylistLabel";
            // 
            // createPlaylistSubtitleLb
            // 
            resources.ApplyResources(this.createPlaylistSubtitleLb, "createPlaylistSubtitleLb");
            this.createPlaylistSubtitleLb.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.createPlaylistSubtitleLb.Name = "createPlaylistSubtitleLb";
            // 
            // playlistnameLb
            // 
            resources.ApplyResources(this.playlistnameLb, "playlistnameLb");
            this.playlistnameLb.Name = "playlistnameLb";
            // 
            // playlistNameTextBox
            // 
            resources.ApplyResources(this.playlistNameTextBox, "playlistNameTextBox");
            this.playlistNameTextBox.Name = "playlistNameTextBox";
            // 
            // createButton
            // 
            resources.ApplyResources(this.createButton, "createButton");
            this.createButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.createButton.Name = "createButton";
            this.createButton.UseVisualStyleBackColor = false;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackColor = System.Drawing.Color.Black;
            this.cancelButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // CreatePlaylistForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.playlistNameTextBox);
            this.Controls.Add(this.playlistnameLb);
            this.Controls.Add(this.createPlaylistSubtitleLb);
            this.Controls.Add(this.createPlaylistLabel);
            this.Name = "CreatePlaylistForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label createPlaylistLabel;
        private System.Windows.Forms.Label createPlaylistSubtitleLb;
        private System.Windows.Forms.Label playlistnameLb;
        private System.Windows.Forms.TextBox playlistNameTextBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
    }
}