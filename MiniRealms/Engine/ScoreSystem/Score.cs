using System;
using System.Collections.Generic;

namespace MiniRealms.Engine.ScoreSystem
{
    [Serializable]
    public class Score
    {
        public DateTime FinishDateTime;
        public int TimeTookMs;
        public int AcScore;
        public string Difficulty;
        public bool YouWon;

        public Score(DateTime dt, int timems, int acScore, string difficulty, bool youWon)
        {
            FinishDateTime = dt;
            TimeTookMs = timems;
            AcScore = acScore;
            Difficulty = difficulty;
            YouWon = youWon;
        }

    }

    [Serializable]
    public class Scores
    {
        public List<Score> Score;

        public Scores(List<Score> score)
        {
            Score = score;
        }
    }
}
