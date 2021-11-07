using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatVibesApp
{
    public class SongModel
    {
        public int Id { get; set; }
        public string Artist { get; set; }
        public string SongTitle { get; set; }
        public string Album { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string RecordLabel { get; set; }
        public string TrackFile { get; set; }
    }
}
