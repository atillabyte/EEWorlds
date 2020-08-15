using System.Collections.Generic;
using System.IO;
using EELVL;

namespace EEWorlds.Handlers.EELVL
{
    public class EELVLWorld : WorldManager
    {
        public override WorldFormat Format => WorldFormat.EELVL;
        public List<(int layer, int x, int y, Blocks.Block block)> BlockCollection { get; set; }

        public override string Owner { get; internal set; }
        public override string Name { get; internal set; }
        public override string Crew { get; internal set; }
        public override string Campaign { get; internal set; }
        public override string Description { get; internal set; }
        public override int Type { get; internal set; }
        public override int Width { get; internal set; }
        public override int Height { get; internal set; }
        public override int Plays { get; internal set; }
        public override int Status { get; internal set; }
        public override int Likes { get; internal set; }
        public override int Favorites { get; internal set; }
        public override int BorderType { get; internal set; }
        public override uint BackgroundColor { get; internal set; }
        public override double Gravity { get; internal set; }
        public override bool Visible { get; internal set; }
        public override bool HideLobby { get; internal set; }
        public override bool MinimapEnabled { get; internal set; }
        public override bool LobbyPreviewEnabled { get; internal set; }
        public override bool FriendsOnly { get; internal set; }
        public override bool AllowSpectating { get; internal set; }
        public override bool IsCrewLogo { get; internal set; }
        public override int CurseLimit { get; internal set; }
        public override int ZombieLimit { get; internal set; }
        public override int Woots { get; internal set; }
        public override int TotalWoots { get; internal set; }
        public override bool AllowPotions { get; internal set; }
        public override string EnabledPotions { get; internal set; }

        internal static WorldManager Load(byte[] input)
        {
            var world = new Level(new MemoryStream(input));
            var blocks = new List<(int layer, int x, int y, Blocks.Block block)>();

            for (var l = 0; l < 1; l++)
            {
                for (var x = 0; x < world.Width; x++)
                {
                    for (var y = 0; y < world.Height; y++)
                    {
                        blocks.Add((l, x, y, world[l, x, y]));
                    }
                }
            }

            return new EELVLWorld()
            {
                Owner = world.OwnerID,
                Name = world.WorldName,
                Crew = world.CrewID,
                Description = world.Description,
                Width = world.Width,
                Height = world.Height,
                Status = world.CrewStatus,
                BackgroundColor = world.BackgroundColor,
                Gravity = world.Gravity,
                MinimapEnabled = world.Minimap,
                BlockCollection = blocks
            };
        }
    }
}