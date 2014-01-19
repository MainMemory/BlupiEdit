using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;

namespace BlupiEdit
{
    public class LevelItem
    {
        public ItemTypes Type { get; set; }
        [DisplayName("A to B time")]
        public ushort ABTime { get; set; }
        [DisplayName("A to B speed")]
        public int ABSpeed
        {
            get
            {
                return (int)Math.Round(LevelData.Distance(PointA, PointB) / ABTime);
            }
            set
            {
                ABTime = (ushort)Math.Round(LevelData.Distance(PointA, PointB) / value);
            }
        }
        [DisplayName("B to A time")]
        public ushort BATime { get; set; }
        [DisplayName("B to A speed")]
        public int BASpeed
        {
            get
            {
                return (int)Math.Round(LevelData.Distance(PointB, PointA) / BATime);
            }
            set
            {
                BATime = (ushort)Math.Round(LevelData.Distance(PointB, PointA) / value);
            }
        }
        [DisplayName("A Wait")]
        public ushort AWait { get; set; }
        [DisplayName("B Wait")]
        public ushort BWait { get; set; }
        public ushort field_A { get; set; }
        [DisplayName("Point A")]
        public Point PointA { get; set; }
        [DisplayName("Point B")]
        public Point PointB { get; set; }
        [DisplayName("Point C?")]
        public Point PointC { get; set; }
        public ushort field_24 { get; set; }
        public ushort field_26 { get; set; }
        public ushort field_28 { get; set; }
        [DisplayName("Art File")]
        public TileTypes ArtFile { get; set; }
        public ushort Tile { get; set; }
        public ushort field_2E { get; set; }

        public LevelItem(BinaryReader reader)
        {
            Type = (ItemTypes)reader.ReadUInt16();
            ABTime = reader.ReadUInt16();
            BATime = reader.ReadUInt16();
            AWait = reader.ReadUInt16();
            BWait = reader.ReadUInt16();
            field_A = reader.ReadUInt16();
            PointA = new Point(reader.ReadInt32(), reader.ReadInt32());
            PointB = new Point(reader.ReadInt32(), reader.ReadInt32());
            PointC = new Point(reader.ReadInt32(), reader.ReadInt32());
            field_24 = reader.ReadUInt16();
            field_26 = reader.ReadUInt16();
            field_28 = reader.ReadUInt16();
            ArtFile = (TileTypes)reader.ReadUInt16();
            Tile = reader.ReadUInt16();
            field_2E = reader.ReadUInt16();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((ushort)Type);
            writer.Write(ABTime);
            writer.Write(BATime);
            writer.Write(AWait);
            writer.Write(BWait);
            writer.Write(field_A);
            writer.Write(PointA.X); writer.Write(PointA.Y);
            writer.Write(PointB.X); writer.Write(PointB.Y);
            writer.Write(PointC.X); writer.Write(PointC.Y);
            writer.Write(field_24);
            writer.Write(field_26);
            writer.Write(field_28);
            writer.Write((ushort)ArtFile);
            writer.Write(Tile);
            writer.Write(field_2E);
        }
    }

    public class TileInfo
    {
        public Rectangle Location { get; set; }
        public Point Offset { get; set; }

        public TileInfo(BinaryReader reader)
        {
            Point loc = new Point(reader.ReadInt16(), reader.ReadInt16());
            Offset = new Point(reader.ReadInt16(), reader.ReadInt16());
            Location = new Rectangle(loc, new Size(reader.ReadInt16(), reader.ReadInt16()));
        }

        public static TileInfo[] Read(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            TileInfo[] result = new TileInfo[reader.ReadInt16()];
            for (int i = 0; i < result.Length; i++)
                result[i] = new TileInfo(reader);
            return result;
        }
    }

    public class LevelData
    {
        #region Instance Members
        public byte MajorVersion { get; set; }
        public byte MinorVersion { get; set; }
        public bool HorizontalScroll { get; set; }
        public bool VerticalScroll { get; set; }
        public short Music { get; set; }
        public short Background { get; set; }
        public Point[] StartPositions { get; private set; }
        public bool[] StartDirections { get; private set; }
        public string LevelName { get; set; }
        public short[,] Tiles { get; private set; }
        public short[,] Tiles2 { get; private set; }
        public List<LevelItem> Items { get; private set; }

        public LevelData(string filename)
        {
            using (FileStream fs = File.OpenRead(filename))
            using (BinaryReader reader = new BinaryReader(fs, Encoding.ASCII))
            {
                MajorVersion = reader.ReadByte();
                MinorVersion = reader.ReadByte();
                fs.Seek(0xD4, SeekOrigin.Begin);
                HorizontalScroll = reader.ReadInt32() == 100;
                VerticalScroll = reader.ReadInt32() == 100;
                fs.Seek(2, SeekOrigin.Current);
                Music = reader.ReadInt16();
                Background = reader.ReadInt16();
                fs.Seek(0x148, SeekOrigin.Begin);
                StartPositions = new Point[4];
                for (int i = 0; i < 4; i++)
                    StartPositions[i] = new Point(reader.ReadInt32(), reader.ReadInt32());
                StartDirections = new bool[4];
                for (int i = 0; i < 4; i++)
                    StartDirections[i] = reader.ReadInt32() != 0;
                LevelName = reader.ReadString(40);
                fs.Seek(0x364, SeekOrigin.Begin);
                Tiles = new short[100, 100];
                for (int x = 0; x < 100; x++)
                    for (int y = 0; y < 100; y++)
                        Tiles[x, y] = reader.ReadInt16();
                Tiles2 = new short[100, 100];
                for (int x = 0; x < 100; x++)
                    for (int y = 0; y < 100; y++)
                        Tiles2[x, y] = reader.ReadInt16();
                Items = new List<LevelItem>();
                for (int i = 0; i < 200; i++)
                {
                    LevelItem item = new LevelItem(reader);
                    if (item.Type != ItemTypes.None)
                        Items.Add(item);
                }
            }
        }

        public void Save(string filename)
        {
            using (FileStream fs = File.Create(filename))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(MajorVersion);
                writer.Write(MinorVersion);
                writer.Seek(0xD4, SeekOrigin.Begin);
                writer.Write(HorizontalScroll ? 100 : 0);
                writer.Write(VerticalScroll ? 100 : 0);
                writer.Seek(2, SeekOrigin.Current);
                writer.Write(Music);
                writer.Write(Background);
                fs.Seek(0x148, SeekOrigin.Begin);
                for (int i = 0; i < 4; i++)
                {
                    writer.Write(StartPositions[i].X);
                    writer.Write(StartPositions[i].Y);
                }
                for (int i = 0; i < 4; i++)
                    writer.Write(StartDirections[i] ? 1 : 0);
                writer.WriteString(LevelName, 40);
                fs.Seek(0x364, SeekOrigin.Begin);
                for (int x = 0; x < 100; x++)
                    for (int y = 0; y < 100; y++)
                        writer.Write(Tiles[x, y]);
                for (int x = 0; x < 100; x++)
                    for (int y = 0; y < 100; y++)
                        writer.Write(Tiles2[x, y]);
                for (int i = 0; i < Items.Count; i++)
                    Items[i].Write(writer);
                if (Items.Count < 200)
                    writer.Write(new byte[0x30 * (200 - Items.Count)]);
            }
        }
        #endregion
        #region Static Members
        public const int LevelSize = 100;
        public const int GridSize = 64;
        public const int PixelSize = LevelSize * GridSize;
        public static bool IsBlupi2 { get; private set; }
        public static string CurrentLevelPath { get; private set; }
        public static LevelData CurrentLevel { get; private set; }
        public static TileInfo[] BlupiTiles { get; private set; }
        public static TileInfo[] ObjectTiles { get; private set; }
        public static TileInfo[] ElementTiles { get; private set; }
        public static TileInfo[] ExploTiles { get; private set; }
        public static Dictionary<TileTypes, Sprite[]> TileImages { get; private set; }

        public static void LoadGame(string filename)
        {
            IsBlupi2 = true;
            Environment.CurrentDirectory = Path.GetDirectoryName(Path.GetFullPath(filename));
            using (FileStream fs = File.OpenRead(filename))
            {
                if (IsBlupi2)
                {
                    fs.Seek(0x862F0, SeekOrigin.Begin);
                    BlupiTiles = TileInfo.Read(fs);
                    fs.Seek(0x872B8, SeekOrigin.Begin);
                    ObjectTiles = TileInfo.Read(fs);
                    fs.Seek(0x88768, SeekOrigin.Begin);
                    ElementTiles = TileInfo.Read(fs);
                    fs.Seek(0x894F8, SeekOrigin.Begin);
                    ExploTiles = TileInfo.Read(fs);
                }
                TileImages = new Dictionary<TileTypes, Sprite[]>();
                List<Sprite> tmp = new List<Sprite>(BlupiTiles.Length);
                using (Bitmap bmp = new Bitmap(@"IMAGE08\blupi000.blp"))
                {
                    bmp.MakeTransparent(Color.Blue);
                    foreach (TileInfo item in BlupiTiles)
                        tmp.Add(new Sprite(bmp.Clone(item.Location, bmp.PixelFormat), item.Offset));
                }
                TileImages.Add(TileTypes.Blupi000, tmp.ToArray());
                tmp = new List<Sprite>(BlupiTiles.Length);
                using (Bitmap bmp = new Bitmap(@"IMAGE08\blupi001.blp"))
                {
                    bmp.MakeTransparent(Color.Blue);
                    foreach (TileInfo item in BlupiTiles)
                        tmp.Add(new Sprite(bmp.Clone(item.Location, bmp.PixelFormat), item.Offset));
                }
                TileImages.Add(TileTypes.Blupi001, tmp.ToArray());
                tmp = new List<Sprite>(BlupiTiles.Length);
                using (Bitmap bmp = new Bitmap(@"IMAGE08\blupi002.blp"))
                {
                    bmp.MakeTransparent(Color.Blue);
                    foreach (TileInfo item in BlupiTiles)
                        tmp.Add(new Sprite(bmp.Clone(item.Location, bmp.PixelFormat), item.Offset));
                }
                TileImages.Add(TileTypes.Blupi002, tmp.ToArray());
                tmp = new List<Sprite>(BlupiTiles.Length);
                using (Bitmap bmp = new Bitmap(@"IMAGE08\blupi003.blp"))
                {
                    bmp.MakeTransparent(Color.Blue);
                    foreach (TileInfo item in BlupiTiles)
                        tmp.Add(new Sprite(bmp.Clone(item.Location, bmp.PixelFormat), item.Offset));
                }
                TileImages.Add(TileTypes.Blupi003, tmp.ToArray());
                tmp = new List<Sprite>(ObjectTiles.Length);
                using (Bitmap bmp = new Bitmap(@"IMAGE16\object.blp"))
                {
                    bmp.MakeTransparent(Color.Blue);
                    foreach (TileInfo item in ObjectTiles)
                        tmp.Add(new Sprite(bmp.Clone(item.Location, bmp.PixelFormat), item.Offset));
                }
                TileImages.Add(TileTypes.Object, tmp.ToArray());
                tmp = new List<Sprite>(ElementTiles.Length);
                using (Bitmap bmp = new Bitmap(@"IMAGE16\element.blp"))
                {
                    bmp.MakeTransparent(Color.Blue);
                    foreach (TileInfo item in ElementTiles)
                        tmp.Add(new Sprite(bmp.Clone(item.Location, bmp.PixelFormat), item.Offset));
                }
                TileImages.Add(TileTypes.Element, tmp.ToArray());
                tmp = new List<Sprite>(ExploTiles.Length);
                using (Bitmap bmp = new Bitmap(@"IMAGE16\explo.blp"))
                {
                    bmp.MakeTransparent(Color.Blue);
                    foreach (TileInfo item in ExploTiles)
                        tmp.Add(new Sprite(bmp.Clone(item.Location, bmp.PixelFormat), item.Offset));
                }
                TileImages.Add(TileTypes.Explo, tmp.ToArray());
            }
        }

        public static void LoadLevel(int userid, int levelnum, bool userlevel)
        {
            CurrentLevelPath = GetLevelName(userid, levelnum, userlevel);
            CurrentLevel = new LevelData(CurrentLevelPath);
        }

        public static string GetLevelName(int userid, int levelnum, bool userlevel)
        {
            if (userlevel)
                return string.Format("data\\u{0:000}-{1:000}.blp", userid, levelnum);
            else
                return string.Format("data\\world{0:000}.blp", levelnum);
        }

        public static void SaveLevel()
        {
            CurrentLevel.Save(CurrentLevelPath);
        }

        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        public static Bitmap LoadImage(string filename)
        {
            if (File.Exists(string.Format(@"IMAGE16\{0}.blp", filename)))
                return new Bitmap(string.Format(@"IMAGE16\{0}.blp", filename));
            else
                return new Bitmap(string.Format(@"IMAGE08\{0}.blp", filename));
        }
        #endregion
    }

    public enum ItemTypes : ushort
    {
        None = 0,
        Lift = 1,
        Bomb = 2,
        HangingBomb = 3,
        Bulldozer = 4,
        TreasureChest = 5,
        Egg = 6,
        Goal = 7,
        Explosion1 = 8,
        Explosion2 = 9,
        Explosion3 = 10,
        Explosion4 = 11,
        WoodenCase = 12,
        Helicopter = 13,
        Splash = 14,
        Bubbles = 15,
        MovingBomb = 16,
        Fish = 17,
        Jeep = 19,
        Bird = 20,
        Key = 21,
        Skateboard = 24,
        Shield = 25,
        Lollypop = 26,
        Sparkles = 27,
        GlueTank = 28,
        GlueSupply = 29,
        InvisibilityPotion = 30,
        RechargingDevice = 31,
        HeliportedEnemy = 32,
        MotorizedEnemy = 33,
        StuckEnemy = 34,
        Inverter = 40,
        Wasp = 44,
        Hovercraft = 46,
        LiftWithConveyerBelt = 47,
        RedKey = 49,
        GreenKey = 50,
        BlueKey = 51,
        SlimeCreature = 54,
        Dynamite = 55,
        HomingBomb = 96,
        YellowBomb = 200,
        OrangeBomb = 201,
        BlueBomb = 202,
        GreenBomb = 203,
    }

    public enum TileTypes : ushort
    {
        None = 0,
        Object = 1,
        Blupi000 = 2,
        Explo = 9,
        Element = 10,
        Blupi001 = 11,
        Blupi002 = 12,
        Blupi003 = 13,
    }

    public struct Sprite
    {
        public Point Offset;
        public Bitmap Image;
        public int X { get { return Offset.X; } set { Offset.X = value; } }
        public int Y { get { return Offset.Y; } set { Offset.Y = value; } }
        public int Width { get { return Image.Width; } }
        public int Height { get { return Image.Height; } }
        public Size Size { get { return Image.Size; } }
        public int Left { get { return X; } }
        public int Top { get { return Y; } }
        public int Right { get { return X + Width; } }
        public int Bottom { get { return Y + Height; } }
        public Rectangle Bounds { get { return new Rectangle(Offset, Size); } }

        public Sprite(Bitmap spr, Point off)
        {
            Image = spr;
            Offset = off;
        }

        public Sprite(Sprite sprite)
        {
            Image = (Bitmap)sprite.Image.Clone();
            Offset = sprite.Offset;
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Reads a null-terminated ASCII string from the current stream and advances the position by <paramref name="length"/> bytes.
        /// </summary>
        /// <param name="length">The maximum length of the string, in bytes.</param>
        public static string ReadString(this BinaryReader br, int length)
        {
            byte[] buffer = br.ReadBytes(length);
            for (int i = 0; i < length; i++)
                if (buffer[i] == 0)
                    return Encoding.ASCII.GetString(buffer, 0, i);
            return Encoding.ASCII.GetString(buffer);
        }

        public static void WriteString(this BinaryWriter bw, string value, int length)
        {
            if (value.Length > length)
                value = value.Substring(0, length);
            bw.Write(Encoding.ASCII.GetBytes(value));
            if (length > value.Length)
                bw.Write(new byte[length - value.Length]);
        }

        public static void DrawSprite(this Graphics gfx, Sprite spr, Point point)
        {
            gfx.DrawImage(spr.Image, point.X + spr.Offset.X, point.Y + spr.Offset.Y, spr.Width, spr.Height);
        }
    }
}