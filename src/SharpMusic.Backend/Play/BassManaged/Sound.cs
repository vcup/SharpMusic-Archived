using System;
using System.Collections.Generic;
using ManagedBass;
using TagLib.Id3v2;

namespace SharpMusic.Backend.Play.BassManaged
{
    public class Sound
    {
        private int _streamHandle;
        private Uri _streamUri;
        private static readonly Dictionary<string, int> _HandleTable = new();
        
        public Sound(string filePath)
        {
            _streamUri = new Uri(filePath);
            _streamHandle = CreateStream(_streamUri.LocalPath);
        }

        public Sound(Uri uri)
        {
            if (uri.IsFile)
            {
                _streamHandle = CreateStream(uri.LocalPath);
                _streamUri = uri;
            }
            else
            {
                throw new ArgumentException("UnSupport Network Stream");
            }
        }

        private int CreateStream(string filePath)
        {
            if (_HandleTable.TryGetValue(filePath, out int handle))
            {
                return handle;
            }
            
            handle = Bass.CreateStream(filePath);
            if (handle == 0)
            {
                switch (Bass.LastError)
                {
                    default:
                        Console.WriteLine(Bass.LastError);
                        break;
                }
            }

            return handle;
        }

        public int Handle
        {
            get => _streamHandle;
        }

        ~Sound()
        {
            Bass.StreamFree(_streamHandle);
        }
    }
}