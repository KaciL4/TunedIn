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
    public partial class SelectSongsForm : Form
    {
        // This is declared globally in the form so other methods can access the data source later if needed.
        private List<Song> allMusicLibrary;
        public List<Song> SelectedSongs { get; private set; } = new List<Song>();
        public SelectSongsForm(List<Song> musicLibrarySource)
        {
            InitializeComponent();
            // Setup DataGridView
            dgvSelectSongs.AutoGenerateColumns = false;
            dgvSelectSongs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 1. Add Checkbox Column for selection
            dgvSelectSongs.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Add",
                Name = "colSelect",
                Width = 50
            });

            // Add Song data columns (Title, Artist, Duration)
            dgvSelectSongs.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Title", HeaderText = "Title", Width = 250 });
            dgvSelectSongs.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Artist", HeaderText = "Artist", Width = 200 });
            dgvSelectSongs.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Duration", HeaderText = "Duration", Width = 70 });

            // Bind the Music Library
            dgvSelectSongs.DataSource = musicLibrarySource;
        }
        private void selectButton_Click(object sender, EventArgs e)
        {
            // Iterate through all rows in the DataGridView
            foreach (DataGridViewRow row in dgvSelectSongs.Rows)
            {
                // Check if the checkbox column is checked (index 0 is the checkbox)
                if (row.Cells["colSelect"].Value != null && (bool)row.Cells["colSelect"].Value)
                {
                    // Get the Song object bound to this row
                    if (row.DataBoundItem is Song song)
                    {
                        SelectedSongs.Add(song);
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Use the Designer DialogResult = Cancel setup if possible, otherwise:
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
