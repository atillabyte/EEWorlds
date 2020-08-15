using Tson;

namespace EEWorlds.Handlers.TSON
{
    public class TsonBlockChunk : IBlockChunk
    {
        [TsonProperty("type")]
        public uint Type { get; set; }

        [TsonProperty("layer")]
        public int Layer { get; set; }

        [TsonProperty("x")]
        public byte[] X { get; set; }

        [TsonProperty("y")]
        public byte[] Y { get; set; }

        [TsonProperty("x1")]
        public byte[] X1 { get; set; }

        [TsonProperty("y1")]
        public byte[] Y1 { get; set; }

        [TsonProperty("text")]
        public string Text { get; set; }

        [TsonProperty("rotation")]
        public int Rotation { get; set; }

        [TsonProperty("goal")]
        public int Goal { get; set; }

        [TsonProperty("id")]
        public object Id { get; set; }

        [TsonProperty("target")]
        public object Target { get; set; }

        [TsonProperty("signtype")]
        public int SignType { get; set; }

        [TsonProperty("mes1")]
        public string TextMessageFirst { get; set; }

        [TsonProperty("mes2")]
        public string TextMessageSecond { get; set; }

        [TsonProperty("mes3")]
        public string TextMessageThird { get; set; }

        [TsonProperty("goal")]
        public uint WrapLength { get; set; }

        [TsonProperty("text_color")]
        public string TextColor { get; set; }
    }
}