using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunedIn
{
    public class Playlist
    {
        public string Name { get; set; }
        //list to hold the Song objects that belong to this playlist
        public List<Song> Songs { get; set; } = new List<Song>();

        // Constructor
        public Playlist(string name)
        {
            Name = name;
        }

        // Calculated property for displaying song count
        public string SongCountDisplay => $"{Songs.Count} song(s)";
    }
}
