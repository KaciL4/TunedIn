using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TunedIn.Models
{
    [Table("Songs")]
    public class SongEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public string Title { get; set; } = "Unknown Title";
        public string Artist { get; set; } = "Unknown Artist";
        public string Album { get; set; } = "Unknown Album";

        // store duration as seconds for simple mapping
        public int DurationSeconds { get; set; }
    }
}