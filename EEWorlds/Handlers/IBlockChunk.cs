namespace EEWorlds
{
    public interface IBlockChunk
    {
        uint Type { get; set; }
        uint WrapLength { get; set; }

        int Layer { get; set; }
        int Rotation { get; set; }
        int Goal { get; set; }
        int SignType { get; set; }

        byte[] X { get; set; }
        byte[] Y { get; set; }
        byte[] X1 { get; set; }
        byte[] Y1 { get; set; }

        string Text { get; set; }
        string TextMessageFirst { get; set; }
        string TextMessageSecond { get; set; }
        string TextMessageThird { get; set; }
        string TextColor { get; set; }

        object Id { get; set; }
        object Target { get; set; }
    }
}