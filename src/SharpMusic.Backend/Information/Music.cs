using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharpMusic.Backend.Information
{
    [Serializable]
    public class Music : InformationBase, ISerializable
    {
        private List<string> _alias = new();
        private List<Artist> _artists = new();
        public static readonly HashSet<Music> AllMusics = new();

        public Music()
        {
            AllMusics.Add(this);
        }

        #region Serialization

        public Music(SerializationInfo info, StreamingContext context)
        {
            StreamUri = (Uri)         info.GetValue("StreamUri", typeof(Uri)         ) ;
            TrackNo   = (uint)        info.GetValue("TrackNo"  , typeof(uint)        )!;
            Album     = (Album)       info.GetValue("Album"    , typeof(Album)       ) ;
            Name      = (string)      info.GetValue("Name"     , typeof(string)      ) ; 
            PlayTime  = (TimeSpan)    info.GetValue("PlayTime" , typeof(TimeSpan)    )!;
            _alias    = (List<string>)info.GetValue("Alias"    , typeof(List<string>)) ;
            _artists  = (List<Artist>)info.GetValue("Artists"  , typeof(List<Artist>)) ;

            AllMusics.Add(this);
        }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StreamUri", StreamUri, typeof(Uri)         );
            info.AddValue("TrackNo"  , TrackNo  , typeof(uint)        );
            info.AddValue("Album"    , Album    , typeof(Album)       );
            info.AddValue("Name"     , Name     , typeof(string)      );
            info.AddValue("PlayTime" , PlayTime , typeof(TimeSpan)    );
            info.AddValue("Artists"  , Artists  , typeof(List<Artist>));
            info.AddValue("Alias"    , Alias    , typeof(List<string>));
        }
        
        #endregion

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