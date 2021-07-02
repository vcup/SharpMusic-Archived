using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagLib.Audible;
using File = TagLib.Audible.File;

namespace SharpMusic.Backend.Information
{
    public class Music : InformationBase
    {
        private List<string> _alias = new();
        private List<Artist> _artists = new();
        public static readonly HashSet<Music> AllMusics = new();

        public Music()
        {
            AllMusics.Add(this);
        }

        #region Property

        public string Name { get; set; }

        public IList<string> Alias => _alias;
        public Album Album { get; set; }

        public IList<Artist> Artists => _artists;

        public Uri StreamUri { get; set; }

        public TimeSpan PlayTime { get; set; }
        
        public uint TrackNo { get; set; }

        #endregion
    }
}