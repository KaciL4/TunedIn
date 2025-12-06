using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunedIn
{
    public class Song
    {
        //public int Id { get; set; }//Unique ID for each song
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Duration { get; set; }
        public TimeSpan DurationTimeSpan { get; set; } 
    }
}
