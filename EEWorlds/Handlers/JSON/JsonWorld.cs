using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayerIOClient;

namespace EEWorlds.Handlers.JSON
{
    public class JsonWorld : WorldManager
    {
        public override WorldFormat Format => WorldFormat.JSON;
        internal JObject _world_json_object;

        [JsonIgnore]
        public IEnumerable<IBlockChunk> Chunks { get => this.JsonWorldData; }

        internal static WorldManager Load(string input)
        {
            var json_object = JObject.Parse((string)input);
            var world = JsonConvert.DeserializeObject<JsonWorld>(input);

            world._world_json_object = json_object;
            return world;
        }

        [JsonIgnore]
        internal IEnumerable<JsonBlockChunk> JsonWorldData => _world_json_object.FromJsonArray();

        [JsonProperty("owner")]
        public override string Owner { get; internal set; }

        [JsonProperty("name")]
        public override string Name { get; internal set; }

        [JsonProperty("Campaign")]
        public override string Campaign { get; internal set; }

        [JsonProperty("Crew")]
        public override string Crew { get; internal set; }

        [JsonProperty("worldDescription")]
        public override string Description { get; internal set; }

        [JsonProperty("type")]
        public override int Type { get; internal set; }

        [JsonProperty("width")]
        public override int Width { get; internal set; }

        [JsonProperty("height")]
        public override int Height { get; internal set; }

        [JsonProperty("plays")]
        public override int Plays { get; internal set; }

        [JsonProperty("Status")]
        public override int Status { get; internal set; } = 1;

        [JsonProperty("HideLobby")]
        public override bool HideLobby { get; internal set; }

        [JsonProperty("Likes")]
        public override int Likes { get; internal set; }

        [JsonProperty("Favorites")]
        public override int Favorites { get; internal set; }

        [JsonProperty("BorderType")]
        public override int BorderType { get; internal set; } = -1;

        [JsonProperty("visible")]
        public override bool Visible { get; internal set; }

        [JsonProperty("backgroundColor")]
        public override uint BackgroundColor { get; internal set; }

        [JsonProperty("allowSpectating")]
        public override bool AllowSpectating { get; internal set; } = true;

        [JsonProperty("MinimapEnabled")]
        public override bool MinimapEnabled { get; internal set; } = true;

        [JsonProperty("LobbyPreviewEnabled")]
        public override bool LobbyPreviewEnabled { get; internal set; } = true;

        [JsonProperty("IsCrewLogo")]
        public override bool IsCrewLogo { get; internal set; }

        [JsonProperty("Gravity")]
        public override double Gravity { get; internal set; } = -1;

        [JsonProperty("friendsOnly")]
        public override bool FriendsOnly { get; internal set; }

        [Obsolete("Curses were removed in 2016 - this property is now deprecated.")]
        [JsonProperty("curseLimit")]
        public override int CurseLimit { get; internal set; }

        [Obsolete("Zombies were removed in 2016 - this property is now deprecated")]
        [JsonProperty("zombieLimit")]
        public override int ZombieLimit { get; internal set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [JsonProperty("woots")]
        public override int Woots { get; internal set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [JsonProperty("totalwoots")]
        public override int TotalWoots { get; internal set; }

        [Obsolete("Potions were removed in 2015 - this property is now deprecated.")]
        [JsonProperty("allowpotions")]
        public override bool AllowPotions { get; internal set; }

        [Obsolete("Potions were removed in 2015 - this property is now deprecated.")]
        [JsonProperty("enabledpotions")]
        public override string EnabledPotions { get; internal set; }
    }

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

        internal static byte[] TryGetBytes(this DatabaseObject input, string key, byte[] defaultValue)
        {
            if (input.TryGetValue(key, out var obj))
                return (obj is string) ? Convert.FromBase64String(obj as string) : (obj is byte[]) ? obj as byte[] : defaultValue;

            return defaultValue;
        }

        internal static List<JsonBlockChunk> FromWorldData(this DatabaseArray input)
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

        internal static List<JsonBlockChunk> FromJsonArray(this JObject world)
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
}