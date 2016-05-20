using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MiniRealms.Levels.Tiles;

namespace MiniRealms.Engine.Gfx
{
    public class SpriteSheet
    {
        private Texture2D _image;
        public readonly int Width;
        public readonly int Height;
        public readonly int[] Pixels;
        private static Dictionary<TileId, List<Sprite>> _tiles;

        public SpriteSheet(Texture2D image)
        {
            Width = image.Width;
            Height = image.Height;

            var colors = new Microsoft.Xna.Framework.Color[Width  * Height];
            image.GetData(colors);

            Pixels = new int[Width * Height];
            for (int i = 0; i < Pixels.Length; i++)
                Pixels[i] = colors[i].B / 64;
        }

        public static void LoadTiles(ContentManager manager)
        {
            var path = Path.Combine(manager.RootDirectory, "Data", "tiles.xml");

            XmlSerializer xs = new XmlSerializer(typeof(XmlDictionary<TileId, List<Sprite>>));
            MemoryStream ms = new MemoryStream(File.ReadAllBytes(path)) {Position = 0};
            _tiles = (XmlDictionary<TileId, List<Sprite>>) xs.Deserialize(ms);
        }

        public static List<Sprite> GetSprites(TileId tile)
        {
            if (!_tiles.ContainsKey(tile)) return new List<Sprite>();

            return _tiles[tile];
        }

        public void Unload()
        {
            _image = null;
        }
    }
}
