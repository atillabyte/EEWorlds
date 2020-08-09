namespace EEWorlds.Handlers.JSON
{
    internal class JsonBlockChunk : PropertyEnumerable, IBlockChunk
    {
        public uint Type { get => _properties.Get<uint>("type"); set => _properties["type"] = value; }
        public int Layer { get => _properties.Get<int>("layer"); set => _properties["layer"] = value; }

        public int Rotation { get => _properties.Get<int>("rotation"); set => _properties["rotation"] = value; }
        public int Goal { get => _properties.Get<int>("goal"); set => _properties["goal"] = value; }

        public object Id { get => _properties.Get<object>("id"); set => _properties["id"] = value; }
        public object Target { get => _properties.Get<object>("target"); set => _properties["target"] = value; }

        public string Text { get => _properties.Get<string>("text"); set => _properties["text"] = value; }
        public int SignType { get => _properties.Get<int>("signtype"); set => _properties["signtype"] = value; }

        public string Name { get => _properties.Get<string>("name"); set => _properties["name"] = value; }
        public string TextMessage1 { get => _properties.Get<string>("mes1"); set => _properties["mes1"] = value; }
        public string TextMessage2 { get => _properties.Get<string>("mes2"); set => _properties["mes2"] = value; }
        public string TextMessage3 { get => _properties.Get<string>("mes3"); set => _properties["mes3"] = value; }

        public string TextMessageFirst { get => TextMessage1; set => TextMessage1 = value; }
        public string TextMessageSecond { get => TextMessage2; set => TextMessage2 = value; }
        public string TextMessageThird { get => TextMessage3; set => TextMessage3 = value; }

        public byte[] X { get; set; }
        public byte[] Y { get; set; }
        public byte[] X1 { get; set; }
        public byte[] Y1 { get; set; }
    }
}
