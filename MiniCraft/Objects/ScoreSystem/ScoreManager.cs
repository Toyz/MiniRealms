using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace MiniRealms.Objects.ScoreSystem
{
    public static class ScoreManager
    {
        public static Scores Scores { get; private set; }

        static ScoreManager()
        {
            if (Scores == null)
            {
                Scores = new Scores(new List<Score>());
            }

            if (!Directory.Exists(Path.GetDirectoryName(GameConts.ScoreLocation)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(GameConts.ScoreLocation));
            }

            if (File.Exists(GameConts.ScoreLocation))
            {
                Load();
            }
        }

        public static void AddItem(DateTime dt, int score)
        {
            Scores.Score.Add(new Score(dt, score));

            Scores.Score = Scores.Score.OrderByDescending(x => x.TimeTookMs).ToList();

            if(Scores.Score.Count > 5)
                Scores.Score.RemoveAt(Scores.Score.Count - 1);

            Save();
        }

        private static void Save()
        {
            FileStream fs = new FileStream(GameConts.ScoreLocation, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, Scores);
            fs.Close();
        }

        public static void Load()
        {
            if (File.Exists(GameConts.ScoreLocation))
            {
                FileStream fs = new FileStream(GameConts.ScoreLocation, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                fs.Position = 0;
                Scores = (Scores) bf.Deserialize(fs);
                fs.Close();
            }
        }
    }
}
