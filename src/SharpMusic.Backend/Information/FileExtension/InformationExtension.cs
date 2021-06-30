using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SharpMusic.Backend.Information.FileExtension
{
    public static class InformationExtension
    {
        static InformationExtension()
        {
            if (!Directory.Exists("./Cache"))
                Directory.CreateDirectory("./Cache");
        }

        public static Dictionary<string, string> _jsonDataStruct = new();
            
        private static readonly Stream JsonStream = System.IO.File.Open(
            "./Cache/Information.json", FileMode.OpenOrCreate
        );
        private static Utf8JsonWriter _utf8JsonWriter = new(JsonStream);
        
        public static void SaveWithJson(this Album album)
        {
        }

    }
}