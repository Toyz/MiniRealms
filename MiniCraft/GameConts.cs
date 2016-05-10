using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using static System.IO.Path;

namespace MiniRealms
{
    [Serializable]
    [XmlRoot("options")]
    public class GameConts
    {
        private static GameConts _instance { get; set; }
        public static GameConts Instance => _instance ?? (_instance = new GameConts());

        [DefaultValue(5), XmlElement("WindowScale")]
        public int BaseScaling { get; set; } = 5;

        [DefaultValue(3), XmlElement("GameScale")]
        public int Scale { get; set; } = 3;

        [DefaultValue(0.50f), XmlElement("Volume")]
        public float Volume { get; set; } = 0.50f;

        [DefaultValue(false), XmlElement("Borderless")]
        public bool Borderless { get; set; }

        [DefaultValue(false), XmlElement("Fullscreen")]
        public bool FullScreen { get; set; }

        [NonSerialized] [XmlIgnore] public int MaxHeight = 256;

        [NonSerialized] [XmlIgnore] public int MaxWidth = 256;

        [NonSerialized] [XmlIgnore] public static string Name = "MiniRealms";

        [NonSerialized] [XmlIgnore] public static string Version = "0.0.1 - Alpha";

        [NonSerialized] [XmlIgnore] public static int Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/
                                                               Instance.BaseScaling;

        [NonSerialized] [XmlIgnore] public static int Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/
                                                              Instance.BaseScaling;

        [NonSerialized] [XmlIgnore] public static int ScreenMiddleWidth = Width/2;
        [NonSerialized] [XmlIgnore] public static int ScreenMiddleHeight = Height/2;

        [NonSerialized] [XmlIgnore] public static int ScreenThirdWidth = Width/3;
        [NonSerialized] [XmlIgnore] public static int ScreenThirdHeight = Height/2;

        [NonSerialized] [XmlIgnore] public static string Settings =
            Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Name, "settings.conf");

        public void Save()
        {
            if (!Directory.Exists(GetDirectoryName(Settings)))
            {
                Directory.CreateDirectory(GetDirectoryName(Settings));
            }
            File.WriteAllText(Settings, Engine.XmlHelpers.Serialize(Instance));
        }

        public void Load()
        {
            if (!Directory.Exists(GetDirectoryName(Settings)))
            {
                Directory.CreateDirectory(GetDirectoryName(Settings));
            }
            if (File.Exists(Settings))
                _instance = (GameConts) Engine.XmlHelpers.Deserialize(File.ReadAllText(Settings), typeof(GameConts));
        }
    }
}
