using EEWorlds.Handlers.EELVL;
using EEWorlds.Handlers.JSON;
using EEWorlds.Handlers.TSON;

namespace EEWorlds
{
    public abstract class WorldManager
    {
        public abstract WorldFormat Format { get; }

        /// <summary>
        /// Load a world from the TSON format. This format was written by miou. (Atilla Lonny)
        /// </summary>
        /// <param name="input"> A TSON string representation of the world. </param>
        public static PrettyWorld LoadFromTSON(string input)
            => new PrettyWorld(TsonWorld.Load(input));

        /// <summary>
        /// Load a world from the JSON format. This format was written by miou. (Atilla Lonny)
        /// This format is legacy, deprecated in favor of TSON.
        /// </summary>
        /// <param name="input"> A TSON string representation of the world. </param>
        public static PrettyWorld LoadFromJSON(string input)
            => new PrettyWorld(JsonWorld.Load(input));

        /// <summary>
        /// Load a world from the EELVL format. This format was written by LukeM. (Luke Miles)
        /// </summary>
        /// <param name="input"> The raw bytes of the world. </param>
        public static PrettyWorld LoadFromEELVL(byte[] input)
            => new PrettyWorld(EELVLWorld.Load(input));

        /// <summary>
        /// Load a world from the EEditor (EELEVEL) format. This format was written by Cyph1e and Capasha.
        /// <param name="input"/> The raw bytes of the world. </param>
        /// <param name="version"> The format version of EELEVEL. </param>
        /// </summary>
        public static PrettyWorld LoadFromEEditor(byte[] input, EELevelVersion version)
            => new PrettyWorld(EELevelWorld.Load(input, (int)version));

        public abstract string Owner { get; internal set; }
        public abstract string Name { get; internal set; }
        public abstract string Crew { get; internal set; }
        public abstract string Campaign { get; internal set; }
        public abstract string Description { get; internal set; }

        public abstract int Type { get; internal set; }
        public abstract int Width { get; internal set; }
        public abstract int Height { get; internal set; }
        public abstract int Plays { get; internal set; }
        public abstract int Status { get; internal set; }
        public abstract int Likes { get; internal set; }
        public abstract int Favorites { get; internal set; }
        public abstract int BorderType { get; internal set; }
        public abstract uint BackgroundColor { get; internal set; }
        public abstract double Gravity { get; internal set; }

        public abstract bool Visible { get; internal set; }
        public abstract bool HideLobby { get; internal set; }
        public abstract bool MinimapEnabled { get; internal set; }
        public abstract bool LobbyPreviewEnabled { get; internal set; }
        public abstract bool FriendsOnly { get; internal set; }
        public abstract bool AllowSpectating { get; internal set; }
        public abstract bool IsCrewLogo { get; internal set; }

        // obsolete properties
        public abstract int CurseLimit { get; internal set; }

        public abstract int ZombieLimit { get; internal set; }
        public abstract int Woots { get; internal set; }
        public abstract int TotalWoots { get; internal set; }
        public abstract bool AllowPotions { get; internal set; }
        public abstract string EnabledPotions { get; internal set; }
    }
}