using System;
using System.Xml;
using System.Xml.Serialization;

namespace MiniRealms.Engine.Gfx
{
    [Serializable]
    public class Sprite
    {
        [XmlElement("ID")]
        public int Img;
        [XmlElement("Color")]
        public int Col;
        [XmlElement("Rotation")]
        public int Bits;

        public Sprite()
        {
            
        }

        public Sprite(int img, int col, int bits)
        {
            Img = img;
            Col = col;
            Bits = bits;
        }
    }
}
