﻿using System;
using MiniRealms.Levels.Tiles;

namespace MiniRealms.Engine.LevelGens
{

    public class LevelGen
    {
        public static Random R;
        private static Random Random
        {
            get
            {
                if (R != null) return R;
                Seed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                R = new Random((int)Seed);

                return R;
            }
        }

        private readonly double[] _values;
        public static long Seed;
        private readonly int _w;
        private readonly int _h;

        private LevelGen(int w, int h, int featureSize)
        {
            _w = w;
            _h = h;

            _values = new double[w * h];

            for (int y = 0; y < w; y += featureSize)
            {
                for (int x = 0; x < w; x += featureSize)
                {
                    SetSample(x, y, Random.NextFloat() * 2 - 1);
                }
            }

            int stepSize = featureSize;
            double scale = 1.0 / w;
            double scaleMod = 1;
            do
            {
                int halfStep = stepSize / 2;
                for (int y = 0; y < w; y += stepSize)
                {
                    for (int x = 0; x < w; x += stepSize)
                    {
                        double a = Sample(x, y);
                        double b = Sample(x + stepSize, y);
                        double c = Sample(x, y + stepSize);
                        double d = Sample(x + stepSize, y + stepSize);

                        double e = (a + b + c + d) / 4.0 + (Random.NextFloat() * 2 - 1) * stepSize * scale;
                        SetSample(x + halfStep, y + halfStep, e);
                    }
                }
                for (int y = 0; y < w; y += stepSize)
                {
                    for (int x = 0; x < w; x += stepSize)
                    {
                        double a = Sample(x, y);
                        double b = Sample(x + stepSize, y);
                        double c = Sample(x, y + stepSize);
                        double d = Sample(x + halfStep, y + halfStep);
                        double e = Sample(x + halfStep, y - halfStep);
                        double f = Sample(x - halfStep, y + halfStep);

                        var H = (a + b + d + e) / 4.0 + (Random.NextFloat() * 2 - 1) * stepSize * scale * 0.5;
                        var g = (a + c + d + f) / 4.0 + (Random.NextFloat() * 2 - 1) * stepSize * scale * 0.5;
                        SetSample(x + halfStep, y, H);
                        SetSample(x, y + halfStep, g);
                    }
                }
                stepSize /= 2;
                scale *= (scaleMod + 0.8);
                scaleMod *= 0.3;
            } while (stepSize > 1);
        }

        private double Sample(int x, int y) => _values[(x & (_w - 1)) + (y & (_h - 1)) * _w];

        private void SetSample(int x, int y, double value)
        {
            if (_values != null) _values[(x & (_w - 1)) + (y & (_h - 1)) * _w] = value;
        }

        public static byte[][] CreateAndValidateTopMap(int w, int h)
        {
            do
            {
                byte[][] result = CreateTopMap(w, h);

                int[] count = new int[256];

                for (int i = 0; i < w * h; i++)
                {
                    count[result[0][i] & 0xff]++;
                }
                if (count[Tile.Rock.Id & 0xff] < 100) continue;
                if (count[Tile.Sand.Id & 0xff] < 100) continue;
                if (count[Tile.Grass.Id & 0xff] < 100) continue;
                if (count[Tile.Tree.Id & 0xff] < 100) continue;
                if (count[Tile.DarkOakTree.Id & 0xff] < 100) continue;
                if (count[Tile.SkyTree.Id & 0xff] < 15) continue;
                if (count[Tile.StairsDown.Id & 0xff] < 2) continue;

                return result;

            } while (true);
        }

        public static byte[][] CreateAndValidateUndergroundMap(int w, int h, int depth)
        {
            do
            {
                byte[][] result = CreateUndergroundMap(w, h, depth);

                int[] count = new int[256];

                for (int i = 0; i < w * h; i++)
                {
                    count[result[0][i] & 0xff]++;
                }
                if (count[Tile.Rock.Id & 0xff] < 100) continue;
                if (count[Tile.Dirt.Id & 0xff] < 100) continue;
                if (count[(Tile.IronOre.Id & 0xff) + depth - 1] < 20) continue;
                if (depth >= 3) return result;
                if (count[Tile.StairsDown.Id & 0xff] < 2) continue;

                return result;

            } while (true);
        }

        public static byte[][] CreateAndValidateSkyMap(int w, int h)
        {
            do
            {
                byte[][] result = CreateSkyMap(w, h);

                int[] count = new int[256];

                for (int i = 0; i < w * h; i++)
                {
                    count[result[0][i] & 0xff]++;
                }
                if (count[Tile.Cloud.Id & 0xff] < 2000) continue;
                if (count[Tile.StairsDown.Id & 0xff] < 2) continue;

                return result;

            } while (true);
        }

        private static byte[][] CreateTopMap(int w, int h)
        {
            LevelGen mnoise1 = new LevelGen(w, h, 16);
            LevelGen mnoise2 = new LevelGen(w, h, 16);
            LevelGen mnoise3 = new LevelGen(w, h, 16);

            LevelGen noise1 = new LevelGen(w, h, 32);
            LevelGen noise2 = new LevelGen(w, h, 32);

            byte[] map = new byte[w * h];
            byte[] data = new byte[w * h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = x + y * w;

                    double val = Math.Abs(noise1._values[i] - noise2._values[i]) * 3 - 2;
                    double mval = Math.Abs(mnoise1._values[i] - mnoise2._values[i]);
                    mval = Math.Abs(mval - mnoise3._values[i]) * 3 - 2;

                    double xd = x / (w - 1.0) * 2 - 1;
                    double yd = y / (h - 1.0) * 2 - 1;
                    if (xd < 0) xd = -xd;
                    if (yd < 0) yd = -yd;
                    double dist = xd >= yd ? xd : yd;
                    dist = dist * dist * dist * dist;
                    dist = dist * dist * dist * dist;
                    val = val + 1 - dist * 20;

                    map[i] = val < -0.5 ? Tile.Water.Id : (val > 0.5 && mval < -1.5 ? Tile.Rock.Id : Tile.Grass.Id);
                }
            }

            for (int i = 0; i < w * h / 2800; i++)
            {
                int xs = Random.NextInt(w);
                int ys = Random.NextInt(h);
                for (int k = 0; k < 10; k++)
                {
                    int x = xs + Random.NextInt(21) - 10;
                    int y = ys + Random.NextInt(21) - 10;
                    for (int j = 0; j < 100; j++)
                    {
                        int xo = x + Random.NextInt(5) - Random.NextInt(5);
                        int yo = y + Random.NextInt(5) - Random.NextInt(5);
                        for (int yy = yo - 1; yy <= yo + 1; yy++)
                        {
                            for (int xx = xo - 1; xx <= xo + 1; xx++)
                            {
                                if (xx < 0 || yy < 0 || xx >= w || yy >= h || map[xx + yy*w] != Tile.Grass.Id) continue;
                                map[xx + yy*w] = Tile.Sand.Id;
                            }
                        }
                    }
                }
            }

            /*
             * for (int i = 0; i < w * h / 2800; i++) { int xs = random.nextInt(w); int ys = random.nextInt(h); for (int k = 0; k < 10; k++) { int x = xs + random.nextInt(21) - 10; int y = ys + random.nextInt(21) - 10; for (int j = 0; j < 100; j++) { int xo = x + random.nextInt(5) - random.nextInt(5); int yo = y + random.nextInt(5) - random.nextInt(5); for (int yy = yo - 1; yy <= yo + 1; yy++) for (int xx = xo - 1; xx <= xo + 1; xx++) if (xx >= 0 && yy >= 0 && xx < w && yy < h) { if (map[xx + yy * w] == Tile.grass.id) { map[xx + yy * w] = Tile.dirt.id; } } } } }
             */

            for (int i = 0; i < w * h / 200; i++)
            {
                int x = Random.NextInt(w);
                int y = Random.NextInt(h);
                for (int j = 0; j < 100; j++)
                {
                    int xx = x + Random.NextInt(15) - Random.NextInt(15);
                    int yy = y + Random.NextInt(15) - Random.NextInt(15);
                    if (xx < 0 || yy < 0 || xx >= w || yy >= h || map[xx + yy*w] != Tile.Grass.Id) continue;
                    var r = Random.Next(10);
                    if (r <= 5)
                    {
                       map[xx + yy * w] = Tile.Tree.Id;
                    }else if (r <= 7)
                    {
                        map[xx + yy * w] = Tile.DarkOakTree.Id;
                    }else if (r == 9)
                    {
                        map[xx + yy * w] = Tile.SkyTree.Id;
                    }
                }
            }

            for (int i = 0; i < w * h / 400; i++)
            {
                int x = Random.NextInt(w);
                int y = Random.NextInt(h);
                int col = Random.NextInt(4);
                for (int j = 0; j < 30; j++)
                {
                    int xx = x + Random.NextInt(5) - Random.NextInt(5);
                    int yy = y + Random.NextInt(5) - Random.NextInt(5);
                    if (xx < 0 || yy < 0 || xx >= w || yy >= h || map[xx + yy*w] != Tile.Grass.Id) continue;

                    var ftype = Random.NextInt(7);
                    switch (ftype)
                    {
                        case 6:
                            map[xx + yy * w] = Tile.BlueFlower.Id;
                            break;
                        //case 3:
                        case 2:
                            map[xx + yy * w] = Tile.YellowFlower.Id;
                            break;
                        //case 5:
                        case 1:
                            map[xx + yy * w] = Tile.RedFlower.Id;
                            break;
                        //case 4:
                        case 0:
                            map[xx + yy * w] = Tile.Flower.Id;
                            break;
                    }

 
                    data[xx + yy*w] = (byte) (col + Random.NextInt(4)*16);
                }
            }

            for (int i = 0; i < w * h / 100; i++)
            {
                int xx = Random.NextInt(w);
                int yy = Random.NextInt(h);
                if (xx < 0 || yy < 0 || xx >= w || yy >= h || map[xx + yy*w] != Tile.Sand.Id) continue;
                map[xx + yy*w] = Tile.Cactus.Id;
            }

            int count = 0;
            for (int i = 0; i < w * h / 100; i++)
            {
                int x = Random.NextInt(w - 2) + 1;
                int y = Random.NextInt(h - 2) + 1;

                var stop = false;
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        if (map[xx + yy*w] == Tile.Rock.Id) continue;
                        stop = true;
                        break;
                    }
                    if (stop) break;
                }

                if (stop) continue;

                map[x + y * w] = Tile.StairsDown.Id;
                count++;
                if (count == 4) break;
            }

            return new[] { map, data };
        }

        private static byte[][] CreateUndergroundMap(int w, int h, int depth)
        {
            LevelGen mnoise1 = new LevelGen(w, h, 16);
            LevelGen mnoise2 = new LevelGen(w, h, 16);
            LevelGen mnoise3 = new LevelGen(w, h, 16);

            LevelGen nnoise1 = new LevelGen(w, h, 16);
            LevelGen nnoise2 = new LevelGen(w, h, 16);
            LevelGen nnoise3 = new LevelGen(w, h, 16);

            //var wnoise1 = new LevelGen(w, h, 16);
            //var wnoise2 = new LevelGen(w, h, 16);
            LevelGen wnoise3 = new LevelGen(w, h, 16);

            LevelGen noise1 = new LevelGen(w, h, 32);
            LevelGen noise2 = new LevelGen(w, h, 32);

            byte[] map = new byte[w * h];
            byte[] data = new byte[w * h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = x + y * w;

                    double val = Math.Abs(noise1._values[i] - noise2._values[i]) * 3 - 2;

                    double mval = Math.Abs(mnoise1._values[i] - mnoise2._values[i]);
                    mval = Math.Abs(mval - mnoise3._values[i]) * 3 - 2;

                    double nval = Math.Abs(nnoise1._values[i] - nnoise2._values[i]);
                    nval = Math.Abs(nval - nnoise3._values[i]) * 3 - 2;

                    //double wval = Math.Abs(wnoise1._values[i] - wnoise2._values[i]);
                    //wval = Math.Abs(wval - wnoise3._values[i]) * 5 - 2;
                    var wval = Math.Abs(nval - wnoise3._values[i]) * 4 - 2;

                    double xd = x / (w - 1.0) * 2 - 1;
                    double yd = y / (h - 1.0) * 2 - 1;
                    if (xd < 0) xd = -xd;
                    if (yd < 0) yd = -yd;
                    double dist = xd >= yd ? xd : yd;
                    dist = dist * dist * dist * dist;
                    dist = dist * dist * dist * dist;
                    val = val + 1 - dist * 20;

                    map[i] = val > -2 && wval < -2.0 + (depth)/2*3
                        ? (depth > 2 ? Tile.Lava.Id : Tile.Water.Id)
                        : (val > -2 && (mval < -1.7 || nval < -1.4) ? Tile.Dirt.Id : Tile.Rock.Id);
                }
            }

            {
                int r = 2;
                for (int i = 0; i < w * h / 400; i++)
                {
                    int x = Random.NextInt(w);
                    int y = Random.NextInt(h);
                    for (int j = 0; j < 30; j++)
                    {
                        int xx = x + Random.NextInt(5) - Random.NextInt(5);
                        int yy = y + Random.NextInt(5) - Random.NextInt(5);
                        if (xx < r || yy < r || xx >= w - r || yy >= h - r || map[xx + yy*w] != Tile.Rock.Id) continue;
                        map[xx + yy*w] = (byte) ((Tile.IronOre.Id & 0xff) + depth - 1);
                    }
                }
            }

            if (depth < 3)
            {
                int count = 0;
                for (int i = 0; i < w * h / 100; i++)
                {
                    int x = Random.NextInt(w - 20) + 10;
                    int y = Random.NextInt(h - 20) + 10;

                    var stop = false;
                    for (int yy = y - 1; yy <= y + 1; yy++)
                    {
                        for (int xx = x - 1; xx <= x + 1; xx++)
                        {
                            if (map[xx + yy*w] == Tile.Rock.Id) continue;
                            stop = true;
                            break; //continue stairsLoop;
                        }
                        if (stop) break;
                    }

                    if (stop) continue;

                    map[x + y * w] = Tile.StairsDown.Id;
                    count++;
                    if (count == 4) break;
                }
            }

            return new[] { map, data };
        }

        private static byte[][] CreateSkyMap(int w, int h)
        {
            LevelGen noise1 = new LevelGen(w, h, 8);
            LevelGen noise2 = new LevelGen(w, h, 8);

            byte[] map = new byte[w * h];
            byte[] data = new byte[w * h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = x + y * w;

                    double val = Math.Abs(noise1._values[i] - noise2._values[i]) * 3 - 2;

                    double xd = x / (w - 1.0) * 2 - 1;
                    double yd = y / (h - 1.0) * 2 - 1;
                    if (xd < 0) xd = -xd;
                    if (yd < 0) yd = -yd;
                    double dist = xd >= yd ? xd : yd;
                    dist = dist * dist * dist * dist;
                    dist = dist * dist * dist * dist;
                    val = -val * 1 - 2.2;
                    val = val + 1 - dist * 20;

                    if (val < -0.25)
                    {
                        map[i] = Tile.InfiniteFall.Id;
                    }
                    else
                    {
                        map[i] = Tile.Cloud.Id;
                    }
                }
            }

            for (int i = 0; i < w * h / 50; i++)
            {
                int x = Random.NextInt(w - 2) + 1;
                int y = Random.NextInt(h - 2) + 1;

                var stop = false;
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        if (map[xx + yy*w] == Tile.Cloud.Id) continue;
                        stop = true; break;
                    }
                    if (stop) break;
                }

                if (stop) continue;

                map[x + y * w] = Tile.CloudCactus.Id;
            }

            int count = 0;
            for (int i = 0; i < w * h; i++)
            {
                int x = Random.NextInt(w - 2) + 1;
                int y = Random.NextInt(h - 2) + 1;

                var stop = false;
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        if (map[xx + yy*w] == Tile.Cloud.Id) continue;
                        stop = true;
                        break;
                    }
                    if (stop) break;
                }
                if (stop) continue;

                map[x + y * w] = Tile.StairsDown.Id;
                count++;
                if (count == 2) break;
            }

            return new[] { map, data };
        }
    }
}
