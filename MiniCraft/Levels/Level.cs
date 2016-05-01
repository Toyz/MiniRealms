﻿using System;
using System.Collections.Generic;
using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Levels.LevelGens;
using MiniCraft.Levels.Tiles;

namespace MiniCraft.Levels
{
    public class Level
    {
        private readonly Random _random = new Random();

        public int W, H;

        public byte[] Tiles;
        public byte[] Data;
        public List<Entity>[] EntitiesInTiles;

        public int GrassColor = 141;
        public int DirtColor = 322;
        public int SandColor = 550;
        private readonly int _depth;
        public int MonsterDensity = 8;

        public List<Entity> Entities = new List<Entity>();
        private static readonly SpriteSorter spriteSorter = new SpriteSorter();
        private class SpriteSorter : IComparer<Entity>
        {
            public int Compare(Entity e0, Entity e1)
            {
                if (e1.Y < e0.Y) return +1;
                if (e1.Y > e0.Y) return -1;
                return 0;
            }
        }

        public Level(int w, int h, int level, Level parentLevel)
        {
            unchecked
            {
                if (level < 0)
                {
                    DirtColor = 222;
                }
                _depth = level;
                W = w;
                H = h;
                byte[][] maps;

                if (level == 1)
                {
                    DirtColor = 444;
                }
                if (level == 0)
                    maps = LevelGen.CreateAndValidateTopMap(w, h);
                else if (level < 0)
                {
                    maps = LevelGen.CreateAndValidateUndergroundMap(w, h, -level);
                    MonsterDensity = 4;
                }
                else
                {
                    maps = LevelGen.CreateAndValidateSkyMap(w, h); // Sky level
                    MonsterDensity = 4;
                }

                Tiles = maps[0];
                Data = maps[1];

                if (parentLevel != null)
                {
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                        {
                            if (parentLevel.GetTile(x, y) == Tile.StairsDown)
                            {

                                SetTile(x, y, Tile.StairsUp, 0);
                                if (level == 0)
                                {
                                    SetTile(x - 1, y, Tile.HardRock, 0);
                                    SetTile(x + 1, y, Tile.HardRock, 0);
                                    SetTile(x, y - 1, Tile.HardRock, 0);
                                    SetTile(x, y + 1, Tile.HardRock, 0);
                                    SetTile(x - 1, y - 1, Tile.HardRock, 0);
                                    SetTile(x - 1, y + 1, Tile.HardRock, 0);
                                    SetTile(x + 1, y - 1, Tile.HardRock, 0);
                                    SetTile(x + 1, y + 1, Tile.HardRock, 0);
                                }
                                else
                                {
                                    SetTile(x - 1, y, Tile.Dirt, 0);
                                    SetTile(x + 1, y, Tile.Dirt, 0);
                                    SetTile(x, y - 1, Tile.Dirt, 0);
                                    SetTile(x, y + 1, Tile.Dirt, 0);
                                    SetTile(x - 1, y - 1, Tile.Dirt, 0);
                                    SetTile(x - 1, y + 1, Tile.Dirt, 0);
                                    SetTile(x + 1, y - 1, Tile.Dirt, 0);
                                    SetTile(x + 1, y + 1, Tile.Dirt, 0);
                                }
                            }

                        }
                }

                EntitiesInTiles = new List<Entity>[w * h];
                for (int i = 0; i < w * h; i++)
                {
                    EntitiesInTiles[i] = new List<Entity>();
                }

                if (level == 1)
                {
                    AirWizard aw = new AirWizard
                    {
                        X = w*8,
                        Y = h*8
                    };
                    Add(aw);
                }
            }
        }

        public void RenderBackground(Screen screen, int xScroll, int yScroll)
        {
            int xo = xScroll >> 4;
            int yo = yScroll >> 4;
            int w = (screen.W + 15) >> 4;
            int h = (screen.H + 15) >> 4;
            screen.SetOffset(xScroll, yScroll);
            for (int y = yo; y <= h + yo; y++)
            {
                for (int x = xo; x <= w + xo; x++)
                {
                    GetTile(x, y).Render(screen, this, x, y);
                }
            }
            screen.SetOffset(0, 0);
        }

        private readonly List<Entity> _rowSprites = new List<Entity>();

        public Player Player;

        public void RenderSprites(Screen screen, int xScroll, int yScroll)
        {
            int xo = xScroll >> 4;
            int yo = yScroll >> 4;
            int w = (screen.W + 15) >> 4;
            int h = (screen.H + 15) >> 4;

            screen.SetOffset(xScroll, yScroll);
            for (int y = yo; y <= h + yo; y++)
            {
                for (int x = xo; x <= w + xo; x++)
                {
                    if (x < 0 || y < 0 || x >= W || y >= H) continue;
                    _rowSprites.AddAll(EntitiesInTiles[x + y * W]);
                }
                if (_rowSprites.Size() > 0)
                {
                    SortAndRender(screen, _rowSprites);
                }
                Extensions.Clear(_rowSprites);
            }
            screen.SetOffset(0, 0);
        }

        public void RenderLight(Screen screen, int xScroll, int yScroll)
        {
            int xo = xScroll >> 4;
            int yo = yScroll >> 4;
            int w = (screen.W + 15) >> 4;
            int h = (screen.H + 15) >> 4;

            screen.SetOffset(xScroll, yScroll);
            int r = 4;
            for (int y = yo - r; y <= h + yo + r; y++)
            {
                for (int x = xo - r; x <= w + xo + r; x++)
                {
                    if (x < 0 || y < 0 || x >= W || y >= H) continue;
                    List<Entity> entities = EntitiesInTiles[x + y * W];
                    for (int i = 0; i < entities.Size(); i++)
                    {
                        Entity e = entities.Get(i);
                        // e.render(screen);
                        int lr = e.GetLightRadius();
                        if (lr > 0) screen.RenderLight(e.X - 1, e.Y - 4, lr * 8);
                    }

                    {
                        int lr = GetTile(x, y).GetLightRadius(this, x, y);
                        if (lr > 0) screen.RenderLight(x * 16 + 8, y * 16 + 8, lr * 8);
                    }
                }
            }
            screen.SetOffset(0, 0);
        }

        // private void renderLight(Screen screen, int x, int y, int r) {
        // screen.renderLight(x, y, r);
        // }

        private void SortAndRender(Screen screen, List<Entity> list)
        {
            list.Sort(spriteSorter);
            for (int i = 0; i < list.Size(); i++)
            {
                list.Get(i).Render(screen);
            }
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= W || y >= H) return Tile.Rock;
            return Tile.Tiles[Tiles[x + y * W]];
        }

        public void SetTile(int x, int y, Tile t, int dataVal)
        {
            if (x < 0 || y < 0 || x >= W || y >= H) return;
            Tiles[x + y * W] = t.Id;
            Data[x + y * W] = (byte)dataVal;
        }

        public int GetData(int x, int y)
        {
            if (x < 0 || y < 0 || x >= W || y >= H) return 0;
            return Data[x + y * W] & 0xff;
        }

        public void SetData(int x, int y, int val)
        {
            if (x < 0 || y < 0 || x >= W || y >= H) return;
            Data[x + y * W] = (byte)val;
        }

        public void Add(Entity entity)
        {
            var player = entity as Player;
            if (player != null)
            {
                Player = player;
            }
            entity.Removed = false;
            Extensions.Add(Entities, entity);
            entity.Init(this);

            InsertEntity(entity.X >> 4, entity.Y >> 4, entity);
        }

        public void Remove(Entity e)
        {
            Extensions.Remove(Entities, e);
            int xto = e.X >> 4;
            int yto = e.Y >> 4;
            RemoveEntity(xto, yto, e);
        }

        private void InsertEntity(int x, int y, Entity e)
        {
            if (x < 0 || y < 0 || x >= W || y >= H) return;
            Extensions.Add(EntitiesInTiles[x + y * W], e);
        }

        private void RemoveEntity(int x, int y, Entity e)
        {
            if (x < 0 || y < 0 || x >= W || y >= H) return;
            Extensions.Remove(EntitiesInTiles[x + y * W], e);
        }

        public void TrySpawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Mob mob;

                int minLevel = 1;
                int maxLevel = 1;
                if (_depth < 0)
                {
                    maxLevel = (-_depth) + 1;
                }
                if (_depth > 0)
                {
                    minLevel = maxLevel = 4;
                }

                int lvl = _random.NextInt(maxLevel - minLevel + 1) + minLevel;
                if (_random.NextInt(2) == 0)
                    mob = new Slime(lvl);
                else
                    mob = new Zombie(lvl);

                if (mob.FindStartPos(this))
                {
                    Add(mob);
                }
            }
        }

        public virtual void Tick()
        {
            TrySpawn(1);

            for (int i = 0; i < W * H / 50; i++)
            {
                int xt = _random.NextInt(W);
                int yt = _random.NextInt(W);
                GetTile(xt, yt).Tick(this, xt, yt);
            }
            for (int i = 0; i < Entities.Size(); i++)
            {
                Entity e = Entities.Get(i);
                int xto = e.X >> 4;
                int yto = e.Y >> 4;

                e.Tick();

                if (e.Removed)
                {
                    Entities.Remove(i--);
                    RemoveEntity(xto, yto, e);
                }
                else
                {
                    int xt = e.X >> 4;
                    int yt = e.Y >> 4;

                    if (xto != xt || yto != yt)
                    {
                        RemoveEntity(xto, yto, e);
                        InsertEntity(xt, yt, e);
                    }
                }
            }
        }

        public List<Entity> GetEntities(int x0, int y0, int x1, int y1)
        {
            List<Entity> result = new List<Entity>();
            int xt0 = (x0 >> 4) - 1;
            int yt0 = (y0 >> 4) - 1;
            int xt1 = (x1 >> 4) + 1;
            int yt1 = (y1 >> 4) + 1;
            for (int y = yt0; y <= yt1; y++)
            {
                for (int x = xt0; x <= xt1; x++)
                {
                    if (x < 0 || y < 0 || x >= W || y >= H) continue;
                    List<Entity> entities = EntitiesInTiles[x + y * this.W];
                    for (int i = 0; i < entities.Size(); i++)
                    {
                        Entity e = entities.Get(i);
                        if (e.Intersects(x0, y0, x1, y1)) Extensions.Add(result, e);
                    }
                }
            }
            return result;
        }
    }
}