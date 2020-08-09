using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayerIOClient;

namespace EEWorlds.Handlers.JSON
{
    internal class PropertyEnumerable
    {
        internal Dictionary<string, object> _properties = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        public Dictionary<string, object> Properties => _properties;
        public object this[string key]
        {
            get { return _properties.ContainsKey(key) ? _properties[key] : null; }
            internal set { if (_properties.ContainsKey(key)) _properties.Add(key, value); else _properties[key] = value; }
        }
    }

    internal static class JsonWorldExtensions
    {
        internal static T Get<T>(this Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key].GetType() == typeof(JValue) ? (((JValue)(dictionary[key])).ToObject<T>()) : (T)Convert.ChangeType(dictionary[key], typeof(T));

            return default;
        }

        private static byte[] TryGetBytes(this DatabaseObject input, string key, byte[] defaultValue)
        {
            if (input.TryGetValue(key, out var obj))
                return (obj is string) ? Convert.FromBase64String(obj as string) : (obj is byte[]) ? obj as byte[] : defaultValue;

            return defaultValue;
        }

        public static List<JsonBlockChunk> FromWorldData(this DatabaseArray input)
        {
            var blocks = new List<JsonBlockChunk>();

            if (input == null || input.Count == 0)
                return blocks;

            for (var i = 0; i < input.Count; i++)
            {
                if (input.Contains(i) && input.GetObject(i).Count != 0)
                {
                    var obj = input.GetObject(i);
                    dynamic temp = new JsonBlockChunk();

                    foreach (var kvp in obj)
                        temp[kvp.Key] = kvp.Value;

                    byte[] x = obj.TryGetBytes("x", new byte[0]), y = obj.TryGetBytes("y", new byte[0]);
                    byte[] x1 = obj.TryGetBytes("x1", new byte[0]), y1 = obj.TryGetBytes("y1", new byte[0]);

                    temp.X = x;
                    temp.Y = y;
                    temp.X1 = x1;
                    temp.Y1 = y1;

                    blocks.Add(temp);
                }
            }

            return blocks;
        }

        public static List<JsonBlockChunk> FromJsonArray(this JObject world)
        {
            var array = world["worlddata"].Values().AsJEnumerable();
            var temp = new DatabaseArray();

            foreach (var block in array)
            {
                var dbo = new DatabaseObject();

                foreach (var token in block)
                {
                    var property = (JProperty)token;
                    var value = property.Value;

                    switch (value.Type)
                    {
                        case JTokenType.Integer:
                            dbo.Set(property.Name, (uint)value);
                            break;
                        case JTokenType.Boolean:
                            dbo.Set(property.Name, (bool)value);
                            break;
                        case JTokenType.Float:
                            dbo.Set(property.Name, (double)value);
                            break;
                        default:
                            dbo.Set(property.Name, (string)value);
                            break;
                    }
                }

                temp.Add(dbo);
            }

            return FromWorldData(temp);
        }
    }

    public class JsonWorld : World
    {
        internal JObject _world_json_object;

        internal static World Load(string input)
        {
            var json_object = JObject.Parse((string)input);
            var world = JsonConvert.DeserializeObject<JsonWorld>(input);

            world._world_json_object = json_object;
            return world;
        }

        [JsonIgnore]
        public override IEnumerable<IBlockChunk> WorldData { get => this.JsonWorldData; }

        [JsonIgnore]
        internal IEnumerable<JsonBlockChunk> JsonWorldData => _world_json_object.FromJsonArray();

        [JsonProperty("owner")]
        public override string Owner { get; set; }

        [JsonProperty("name")]
        public override string Name { get; set; }

        [JsonProperty("Campaign")]
        public override string Campaign { get; set; }

        [JsonProperty("Crew")]
        public override string Crew { get; set; }

        [JsonProperty("type")]
        public override int Type { get; set; }

        [JsonProperty("width")]
        public override int Width { get; set; }

        [JsonProperty("height")]
        public override int Height { get; set; }

        [JsonProperty("plays")]
        public override int Plays { get; set; }

        [JsonProperty("Status")]
        public override int Status { get; set; }

        [JsonProperty("HideLobby")]
        public override bool HideLobby { get; set; }

        [JsonProperty("Likes")]
        public override int Likes { get; set; }

        [JsonProperty("Favorites")]
        public override int Favorites { get; set; }

        [JsonProperty("visible")]
        public override bool Visible { get; set; }

        [JsonProperty("backgroundColor")]
        public override uint BackgroundColor { get; set; }

        [JsonProperty("allowSpectating")]
        public override bool AllowSpectating { get; set; } = true;

        [JsonProperty("MinimapEnabled")]
        public override bool MinimapEnabled { get; set; } = true;

        [JsonProperty("LobbyPreviewEnabled")]
        public override bool LobbyPreviewEnabled { get; set; } = true;

        [JsonProperty("IsCrewLogo")]
        public override bool IsCrewLogo { get; set; }

        [JsonProperty("Gravity")]
        public override double Gravity { get; set; } = 1.0f;

        [JsonProperty("friendsOnly")]
        public override bool FriendsOnly { get; set; }

        [Obsolete("Curses were removed in 2016 - this property is now deprecated.")]
        [JsonProperty("curseLimit")]
        public override int CurseLimit { get; set; }

        [Obsolete("Zombies were removed in 2016 - this property is now deprecated")]
        [JsonProperty("zombieLimit")]
        public override int ZombieLimit { get; set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [JsonProperty("woots")]
        public override int Woots { get; set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [JsonProperty("totalwoots")]
        public override int TotalWoots { get; set; }

        [Obsolete("Potions were removed in 2015 - this property is now deprecated.")]
        [JsonProperty("allowpotions")]
        public override bool AllowPotions { get; set; }
    }
}
