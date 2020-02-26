using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Myplanegame
{
    public class EBullet:BaseBullet
    {
        public EBullet(Point startPos, AngleType angle, IPlane plane):base(startPos,angle,plane)
        {
            BulletImage = Resource.en_bul01;
        }
    }
}
