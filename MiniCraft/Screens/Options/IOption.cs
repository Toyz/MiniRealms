using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRealms.Screens.Options
{
    public interface IOption
    {
        string Name { get; set; }
        void Tick(InputHandler input);
        void Update();
    }
}
