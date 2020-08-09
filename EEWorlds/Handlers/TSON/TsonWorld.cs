using System;
using System.Collections.Generic;
using Tson;

namespace EEWorlds.Handlers.TSON
{
    public class TsonWorld : World
    {
        public override IEnumerable<IBlockChunk> WorldData { get => this.TsonWorldData; }

        internal static World Load(string input)
        {
            return TsonConvert.DeserializeObject<TsonWorld>(input);
        }

        [TsonProperty("owner")]
        public override string Owner { get; set; }

        [TsonProperty("name")]
        public override string Name { get; set; }

        [TsonProperty("Campaign")]
        public override string Campaign { get; set; }

        [TsonProperty("type")]
        public override int Type { get; set; }

        [TsonProperty("width")]
        public override int Width { get; set; }

        [TsonProperty("height")]
        public override int Height { get; set; }

        [TsonProperty("plays")]
        public override int Plays { get; set; }

        [TsonProperty("visible")]
        public override bool Visible { get; set; }

        [TsonProperty("worlddata")]
        public IEnumerable<TsonBlockChunk> TsonWorldData { get; set; }

        [TsonProperty("Gravity")]
        public override double Gravity { get; set; } = 1.0f;

        [TsonProperty("backgroundColor")]
        public override uint BackgroundColor { get; set; }

        [TsonProperty("Crew")]
        public override string Crew { get; set; }

        [TsonProperty("Status")]
        public override int Status { get; set; }

        [TsonProperty("HideLobby")]
        public override bool HideLobby { get; set; }

        [TsonProperty("Likes")]
        public override int Likes { get; set; }

        [TsonProperty("Favorites")]
        public override int Favorites { get; set; }

        [TsonProperty("allowSpectating")]
        public override bool AllowSpectating { get; set; } = true;

        [TsonProperty("MinimapEnabled")]
        public override bool MinimapEnabled { get; set; } = true;

        [TsonProperty("LobbyPreviewEnabled")]
        public override bool LobbyPreviewEnabled { get; set; } = true;

        [TsonProperty("friendsOnly")]
        public override bool FriendsOnly { get; set; }

        [TsonProperty("IsCrewLogo")]
        public override bool IsCrewLogo { get; set; }

        [Obsolete("Curses were removed in 2016 - this property is now deprecated.")]
        [TsonProperty("curseLimit")]
        public override int CurseLimit { get; set; }

        [Obsolete("Zombies were removed in 2016 - this property is now deprecated")]
        [TsonProperty("zombieLimit")]
        public override int ZombieLimit { get; set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [TsonProperty("woots")]
        public override int Woots { get; set; }

        [Obsolete("Woots were removed in 2015 - this property is now deprecated.")]
        [TsonProperty("totalwoots")]
        public override int TotalWoots { get; set; }

        [Obsolete("Potions were removed in 2015 - this property is now deprecated.")]
        [TsonProperty("allowpotions")]
        public override bool AllowPotions { get; set; }
    }
}
