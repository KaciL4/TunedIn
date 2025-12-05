using System;
using System.Collections.Generic;

namespace TunedIn.Models
{
    public class Playlist
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "New Playlist";
        public List<Guid> SongIds { get; set; } = new List<Guid>();
    }
}