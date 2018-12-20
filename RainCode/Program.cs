using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RainCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
            Console.ReadKey();
        }

        public void Reset()
        {
            rpxs = 1;
            dpxs = 1;
            rxpos = 0;
            rypos = 0;
            dxpos = 0;
            dypos = 0;
        }

        public void Run()
        {
            while (true)
            {
                Reset();
                Console.Title = "RainCode v1.0";
                Console.WriteLine(@"__________       .__       _________            .___      ");
                Console.WriteLine(@"\______   _____  |__| ____ \_   ___ \  ____   __| _/____  ");
                Console.WriteLine(@" |       _\__  \ |  |/    \/    \  \/ /  _ \ / __ _/ __ \ ");
                Console.WriteLine(@" |    |   \/ __ \|  |   |  \     \___(  <_> / /_/ \  ___/ ");
                Console.WriteLine(@" |____|_  (____  |__|___|  /\______  /\____/\____ |\___  >");
                Console.WriteLine(@"        \/     \/        \/        \/            \/    \/ ");
                Console.WriteLine("                    R a i n C o d e  v 1 . 0               ");
                Console.WriteLine("===========================================================\n\n");
                Console.WriteLine("Press 'M' to make raincode");
                Console.WriteLine("Press 'R' to read raincode");

                var Input = Console.ReadKey();
                if (Input.Key == ConsoleKey.M)
                {
                    Console.Clear();
                    Console.WriteLine("Enter input file's path: ");
                    string InputPath = Console.ReadLine();
                    Console.WriteLine("Enter destination path: ");
                    string DestinationPath = Console.ReadLine();
                    try
                    {
                        WriteImage(InputPath, DestinationPath);
                    }
                    catch (Exception ex) { Console.WriteLine("ERROR: " + ex.Message); }
                }
                if (Input.Key == ConsoleKey.R)
                {
                    Console.Clear();
                    Console.WriteLine("Enter input file's path: ");
                    string InputPath = Console.ReadLine();
                    Console.WriteLine("Enter destination path: ");
                    string DestinationPath = Console.ReadLine();
                    try
                    {
                        byte[] Data = ReadImage(InputPath);
                        System.IO.File.WriteAllBytes(DestinationPath, Data);
                        Console.WriteLine("Done.");
                    }
                    catch (Exception ex) { Console.WriteLine("ERROR: " + ex.Message); }
                }
                Console.Clear();
            }
        }

        public static void WriteImage(string DataPath, string DestImagePath)
        {
            int[] Lookup = { 1, 2, 4, 8, 16, 32, 64, 128 };
            string[] DataColors = new string[] { "#FF0000", "#FF6600", "#FFCC00", "#CCFF00", "#00FF00", "#00FFCC", "#0000FF", "#CC00FF" };
            string[] PaddingColors = { "#FF0066", "#000000" };
            byte[] Array = System.IO.File.ReadAllBytes(DataPath);
            List<Color> ColorList = new List<Color>();

            foreach (var Char in Array)
            {
                char[] CharList = Convert.ToString(System.Convert.ToInt32(Char), 2).PadLeft(8, '0').ToArray();
                for (int i = 0; i < CharList.Length; i++)
                {
                    if (CharList[i] == '1')
                        ColorList.Add((Color)new ColorConverter().ConvertFromString(DataColors.Reverse().ToArray()[i].ToUpper()));
                }
                ColorList.Add((Color)new ColorConverter().ConvertFromString(PaddingColors[0].ToUpper()));
            }
            ColorList.Add((Color)new ColorConverter().ConvertFromString(PaddingColors[1].ToUpper()));

            Image AltImage = new Bitmap((int)Math.Ceiling(Math.Sqrt(ColorList.Count)), (int)Math.Ceiling(Math.Sqrt(ColorList.Count)));
            Graphics graphics = Graphics.FromImage(AltImage);
            graphics.FillRectangle(new SolidBrush((Color)new ColorConverter().ConvertFromString(PaddingColors[0])), 0, 0, AltImage.Width, AltImage.Height);
            for (int i = 0; i < ColorList.Count; i++)
            {
                Draw(new SolidBrush(ColorList[i]), ref graphics, AltImage.Size);
            }

            AltImage.Save(DestImagePath);
        }



        public static byte[] ReadImage(string ImagePath)
        {
            Image Image = Image.FromFile(ImagePath);
            Bitmap b = new Bitmap(Image);
            int[] Lookup = { 1, 2, 4, 8, 16, 32, 64, 128 };
            string[] DataColors = new string[] { "#FF0000", "#FF6600", "#FFCC00", "#CCFF00", "#00FF00", "#00FFCC", "#0000FF", "#CC00FF" };
            string[] PaddingColors = { "#FF0066", "#000000" };

            List<byte> Data = new List<byte>();
            int Value = 0;

            for (int x = 0; x < (Image.Width * Image.Height) / rpxs; x++)
            {
                string PixelData = Read(ref b, b.Size);
                if (PixelData != "#FFFFFF" && !string.IsNullOrWhiteSpace(PixelData))
                {
                    if (!PaddingColors.Contains(PixelData))
                    {
                        Value += Lookup[DataColors.ToList().IndexOf(PixelData)];
                    }
                    else
                    {
                        Data.Add((byte)Value);
                        Value = 0;
                    }
                }
            }
            return Data.ToArray();
        }

        public static int rpxs = 1;
        public static int rxpos = 0;
        public static int rypos = 0;

        public static string Read(ref Bitmap Bitmap, Size ImageSize)
        {
            string Hex = "";
            if (rypos != ImageSize.Height && rxpos != ImageSize.Width)
            {
                Color cc = Bitmap.GetPixel(rxpos, rypos);
                Hex = HexConverter(cc);

                rxpos += rpxs;

                return Hex;
            }

            if (rxpos + (rpxs) > ImageSize.Width)
            {
                rxpos = 0;
                rypos = rypos + rpxs;
            }

            return Hex;

        }
        public static int dpxs = 1;
        public static int dxpos = 0;
        public static int dypos = 0;
        public static Color LastDrawnColor = Color.FromArgb(0, 0, 0, 0);

        public static void Draw(SolidBrush Color, ref Graphics graphics, Size ImageSize)
        {

            if (dxpos + (dpxs) > ImageSize.Width)
            {
                dxpos = 0;
                dypos = dypos + dpxs;
            }
            if (Color.Color != LastDrawnColor || LastDrawnColor != System.Drawing.Color.FromArgb(0, 0, 0, 0))
                graphics.FillRectangle(Color, new Rectangle(new Point(dxpos, dypos), new Size(dpxs, dpxs)));

            dxpos += dpxs;
            LastDrawnColor = Color.Color;
        }

        private static string HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

    }
}
