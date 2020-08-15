using System;
using System.Collections.Generic;
using System.Linq;
using BotBits;
using EEWorlds.Handlers.EELVL;
using EEWorlds.Handlers.JSON;
using EEWorlds.Handlers.TSON;

namespace EEWorlds
{
    public class PrettyWorld : World
    {
        public Minimap Minimap { get; }
        public ObsoleteProperties Obsolete { get; }
        public bool HasErrors { get; }

        public string Owner { get; }
        public string Name { get; }
        public string Crew { get; }
        public string Campaign { get; }
        public string Description { get; }

        public int Plays { get; }
        public int Likes { get; }
        public int Favorites { get; }
        public int BorderType { get; }

        public WorldType Type { get; }
        public WorldStatus Status { get; }
        public uint BackgroundColor { get; }
        public double Gravity { get; }

        public bool Visible { get; }
        public bool HideLobby { get; }
        public bool MinimapEnabled { get; }
        public bool LobbyPreviewEnabled { get; }
        public bool FriendsOnly { get; }
        public bool AllowSpectating { get; }
        public bool IsCrewLogo { get; }

        internal PrettyWorld(WorldManager world) : base(world.Width, world.Height)
        {
            this.Owner = world.Owner;
            this.Name = world.Name;
            this.Crew = world.Crew;
            this.Campaign = world.Campaign;
            this.Description = world.Description;
            this.Plays = world.Plays;
            this.Likes = world.Likes;
            this.Favorites = world.Favorites;
            this.Status = (WorldStatus)world.Status;
            this.BorderType = world.BorderType == -1 ? ((world.Type != 8) ? 9 : 182) : world.BorderType;
            this.Type = Enum.IsDefined(typeof(WorldType), world.Type) ? (WorldType)world.Type : WorldType.Unknown;
            this.Gravity = this.Type == WorldType.MoonLarge ? 0.16 : 1.0;

            this.Obsolete = new ObsoleteProperties
            {
                AllowPotions = world.AllowPotions,
                CurseLimit = world.CurseLimit,
                ZombieLimit = world.ZombieLimit,
                EnabledPotions = world.EnabledPotions,
                Woots = world.Woots,
                TotalWoots = world.TotalWoots
            };

            this.LoadBlocks(world, out var encountered_errors);
            this.Minimap = new Minimap(this, this.BackgroundColor);

            this.HasErrors = encountered_errors;
        }

        public PrettyWorld(int width, int height) : base(width, height)
        {
        }

        private void LoadBlocks(WorldManager world, out bool encountered_errors)
        {
            encountered_errors = false;

            switch (world.Format)
            {
                case WorldFormat.EELEVEL:
                case WorldFormat.EELVL:
                    var blocks = (world is EELVLWorld) ?
                        (world as EELVLWorld).BlockCollection :
                        (world as EELevelWorld).BlockCollection;

                    foreach (var (layer, x, y, b) in blocks)
                    {
                        try
                        {
                            if (layer == 0)
                            {
                                var foreground = (Foreground.Id)b.BlockID;
                                var block = GetForegroundFromEELVLBlock(b, foreground);

                                this.Foreground[x, y] = block;
                            }
                            else
                            {
                                var background = (Background.Id)b.BlockID;
                                var block = new BackgroundBlock(background);

                                this.Background[x, y] = block;
                            }
                        }
                        catch
                        {
                            encountered_errors = true;
                        }
                    }
                    break;

                case WorldFormat.JSON:
                case WorldFormat.TSON:
                    var chunks =
                        world.Format == WorldFormat.JSON ? (world as JsonWorld).Chunks :
                        world.Format == WorldFormat.TSON ? (world as TsonWorld).Chunks :
                        throw new NotSupportedException();

                    if (chunks != null)
                    {
                        foreach (var chunk in chunks)
                        {
                            try
                            {
                                var points = this.GetShortPos(chunk.X1 ?? new byte[0], chunk.Y1 ?? new byte[0])
                                                .Concat(this.GetPos(chunk.X ?? new byte[0], chunk.Y ?? new byte[0]));

                                if (chunk.Layer == 0)
                                {
                                    var foreground = (Foreground.Id)chunk.Type;
                                    var block = this.GetForegroundFromChunk(chunk, foreground);

                                    foreach (var location in points)
                                    {
                                        try
                                        {
                                            if (location.X > this.Width - 1 || location.Y > this.Height - 1)
                                                continue;

                                            if (location.X < 0 || location.Y < 0)
                                                continue;

                                            this.Foreground[location.X, location.Y] = block;
                                        }
                                        catch
                                        {
                                            encountered_errors = true;
                                        }
                                    }
                                }
                                else
                                {
                                    var background = (Background.Id)chunk.Type;
                                    var block = new BackgroundBlock(background);

                                    foreach (var location in points)
                                    {
                                        try
                                        {
                                            if (location.X > this.Width - 1 || location.Y > this.Height - 1)
                                                continue;

                                            if (location.X < 0 || location.Y < 0)
                                                continue;

                                            this.Background[location.X, location.Y] = block;
                                        }
                                        catch
                                        {
                                            encountered_errors = true;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                encountered_errors = true;
                            }
                        }
                    }

                    break;
            }

        }

        private IEnumerable<Point> GetPos(byte[] byteArrayX, byte[] byteArrayY)
        {
            for (var i = 0; i <= byteArrayX.Length - 1; i += 2)
            {
                var x = byteArrayX[i] * 256 + byteArrayX[i + 1];
                var y = byteArrayY[i] * 256 + byteArrayY[i + 1];

                yield return new Point(x, y);
            }
        }

        private IEnumerable<Point> GetShortPos(byte[] byteArrayX, byte[] byteArrayY)
        {
            for (var i = 0; i <= byteArrayX.Length - 1; i++)
            {
                int x = byteArrayX[i];
                int y = byteArrayY[i];

                yield return new Point(x, y);
            }
        }

        private ForegroundBlock GetForegroundFromEELVLBlock(EELVL.Blocks.Block block, Foreground.Id id)
        {
            var type = WorldUtils.GetForegroundType(id);

            switch (type)
            {
                case ForegroundType.Normal:
                    return new ForegroundBlock(id);

                case ForegroundType.Note:
                    return new ForegroundBlock(id, (uint)(block as EELVL.Blocks.MusicBlock).Note);

                case ForegroundType.Goal:
                case ForegroundType.Toggle:
                case ForegroundType.ToggleGoal:
                case ForegroundType.Team:
                    return (block is EELVL.Blocks.EnumerableBlock) ? new ForegroundBlock(id, (int)(block as EELVL.Blocks.EnumerableBlock).Variant) :
                           (block is EELVL.Blocks.NumberBlock) ? new ForegroundBlock(id, (int)(block as EELVL.Blocks.NumberBlock).Number) :
                    throw new NotSupportedException("Encountered an unsupported block!");

                case ForegroundType.Morphable when block is EELVL.Blocks.MorphableBlock:
                    return new ForegroundBlock(id, (block as EELVL.Blocks.MorphableBlock).Morph);

                case ForegroundType.Morphable when block is EELVL.Blocks.RotatableBlock:
                    return new ForegroundBlock(id, (block as EELVL.Blocks.RotatableBlock).Rotation);

                case ForegroundType.Morphable when block is EELVL.Blocks.EnumerableBlock:
                    return new ForegroundBlock(id, (block as EELVL.Blocks.EnumerableBlock).Variant);

                case ForegroundType.Portal:
                    return new ForegroundBlock(id,
                        (block as EELVL.Blocks.PortalBlock).ID,
                        (block as EELVL.Blocks.PortalBlock).Target,
                        (Morph.Id)(block as EELVL.Blocks.PortalBlock).Rotation
                        );

                case ForegroundType.WorldPortal:
                    return new ForegroundBlock(id, (block as EELVL.Blocks.WorldPortalBlock).Target);

                case ForegroundType.Label:
                    return new ForegroundBlock(id,
                        (block as EELVL.Blocks.LabelBlock).Text ?? "no text found",
                        (block as EELVL.Blocks.LabelBlock).Color ?? "#FFFFFF",
                        (block as EELVL.Blocks.LabelBlock).Wrap);

                case ForegroundType.Sign:
                    return new ForegroundBlock(id,
                        (block as EELVL.Blocks.SignBlock).Text ?? "no text found.",
                        (Morph.Id)(block as EELVL.Blocks.SignBlock).Morph);

                default:
                    throw new NotSupportedException("Encountered an unsupported block!");
            }
        }

        private ForegroundBlock GetForegroundFromChunk(IBlockChunk chunk, Foreground.Id id)
        {
            var type = WorldUtils.GetForegroundType(id);

            switch (type)
            {
                case ForegroundType.Normal:
                    return new ForegroundBlock(id);

                case ForegroundType.Note:
                    return new ForegroundBlock(id,
                        (uint)chunk.Id);

                case ForegroundType.Goal:
                case ForegroundType.Toggle:
                case ForegroundType.ToggleGoal:
                case ForegroundType.Team:
                    return new ForegroundBlock(id,
                        chunk.Goal);

                case ForegroundType.Morphable:
                    return new ForegroundBlock(id,
                        chunk.Rotation);

                case ForegroundType.Portal:
                    return new ForegroundBlock(id,
                        (uint)chunk.Id,
                        (uint)chunk.Target,
                        (Morph.Id)chunk.Rotation);

                case ForegroundType.WorldPortal:
                    return new ForegroundBlock(id,
                        (string)chunk.Target);

                case ForegroundType.Label:
                    return new ForegroundBlock(id,
                        chunk.Text ?? "no text found",
                        chunk.TextColor ?? "#FFFFFF",
                        chunk.WrapLength);

                case ForegroundType.Sign:
                    return new ForegroundBlock(id,
                        chunk.Text ?? "no text found.",
                        (Morph.Id)chunk.SignType);

                default:
                    throw new NotSupportedException("Encountered an unsupported block!");
            }
        }
    }

    public class ObsoleteProperties
    {
        public int CurseLimit { get; internal set; }
        public int ZombieLimit { get; internal set; }
        public int Woots { get; internal set; }
        public int TotalWoots { get; internal set; }
        public bool AllowPotions { get; internal set; }
        public string EnabledPotions { get; internal set; }
    }
}