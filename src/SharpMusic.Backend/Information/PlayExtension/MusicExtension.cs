using CSCore;
using CSCore.Codecs;

namespace SharpMusic.Information.PlayExtension
{
    public static class MusicExtension
    {
        public static IWaveSource GetWaveSource(this Music music) =>
            CodecFactory.Instance.GetCodec(music.StreamUri);
    }
}