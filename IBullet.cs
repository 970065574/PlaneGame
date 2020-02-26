using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Myplanegame
{
    public interface IBullet
    {
        string GetID();
        void Show(Graphics g);
        void Move();
        bool IsHitTarget( IPlane plane);
        void Boom();
        bool IsNeedDestroy();
    }
}
