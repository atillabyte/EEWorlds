using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EELVL;

namespace EEWorlds
{
    public class EELevelWorld : WorldManager
    {
        public override WorldFormat Format => WorldFormat.EELEVEL;
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

        internal static WorldManager Load(byte[] input, int version)
        {
            var frame = LoadFrame(input, version);
            var level = ConvertToEELVL(frame);
            var blocks = new List<(int layer, int x, int y, Blocks.Block block)>();

            for (var x = 0; x < level.Width; x++)
            {
                for (var y = 0; y < level.Height; y++)
                {
                    blocks.Add((0, x, y, level[0, x, y]));
                    blocks.Add((1, x, y, level[1, x, y]));
                }
            }

            return new EELevelWorld()
            {
                BlockCollection = blocks,
                Width = level.Width,
                Height = level.Height,
                Status = (int)WorldStatus.Open
            };
        }

        internal static Level ConvertToEELVL(Frame frame)
        {
            var level = new Level(frame.Width, frame.Height, 0);

            for (var y = 0; y < frame.Height; ++y)
            {
                for (var x = 0; x < frame.Width; ++x)
                {
                    var fid = frame.Foreground[y, x];
                    var bid = frame.Background[y, x];

                    if (Blocks.IsType(fid, Blocks.BlockType.Normal))
                    {
                        level[0, x, y] = new Blocks.Block(fid);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.Number))
                    {
                        level[0, x, y] = new Blocks.NumberBlock(fid, frame.BlockData[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.NPC))
                    {
                        level[0, x, y] = new Blocks.NPCBlock(fid, frame.BlockData3[y, x], frame.BlockData4[y, x], frame.BlockData5[y, x], frame.BlockData6[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.Morphable))
                    {
                        level[0, x, y] = new Blocks.MorphableBlock(fid, frame.BlockData[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.Enumerable))
                    {
                        level[0, x, y] = new Blocks.EnumerableBlock(fid, frame.BlockData[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.Sign))
                    {
                        level[0, x, y] = new Blocks.SignBlock(fid, frame.BlockData3[y, x], frame.BlockData[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.Rotatable) || Blocks.IsType(fid, Blocks.BlockType.RotatableButNotReally))
                    {
                        level[0, x, y] = new Blocks.RotatableBlock(fid, frame.BlockData[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.Portal))
                    {
                        level[0, x, y] = new Blocks.PortalBlock(fid, frame.BlockData[y, x], frame.BlockData1[y, x], frame.BlockData2[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.WorldPortal))
                    {
                        level[0, x, y] = new Blocks.WorldPortalBlock(fid, frame.BlockData3[y, x], frame.BlockData[y, x]);
                    }
                    if (Blocks.IsType(fid, Blocks.BlockType.Music))
                    {
                        level[0, x, y] = new Blocks.MusicBlock(fid, frame.BlockData[y, x]);
                    }
                    if (Blocks.IsType(bid, Blocks.BlockType.Normal))
                    {
                        level[1, x, y] = new Blocks.Block(bid);
                    }
                }
            }

            return level;
        }

        internal static Frame LoadFrame(byte[] input, int version)
        {
            var reader = new BinaryReader(new MemoryStream(input));

            if (version == 6)
            {
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
                var f = new Frame(width, height);

                f.Foreground = new int[height, width];
                f.Background = new int[height, width];
                f.BlockData = new int[height, width];
                f.BlockData1 = new int[height, width];
                f.BlockData2 = new int[height, width];
                f.BlockData3 = new string[height, width];
                f.BlockData4 = new string[height, width];
                f.BlockData5 = new string[height, width];
                f.BlockData6 = new string[height, width];

                for (var y = 0; y < height; ++y)
                {
                    for (var x = 0; x < width; ++x)
                    {
                        int t = reader.ReadInt16();

                        f.Foreground[y, x] = t;
                        f.Background[y, x] = reader.ReadInt16();

                        if (BlockData.Goal.Contains(t) || BlockData.Rotate.Contains(t) || BlockData.Morphable.Contains(t) && t != 385 && t != 374)
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                        }
                        if (t == 385)
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                            f.BlockData3[y, x] = reader.ReadString();
                        }
                        if (t == 374)
                        {
                            f.BlockData3[y, x] = reader.ReadString();
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                        }
                        if (BlockData.Portals.Contains(t))
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt32());
                            f.BlockData1[y, x] = reader.ReadInt32();
                            f.BlockData2[y, x] = reader.ReadInt32();
                        }
                        if (BlockData.IsNPC(t))
                        {
                            f.BlockData3[y, x] = reader.ReadString();
                            f.BlockData4[y, x] = reader.ReadString();
                            f.BlockData5[y, x] = reader.ReadString();
                            f.BlockData6[y, x] = reader.ReadString();
                        }
                    }
                }
                return f;
            }
            if (version == 5)
            {
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
                var f = new Frame(width, height);

                f.Foreground = new int[height, width];
                f.Background = new int[height, width];
                f.BlockData = new int[height, width];
                f.BlockData1 = new int[height, width];
                f.BlockData2 = new int[height, width];
                f.BlockData3 = new string[height, width];
                f.BlockData4 = new string[height, width];
                f.BlockData5 = new string[height, width];
                f.BlockData6 = new string[height, width];

                for (var y = 0; y < height; ++y)
                {
                    for (var x = 0; x < width; ++x)
                    {
                        int t = reader.ReadInt16();
                        f.Foreground[y, x] = t;
                        f.Background[y, x] = reader.ReadInt16();
                        if (BlockData.Goal.Contains(t) || BlockData.Rotate.Contains(t) || BlockData.Morphable.Contains(t) && t != 385 && t != 374)
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                        }
                        if (t == 385)
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                            f.BlockData3[y, x] = reader.ReadString();
                        }
                        if (t == 374)
                        {
                            f.BlockData3[y, x] = reader.ReadString();
                        }
                        if (BlockData.Portals.Contains(t))
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt32());
                            f.BlockData1[y, x] = reader.ReadInt32();
                            f.BlockData2[y, x] = reader.ReadInt32();
                        }
                        if (BlockData.IsNPC(t))
                        {
                            f.BlockData3[y, x] = reader.ReadString();
                            f.BlockData4[y, x] = reader.ReadString();
                            f.BlockData5[y, x] = reader.ReadString();
                            f.BlockData6[y, x] = reader.ReadString();
                        }
                    }
                }
                return f;
            }
            if (version == 4)
            {
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
                var f = new Frame(width, height);

                f.Foreground = new int[height, width];
                f.Background = new int[height, width];
                f.BlockData = new int[height, width];
                f.BlockData1 = new int[height, width];
                f.BlockData2 = new int[height, width];
                f.BlockData3 = new string[height, width];
                f.BlockData4 = new string[height, width];
                f.BlockData5 = new string[height, width];
                f.BlockData6 = new string[height, width];

                for (var y = 0; y < height; ++y)
                {
                    for (var x = 0; x < width; ++x)
                    {
                        int t = reader.ReadInt16();
                        f.Foreground[y, x] = t;
                        f.Background[y, x] = reader.ReadInt16();
                        if (BlockData.Goal.Contains(t) || BlockData.Rotate.Contains(t) || BlockData.Morphable.Contains(t) && t != 385 && t != 374)
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                        }
                        if (t == 385)
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                            f.BlockData3[y, x] = reader.ReadString();
                        }
                        if (t == 374)
                        {
                            f.BlockData3[y, x] = reader.ReadString();
                        }
                        if (BlockData.Portals.Contains(t))
                        {
                            f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt32());
                            f.BlockData1[y, x] = reader.ReadInt32();
                            f.BlockData2[y, x] = reader.ReadInt32();
                        }
                    }
                }
                return f;
            }
            if (version == 3)
            {
                var filetype = reader.ReadChars(16);
                if (new string(filetype) == "ANIMATOR SAV V05")
                {
                    reader.ReadInt16();
                    int layers = Convert.ToInt16(reader.ReadInt16());
                    int width = Convert.ToInt16(reader.ReadInt16());
                    int height = Convert.ToInt16(reader.ReadInt16());
                    var f = new Frame(width, height);

                    f.Foreground = new int[height, width];
                    f.Background = new int[height, width];
                    f.BlockData = new int[height, width];
                    f.BlockData1 = new int[height, width];
                    f.BlockData2 = new int[height, width];
                    f.BlockData3 = new string[height, width];
                    f.BlockData4 = new string[height, width];
                    f.BlockData5 = new string[height, width];
                    f.BlockData6 = new string[height, width];

                    for (var z = 1; z >= 0; z += -1)
                    {
                        for (var y = 0; y <= height - 1; y++)
                        {
                            for (var x = 0; x <= width - 1; x++)
                            {
                                var blockId = ConvertFromEEAnimatorToBlockId(Convert.ToInt16(reader.ReadInt16()));

                                if (blockId >= 500 && blockId <= 900)
                                    f.Background[y, x] = blockId;
                                else
                                    f.Foreground[y, x] = blockId;
                            }
                        }
                    }
                    return f;
                }
                else
                {
                    return null;
                }
            }

            if (version >= 0 && version <= 2)
            {
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
                var f = new Frame(width, height);

                f.Foreground = new int[height, width];
                f.Background = new int[height, width];
                f.BlockData = new int[height, width];
                f.BlockData1 = new int[height, width];
                f.BlockData2 = new int[height, width];
                f.BlockData3 = new string[height, width];
                f.BlockData4 = new string[height, width];
                f.BlockData5 = new string[height, width];
                f.BlockData6 = new string[height, width];

                for (var y = 0; y < height; ++y)
                {
                    for (var x = 0; x < width; ++x)
                    {
                        if (version == 0)
                        {
                            int t = reader.ReadByte();
                            f.Foreground[y, x] = t;
                            f.Background[y, x] = 0;
                            if (BlockData.Goal.Contains(t) || BlockData.Rotate.Contains(t) || BlockData.Morphable.Contains(t))
                            {
                                f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                            }
                            else if (BlockData.Portals.Contains(t))
                            {
                                var r = reader.ReadInt32();
                                var a = r >> 16;
                                var b = ((r >> 8) & 0xFF);
                                var c = (r & 0xFF);
                                f.BlockData[y, x] = Convert.ToInt32(a);
                                f.BlockData1[y, x] = b;
                                f.BlockData2[y, x] = c;
                            }
                        }
                        else if (version == 1)
                        {
                            int t = reader.ReadInt16();
                            f.Foreground[y, x] = t;
                            f.Background[y, x] = reader.ReadInt16();
                            if (BlockData.Goal.Contains(t) || BlockData.Rotate.Contains(t) || BlockData.Morphable.Contains(t))
                            {
                                f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                            }
                            else if (BlockData.Portals.Contains(t))
                            {
                                var r = reader.ReadInt32();
                                var a = r >> 16;
                                var b = ((r >> 8) & 0xFF);
                                var c = (r & 0xFF);
                                f.BlockData[y, x] = Convert.ToInt32(a);
                                f.BlockData1[y, x] = b;
                                f.BlockData2[y, x] = c;
                            }
                        }
                        else if (version == 2)
                        {
                            int t = reader.ReadInt16();
                            f.Foreground[y, x] = t;
                            f.Background[y, x] = reader.ReadInt16();
                            if (BlockData.Goal.Contains(t) || BlockData.Rotate.Contains(t) || BlockData.Morphable.Contains(t))
                            {
                                f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt16());
                            }
                            else if (t == 374)
                            {
                                f.BlockData[y, x] = 0;
                                f.BlockData3[y, x] = reader.ReadString();
                            }
                            else if (t == 385)
                            {
                                f.BlockData3[y, x] = reader.ReadString();
                            }
                            else if (BlockData.Portals.Contains(t))
                            {
                                f.BlockData[y, x] = Convert.ToInt32(reader.ReadInt32());
                                f.BlockData1[y, x] = reader.ReadInt32();
                                f.BlockData2[y, x] = reader.ReadInt32();
                            }
                        }
                    }
                }

                return f;
            }
            else
            {
                throw new NotImplementedException("The EEditor EELEVEL format version '" + version + "' has not been implemented.");
            }
        }

        private static int ConvertFromEEAnimatorToBlockId(int id)
        {
            if (id == 127)
            {
                return 0;
            }
            else if (id - 128 >= 0 && id - 128 <= 63)
            {
                return id - 128;
            }
            else if (id + 256 >= 500 && id + 256 <= 600)
            {
                return id + 256;
            }
            else
            {
                return id - 1024;
            }
        }

        internal class BlockData
        {
            internal static int[] Goal = { 77, 83, 43, 165, 213, 214, 417, 418, 419, 420, 421, 422, 423, 1027, 1028, 113, 185, 184, 1011, 1012, 453, 461, 467, 1079, 1080, 1520, 1582, 1619, 1620 };
            internal static int[] Rotate = { 1001, 1002, 1003, 1004, 1027, 1028, 361, 385, 374, 1052, 1053, 1054, 1055, 1056, 1092, 1625, 1627, 1629, 1631, 1633, 1635 };
            internal static int[] Ignore = { 1001, 1002, 1003, 1004, 361, 417, 418, 419, 420, 1052, 1053, 1054, 1055, 1056, 1092, 1625, 1627, 1629, 1631, 1633, 1635 };

            internal static int[] Morphable = {
            375, 376, 379, 380, 377, 378, 438, 439, 276, 277, 279, 280, 440, 275,
            329, 273, 328, 327, 338, 339, 340, 1041, 1042, 1043, 456, 457, 458, 447, 448, 449, 450, 451, 452,
            464, 465, 1075, 1076, 1077, 1078, 471, 475, 476, 477, 481, 482, 483, 497, 492, 493, 494, 499, 1500,
            1502, 1507, 1506, 1101, 1102, 1103, 1104, 1105, 1517, 1116, 1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124, 1125, 1135,
            1134, 1538, 1536, 1537, 1538, 1140, 1141, 1535, 1581, 1587, 1588, 1155, 1160, 1592, 1593, 1594, 1595, 1596, 1597, 1584, 1605, 1606, 1607, 1609, 1610, 1611, 1612, 1614, 1615, 1616, 1617 };

            internal static int[] Portals = { 242, 381 };
            internal static int[] Sound = { 77, 83, 1520 };

            internal static int[] increase3 = {
            1001, 1002, 1003, 1004, 361, 375, 376, 377, 378, 379, 380, 438, 439, 275,
            329, 273, 328, 327, 338, 339, 340, 1041, 1042, 1043, 447, 448, 449, 450, 451, 452, 1052, 1053, 1054, 1055,
            1056, 1075, 1076, 1077, 1078, 1092, 492, 493, 494, 499, 1502, 1116, 1117, 1118, 1119, 1120, 1121, 1122, 1123,
            1124, 1125, 1537, 1140, 1141, 1155, 1160, 1592, 1593, 1594, 1595, 1596, 1597, 1605, 1607, 1609, 1610, 1612, 1614, 1615, 1616, 1617, 1625, 1627, 1629, 1631, 1633, 1635};

            internal static int[] increase2 = { 417, 276, 277, 279, 280, 471, 475, 476, 477, 483, 1134, 419 };
            internal static int[] increase1 = { 418, 420, 453, 456, 457, 458, 1135, 1536, 1535, 1500, 1587, 1606, 1611 };
            internal static int[] increase4 = { 1507, 1506, 464, 465, 1588, 1517 };
            internal static int[] increase5 = { 440, 481, 482, 497, 1581 };
            internal static int[] increase11 = { 1538 };

            internal static bool IsBG(int id) => (id >= 500 && id <= 999) || ((id < 500 || id >= 1001) && false);
            internal static bool IsNPC(int id) => (id >= 1550 && id <= 1559) || (id >= 1569 && id <= 1579);
        }
    }
}