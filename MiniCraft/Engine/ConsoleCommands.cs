using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GameConsole.ManualInterpreter;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.LevelGens;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;
using MiniRealms.Levels.Tiles;
using MiniRealms.Screens.GameScreens;
using Color = System.Drawing.Color;

namespace MiniRealms.Engine
{
    public class ConsoleCommands
    {
        private readonly McGame _game;
        public readonly ManualInterpreter ManualInterpreter = new ManualInterpreter();

        public ConsoleCommands(McGame game)
        {
            _game = game;
            ManualInterpreter.RegisterCommand("game-seed", _ => $"Current game seed is: {LevelGen.Seed}");
            ManualInterpreter.RegisterCommand("give-item", GiveItemCommand);
            ManualInterpreter.RegisterCommand("spawn-mob", SpawnMobCommand);
            ManualInterpreter.RegisterCommand("kill-me", KillMeCommand);
            ManualInterpreter.RegisterCommand("save-image", SaveWorldImageCommand);
            ManualInterpreter.RegisterCommand("play-sound", PlaySoundCommand);
            ManualInterpreter.RegisterCommand("goto-level", MoveToLevel);
            ManualInterpreter.RegisterCommand("player-color", NewRandomPlayerColorCommand);
#if DEBUG
            ManualInterpreter.RegisterCommand("etx", ExportTileXmlCommand);
#endif

            //Normal commands XD
            ManualInterpreter.RegisterCommand("clear", _ => game.Console.Clear());
            ManualInterpreter.RegisterCommand("exit", _ => game.Exit());
            ManualInterpreter.RegisterCommand("fps", _ => $"Current FPS: {game.FpsCounterComponent.FrameRate}");
        }

        private static string ExportTileXmlCommand(string[] arg)
        {

            var tileDict = new XmlDictionary<TileId, List<Sprite>>
            {
                {
                    TileId.Cactus, new List<Sprite>
                    {
                        new Sprite(8 + 2*32, Gfx.Color.Get(20, 40, 50, 550), 0),
                        new Sprite(9 + 2*32, Gfx.Color.Get(20, 40, 50, 550), 0),
                        new Sprite(8 + 3*32, Gfx.Color.Get(20, 40, 50, 550), 0),
                        new Sprite(9 + 3*32, Gfx.Color.Get(20, 40, 50, 550), 0)
                    }
                },
                {
                    TileId.Flower, new List<Sprite>
                    {
                        new Sprite(1 + 1*32, Gfx.Color.Get(10, 141, 555, 440), 0)
                    }
                },
                {
                    TileId.RedFlower, new List<Sprite>
                    {
                        new Sprite(1 + 1*32, Gfx.Color.Get(10, 141, 300, 440), 0)
                    }
                },
                {
                    TileId.YellowFlower, new List<Sprite>
                    {
                        new Sprite(1 + 1*32, Gfx.Color.Get(10, 141, 550, 440), 0)
                    }
                },
                {
                    TileId.Dirt, new List<Sprite>
                    {
                        new Sprite(0, -1, 0),
                        new Sprite(1, -1, 0),
                        new Sprite(2, -1, 0),
                        new Sprite(3, -1, 0),
                    }
                },
                {
                    TileId.Tree, new List<Sprite>
                    {
                        new Sprite(10 + 0 * 32, -1, 0), //0
                        new Sprite(10 + 1 * 32, -1, 0), //1
                        new Sprite(10 + 2 * 32, -1, 0), //2
                        new Sprite(10 + 3 * 32, -1, 0), //3
                        new Sprite(9 + 0 * 32, -1, 0), //4
                        new Sprite(9 + 1 * 32, -1, 0) //5
                    }
                },
                {
                    TileId.Farmland, new List<Sprite>
                    {
                        new Sprite(2+32, -1, 1),
                        new Sprite(2+32, -1, 0)
                    }
                },
                {
                    TileId.CactusSapling, new List<Sprite>
                    {
                        new Sprite(11 + 3 * 32, -1, 0)
                    }
                },
                {
                    TileId.TreeSapling, new List<Sprite>
                    {
                        new Sprite(11 + 3 * 32, -1, 0)
                    }
                },
                {
                    TileId.Rock, new List<Sprite>
                    {
                        new Sprite(0, -1, 0), //0
                        new Sprite(7 + 0*32, -1, 0), //1
                        new Sprite(1, -1, 0), //2
                        new Sprite(8 + 0*32, -1, 0), //3
                        new Sprite(2, -1, 0), //4
                        new Sprite(7 + 1*32, -1, 0), //5
                        new Sprite(3, -1, 0), //6
                        new Sprite(8 + 1*32, -1, 0), //7
                    }
                },
                {
                    TileId.Grass, new List<Sprite>
                    {
                        new Sprite(0, Gfx.Color.Get(141, 141, 141 + 111, 141 + 111), 0), //0
                        new Sprite(1, Gfx.Color.Get(141, 141, 141 + 111, 141 + 111), 0), //1
                        new Sprite(2, Gfx.Color.Get(141, 141, 141 + 111, 141 + 111), 0), //2
                        new Sprite(3, Gfx.Color.Get(141, 141, 141 + 111, 141 + 111), 0), //3
                    }
                },
                {
                    //Gfx.Color.Get(444, 444, 555, 555) //norlam
                    //Gfx.Color.Get(333, 444, 555, -1) //trans
                    TileId.Cloud, new List<Sprite>
                    {
                        new Sprite(17, Gfx.Color.Get(444, 444, 555, 555), 0), //0
                        new Sprite(7 + 0*32, Gfx.Color.Get(333, 444, 555, -1), 3), //1
                        new Sprite(18, Gfx.Color.Get(444, 444, 555, 555), 0), //2
                        new Sprite(8 + 0*32, Gfx.Color.Get(333, 444, 555, -1), 3), //3
                        new Sprite(20, Gfx.Color.Get(444, 444, 555, 555), 0), //4
                        new Sprite(7 + 1*32, Gfx.Color.Get(333, 444, 555, -1), 3), //5
                        new Sprite(19, Gfx.Color.Get(444, 444, 555, 555), 0), //6
                        new Sprite(8 + 0*32, Gfx.Color.Get(333, 444, 555, -1), 3), //7
                    }
                }
            };

            FileStream ms = new FileStream("tiles.xml", FileMode.CreateNew);
            XmlSerializer xs = new XmlSerializer(tileDict.GetType());
            xs.Serialize(ms, tileDict);
            ms.Close();

            return "Saved XML";
        }

        private string NewRandomPlayerColorCommand(string[] arg)
        {
            if (_game.Player == null)
            {
                return "Player doesn't exist in current game";
            }

            Player player = _game.Player;

            var rnd = new Random();
            if(arg.Length <= 0)
                player.ChangePlayerColor(100, rnd.NextInt(555) + 1, rnd.NextInt(555) + 1);
            else
            {
                if (arg[0].Equals("skin", StringComparison.OrdinalIgnoreCase))
                {
                    player.ChangePlayerColor(100, player.Shirt, rnd.NextInt(555) + 1);
                }

                if (arg[0].Equals("shirt", StringComparison.OrdinalIgnoreCase))
                {
                    player.ChangePlayerColor(100, rnd.NextInt(555) + 1, player.Skin);
                }

                if (arg[0].Equals("outline", StringComparison.OrdinalIgnoreCase))
                {
                    player.ChangePlayerColor(rnd.NextInt(555) + 1, player.Shirt, player.Skin);
                }
            }

            return $"Set player color to {player.FullColor}";
        }

        private string MoveToLevel(string[] strings)
        {
            if (_game.Levels?[int.Parse(strings[0])] == null)
            {
                return "World is null, or doesn't exist!";
            }

            _game.SetMenu(new LevelTransitionMenu(int.Parse(strings[0]), true));
                
            return "Moved to new level";
        }

        private static string PlaySoundCommand(string[] strings)
        {
            if (strings.Length < 1 || strings[0] == "list-sounds")
            {
                return $"All Sounds: {string.Join(", ", SoundEffectManager.AllSounds.Keys)}";
            }

            strings[0] = strings[0].ToLower();

            if (!SoundEffectManager.AllSounds.ContainsKey(strings[0])) return "SoundEffect does not exist";
            SoundEffectManager.AllSounds[strings[0]].Play();
            return $"Playing sound {strings[0]}";
        }

        private string SaveWorldImageCommand(string[] arg)
        {
            var sp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Output.png");

            if (_game.Levels?[_game.CurrentLevel] == null)
            {
                return "World is null, or doesn't exist!";
            }


            byte[] map = _game.Levels[_game.CurrentLevel].Tiles;

            if (arg.Length >= 1)
            {
                if (arg[0] == "sky")
                {
                    map = _game.Levels[4].Tiles;
                    sp = sp.Replace("Output.png", "Sky.Output.png");
                }
            }
            var bmp = new Bitmap(GameConts.Instance.MaxWidth, GameConts.Instance.MaxHeight, PixelFormat.Format32bppRgb);
            int[] pixels = new int[GameConts.Instance.MaxWidth * GameConts.Instance.MaxHeight];
            for (int y = 0; y < GameConts.Instance.MaxWidth; y++)
            {
                for (int x = 0; x < GameConts.Instance.MaxHeight; x++)
                {
                    int i = x + y* GameConts.Instance.MaxHeight;

                    if (map[i] == Tile.Water.Id) pixels[i] = 0x000080;
                    if (map[i] == Tile.Grass.Id) pixels[i] = 0x208020;
                    if (map[i] == Tile.Rock.Id) pixels[i] = 0xa0a0a0;
                    if (map[i] == Tile.Dirt.Id) pixels[i] = 0x604040;
                    if (map[i] == Tile.Sand.Id) pixels[i] = 0xa0a040;
                    if (map[i] == Tile.Tree.Id) pixels[i] = 0x003000;
                    if (map[i] == Tile.Lava.Id) pixels[i] = 0xff2020;
                    if (map[i] == Tile.Cloud.Id) pixels[i] = 0xa0a0a0;
                    if (map[i] == Tile.StairsDown.Id) pixels[i] = 0xffffff;
                    if (map[i] == Tile.StairsUp.Id) pixels[i] = 0xffffff;
                    if (map[i] == Tile.CloudCactus.Id) pixels[i] = 0xff00ff;

                    bmp.SetPixel(x, y, Color.FromArgb(pixels[i]));
                }
            }

            bmp.Save(sp, ImageFormat.Png);

            return $"Saved world image to {sp}";
        }

        private string KillMeCommand(string[] arg)
        {
            if (_game.Player == null)
            {
                return "Player doesn't exist in current game";
            }

            _game.Player.Health = 0;
            return "Who you kill self D:";
        }

        private string SpawnMobCommand(string[] strings)
        {
            if (_game.Levels?[_game.CurrentLevel] == null)
            {
                return "World is null, or doesn't exist!";
            }

            if (strings.Length < 1)
            {
                return "Invalid command args";
            }


            switch (strings[0].ToLower())
            {
                case "zombie":
                    _game.Levels?[_game.CurrentLevel].Add(new Zombie(1)
                    {
                        X = _game.Player.X + 5,
                        Y = _game.Player.Y + 5
                    });
                    return "Summoned a zombie";
                case "slime":
                    _game.Levels?[_game.CurrentLevel].Add(new Slime(1)
                    {
                        X = _game.Player.X + 5,
                        Y = _game.Player.Y + 5
                    });
                    return "Summoned a slime";
                case "creeper":
                    _game.Levels?[_game.CurrentLevel].Add(new Creeper(1)
                    {
                        X = _game.Player.X + 5,
                        Y = _game.Player.Y + 5
                    });
                    return "Summoned a Creeper";
            }
            return "Invalid mod type";
        }

        public string GiveItemCommand(string[] strings)
        {
            if (_game.Player == null)
            {
                return "Player doesn't exist in current game";
            }

            var r = Resource.AllResources;

            if (strings.Length == 1)
            {
                if (strings[0].ToLower() != "all-items") return "Invalid command args";
                var items = r.Select(ii => ii.Name).ToList();

                return $"All current items: {string.Join(" ,", items).ToLower()}";
            }

            if (strings.Length < 2)
            {
                return "Invalid command args";
            }

            var item = r.FirstOrDefault(i => string.Equals(i.Name, strings[0], StringComparison.CurrentCultureIgnoreCase));

            if (item == null)
            {
                return $"Item {strings[0]} doesn't exist!";
            }

            int amount;
            bool count = int.TryParse(strings[1], out amount);

            if (!count)
            {
                return "Invalid number";
            }

            _game.Player.Inventory.Add(new ResourceItem(item, amount));

            return $"Gave {amount} of item {strings[0]}";
        }
    }
}
