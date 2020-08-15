namespace EEWorlds
{
    internal class Frame
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int[,] Foreground { get; set; }
        public int[,] Background { get; set; }
        public int[,] BlockData { get; set; }
        public int[,] BlockData1 { get; set; }
        public int[,] BlockData2 { get; set; }
        public string[,] BlockData3 { get; set; }
        public string[,] BlockData4 { get; set; }
        public string[,] BlockData5 { get; set; }
        public string[,] BlockData6 { get; set; }
        public string nickname { get; set; }
        public string owner { get; set; }
        public string levelname { get; set; }
        public static byte[] xx;
        public static byte[] yy;
        public static byte[] xx1;
        public static byte[] yy1;
        public static string[] split1;

        public Frame(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}