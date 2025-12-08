using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TunedIn
{
    public partial class CreatePlaylistForm : Form
    {
        public CreatePlaylistForm()
        {
            InitializeComponent();
        }
        public string PlaylistName { get; private set; }
        private void createButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(playlistNameTextBox.Text))
            {
                MessageBox.Show("Please enter a playlist name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None; // Prevent the form from closing
            }
            else
            {
                PlaylistName = playlistNameTextBox.Text.Trim();
                this.DialogResult = DialogResult.OK; //Close the form
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
