using System;
using System.Collections.Generic;
using Tson;

namespace EEWorlds.Handlers.TSON
{
    public class TsonWorld : WorldManager
    {
        public override WorldFormat Format => WorldFormat.TSON;
        public IEnumerable<IBlockChunk> Chunks { get => this.TsonWorldData; }

        internal static WorldManager Load(string input)
        {
            return TsonConvert.DeserializeObject<TsonWorld>(input);
        }

        [TsonProperty("owner")]
        public override string Owner { get; internal set; }

        [TsonProperty("name")]
        public override string Name { get; internal set; }

        [TsonProperty("Campaign")]
        public override string Campaign { get; internal set; }

        [TsonProperty("Crew")]
        public override string Crew { get; internal set; }

        [TsonProperty("worldDescription")]
        public override string Description { get; internal set; }

        [TsonProperty("type")]
        public override int Type { get; internal set; }

        [TsonProperty("width")]
        public override int Width { get; internal set; }

        [TsonProperty("height")]
        public override int Height { get; internal set; }

        [TsonProperty("plays")]
        public override int Plays { get; internal set; }

        [TsonProperty("visible")]
        public override bool Visible { get; internal set; }

        [TsonProperty("worlddata")]
        public IEnumerable<TsonBlockChunk> TsonWorldData { get; set; }

        [TsonProperty("Gravity")]
        public override double Gravity { get; internal set; } = -1;

        [TsonProperty("backgroundColor")]
        public override uint BackgroundColor { get; internal set; }

        [TsonProperty("Status")]
        public override int Status { get; internal set; } = 1;

        [TsonProperty("HideLobby")]
        public override bool HideLobby { get; internal set; }

        [TsonProperty("Likes")]
        public override int Likes { get; internal set; }

        [TsonProperty("Favorites")]
        public override int Favorites { get; internal set; }

        [TsonProperty("BorderType")]
        public override int BorderType { get; internal set; } = -1;

        [TsonProperty("allowSpectating")]
        public override bool AllowSpectating { get; internal set; } = true;

        [TsonProperty("MinimapEnabled")]
        public override bool MinimapEnabled { get; internal set; } = true;

        [TsonProperty("LobbyPreviewEnabled")]
        public override bool LobbyPreviewEnabled { get; internal set; } = true;

        [TsonProperty("friendsOnly")]
        public override bool FriendsOnly { get; internal set; }

        [TsonProperty("IsCrewLogo")]
        public override bool IsCrewLogo { get; internal set; }

        [Obsolete("Curses were removed in 2016 - this property is now deprecated.")]
        [TsonProperty("curseLimit")]
        public override int CurseLimit { get; internal set; }

        [Obsolete("Zombies were removed in 2016 - this property is now deprecated")]
        [TsonProperty("zombieLimit")]
        public override int ZombieLimit { get; internal set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [TsonProperty("woots")]
        public override int Woots { get; internal set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [TsonProperty("totalwoots")]
        public override int TotalWoots { get; internal set; }

        [Obsolete("Potions were removed in 2015 - this property is now deprecated.")]
        [TsonProperty("allowpotions")]
        public override bool AllowPotions { get; internal set; }

        [Obsolete("Potions were removed in 2015 - this property is now deprecated.")]
        [TsonProperty("enabledpotions")]
        public override string EnabledPotions { get; internal set; }
    }
}