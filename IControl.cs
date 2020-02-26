using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Myplanegame
{
    public interface IControl
    {
        void KeyUp(Keys key);
        void KeyDown(Keys key);
    }
}
