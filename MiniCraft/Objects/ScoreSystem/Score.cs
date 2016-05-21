using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MiniRealms.Objects.ScoreSystem
{
    [Serializable]
    public class Score
    {
        [XmlAttribute("z")]
        public DateTime FinishDateTime;
        [XmlAttribute("f")]
        public int TimeTookMs;

        public Score(DateTime dt, int timems)
        {
            FinishDateTime = dt;
            TimeTookMs = timems;
        }
    }

    [Serializable]
    [XmlRoot("d")]
    public class Scores
    {
        [XmlElement("Ss")]
        public List<Score> Score;

        public Scores(List<Score> score)
        {
            Score = score;
        }
    }
}
