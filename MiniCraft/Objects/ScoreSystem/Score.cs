using System;
using System.Collections.Generic;

namespace MiniRealms.Objects.ScoreSystem
{
    [Serializable]
    public class Score
    {
        public DateTime FinishDateTime;
        public int TimeTookMs;
        public int AcScore;
        public string Difficulty;

        public Score(DateTime dt, int timems, int acScore, string difficulty)
        {
            FinishDateTime = dt;
            TimeTookMs = timems;
            AcScore = acScore;
            Difficulty = difficulty;
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
