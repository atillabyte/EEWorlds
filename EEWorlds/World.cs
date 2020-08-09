using System.Collections.Generic;
using EEWorlds.Handlers.JSON;
using EEWorlds.Handlers.TSON;

namespace EEWorlds
{
    public abstract class World
    {
        public static World LoadFromTSON(string input)
            => TsonWorld.Load(input);

        public static World LoadFromJSON(string input)
            => JsonWorld.Load(input);

        public abstract IEnumerable<IBlockChunk> WorldData { get; }

        public abstract string Owner { get; set; }
        public abstract string Name { get; set; }
        public abstract string Crew { get; set; }
        public abstract string Campaign { get; set; }

        public abstract int Type { get; set; }
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract int Plays { get; set; }
        public abstract int Status { get; set; }
        public abstract int Likes { get; set; }
        public abstract int Favorites { get; set; }

        public abstract uint BackgroundColor { get; set; }

        public abstract double Gravity { get; set; }

        public abstract bool Visible { get; set; }
        public abstract bool HideLobby { get; set; }
        public abstract bool MinimapEnabled { get; set; }
        public abstract bool LobbyPreviewEnabled { get; set; }
        public abstract bool FriendsOnly { get; set; }
        public abstract bool AllowSpectating { get; set; }
        public abstract bool IsCrewLogo { get; set; }

        // obsolete properties
        public abstract int CurseLimit { get; set; }
        public abstract int ZombieLimit { get; set; }
        public abstract int Woots { get; set; }
        public abstract int TotalWoots { get; set; }
        public abstract bool AllowPotions { get; set; }
    }
}
