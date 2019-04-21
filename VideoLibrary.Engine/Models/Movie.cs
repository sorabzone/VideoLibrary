using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.Engine.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}
