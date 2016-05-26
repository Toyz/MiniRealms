using System.Collections.Generic;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.OptionItems
{
    public class DifficultyOption : Option
    {
        public class Difficulty
        {

            public Difficulty(string v1, int v2, int v3, int densenty, int bossMobSpawnRate)
            {
                BossMobSpawnRate = bossMobSpawnRate;
                Name = v1;
                BaseLevel = v2;
                SpawnAmount = v3;
                Density = densenty;
            }

            public string Name { get; }
            public int BaseLevel { get; }
            public int SpawnAmount { get; }
            public int BossMobSpawnRate { get; }
            public int Density { get; }
        }

        private readonly List<Difficulty> _modes = new List<Difficulty>
        {
            new Difficulty("Easy", 0, 5000, 8, 50),
            new Difficulty("Medium", 1, 10000, 7, 30),
            new Difficulty("Hard", 2, 15000, 6, 10),
            new Difficulty("Nightmare", 3, 20000, 5, 5)

        };

        private int _selected;

        public override bool Enabled { get; set; } = true;
        public override string Text { get; set; } = "Difficulty: ";
        public override string SelectedText => $"< {Text} >";

        public DifficultyOption()
        {
            Difficulty s = _modes[_selected];

            Text = $"Difficulty: {s.Name}";
        }

        protected internal override void HandleInput(InputHandler input)
        {
            if (input.Left.Clicked)
            {
                _selected--;
            }

            if (input.Right.Clicked)
            {
                _selected++;
            }

            int len = _modes.Count;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;

            Difficulty s = _modes[_selected];

            Text = $"Difficulty: {s.Name}";
        }

        public Difficulty GetDifficulty()
        {
            return _modes[_selected];
        }

        protected internal override void HandleRender()
        {
        }
    }
}
