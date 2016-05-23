using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiniRealms.Engine;

namespace MiniRealms.Objects.ScoreSystem
{
    public static class ScoreBoardManager
    {
        public static Scores Scores { get; private set; }

        static ScoreBoardManager()
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

        public static void AddItem(DateTime dt, int score, int ac, string diff)
        {
            Scores.Score.Add(new Score(dt, score, ac, diff));

            Scores.Score = Scores.Score.OrderByDescending(x => x.TimeTookMs).ThenBy(x => x.AcScore).ToList();

            if(Scores.Score.Count > 3)
                Scores.Score.RemoveAt(Scores.Score.Count - 1);

            Save();
        }

        private static void Save()
        {
           BinaryHelpers.WriteToBinaryFile(GameConts.ScoreLocation, Scores);
        }

        public static void Load()
        {
            if (!File.Exists(GameConts.ScoreLocation)) return;
            Scores = BinaryHelpers.ReadFromBinaryFile<Scores>(GameConts.ScoreLocation);

            Scores.Score = Scores.Score.OrderByDescending(x => x.TimeTookMs)/*.ThenByDescending(x => x.AcScore)*/.ToList();
        }
    }
}
