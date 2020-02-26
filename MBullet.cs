using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Myplanegame
{
    public class MBullet:BaseBullet
    {
        public MBullet(Point startPos, AngleType angle, IPlane plane):base(startPos,angle,plane)
        {
            OFFSET = 6;
            BulletImage = Resource.bul02;
            BulletSize = new Size(BulletImage.Width, BulletImage.Height);
        }
    }
}
