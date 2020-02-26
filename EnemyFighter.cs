using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Myplanegame
{
    public class EnemyFighter : BasePlane
    {
        FighterType _fighterType;
        public EnemyFighter(Point startPos,FighterType fighterType) : base(startPos)
        {
            _fighterType = fighterType;
            switch (fighterType)
            {
                case  FighterType.RED:
                    PlaneImg = Resource.fighterRed;
                    OFFSET += 4;
                    break;
                case  FighterType.GREEN:
                    PlaneImg = Resource.fighterGreen; break;
                case  FighterType.YELLOW:
                    PlaneImg = Resource.fighterYellow;
                    OFFSET += 2;
                    break;
                default:
                    break;
            }
            AutoMove = true;
            IsEnemy = true;
            OFFSET = 4;
            PlaneSize = new Size(PlaneImg.Width, PlaneImg.Height);
        }
        public enum FighterType
        {
            RED,
            GREEN,
            YELLOW
        }
        public override void Move()
        {
            base.Move();
            Position = new Point(Position.X, Position.Y + OFFSET);
        }
    }
}
