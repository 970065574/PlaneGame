using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace Myplanegame
{
    public class BaseBullet:IBullet
    {
        protected IPlane Parent { get; set; }
        protected Point Position { get; set; }
        protected Size BulletSize { get; set; }
        protected int OFFSET { get; set; }
        protected AngleType Angle { get; set; }
        protected Image BulletImage { get; set; }
        protected const double PI = Math.PI;
        protected static Bitmap bm = new Bitmap(Resource.bomb4);//爆炸图片
        protected bool IsHit { get; set; }
        protected Timer timer;
        protected DateTime BoomTime { get; set; }
        TimeSpan BoomHold { get; set; }
        protected string ID;

        bool isBoomed;
        public BaseBullet(Point startPos,AngleType angle,IPlane plane)
        {
            Parent = plane;
            Position = startPos;
            Angle = angle;
            switch (Angle)
            {
                case  AngleType.Zero:
                    BulletImage = Resource.bul02;
                    Position = new Point { X = Position.X, Y = Position.Y - 17 };
                    break;
                case  AngleType.Thirty:
                    BulletImage = Resource.bul02_30;
                    Position = new Point { X = Position.X+12, Y = Position.Y - 12 };
                    break;
                case AngleType.Sixty:
                    BulletImage = Resource.bul02_60;
                    Position = new Point { X = Position.X+2, Y = Position.Y - 17 };
                    break;
                case AngleType.OneTwoZero:
                    BulletImage = Resource.bul02_120;
                    Position = new Point { X = Position.X-35, Y = Position.Y - 12 };
                    break;
                case AngleType.OneFiveZero:
                    BulletImage = Resource.bul02_150;
                    Position = new Point { X = Position.X-20, Y = Position.Y - 12 };
                    break;
                default:
                    break;
            }
            timer = new Timer(50);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            BoomHold = TimeSpan.FromSeconds(3);
            ID = Guid.NewGuid().ToString();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Move();
            timer.Start();
        }

        public virtual void Show(Graphics g)
        {
            g.DrawImage(BulletImage, Position);
        }
        public virtual void Move()
        {
            switch (Angle)
            {
                case  AngleType.Zero:
                    Position = new Point(Position.X, Position.Y - OFFSET);
                    break;
                case AngleType.Thirty:
                    Position = new Point(Position.X + (int)(OFFSET * Math.Cos(PI / 6)), Position.Y - (int)(OFFSET / 2));
                    break;
                case AngleType.Sixty:
                    Position = new Point(Position.X + (int)(OFFSET / 2), Position.Y - (int)(OFFSET * Math.Cos(PI / 6)));
                    break;
                case AngleType.Ninety:
                    Position = new Point(Position.X + OFFSET, Position.Y);
                    break;
                case AngleType.OneTwoZero:
                    Position = new Point(Position.X - (int)(OFFSET / 2), Position.Y - (int)(OFFSET * Math.Cos(PI / 6)));
                    break;
                case AngleType.OneFiveZero:
                    Position = new Point(Position.X - (int)(OFFSET * Math.Cos(PI / 6)), Position.Y - (int)(OFFSET / 2));
                    break;
            }
        }
        public virtual bool IsHitTarget(IPlane plane)
        {
            Rectangle rectangle = new Rectangle(Position, BulletSize);
            Rectangle target = new Rectangle(plane.GetPosition(), plane.GetSize());
            return rectangle.IntersectsWith(target);
        }

        public void Boom()
        {
            BulletImage = bm;
            isBoomed = true;
            BoomTime = DateTime.Now;
        }

        public bool IsNeedDestroy()
        {
            return (isBoomed && BoomTime + BoomHold >= DateTime.Now);
        }
        public string GetID()
        {
            return ID;
        }
        public enum AngleType
        {
            Zero=0,
            Thirty=30,
            Sixty=60,
            Ninety=90,
            OneTwoZero=120,
            OneFiveZero=150
        }
    }
}
