using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Myplanegame
{
    class MyJet:BasePlane,IControl
    {
        private int score = 100;
        bool isGetGun = false;
        bool isGetBlood = false;
        private Image gameOver= Resource.gameover;
        public MyJet(Point startPos) : base(startPos)
        {
            PlaneImg = Resource.plane;
            AutoMove = true;
            IsEnemy = false;
            HitPlaneHealth = 30;
            OFFSET = 12;
            PlaneSize = new Size(PlaneImg.Width, PlaneImg.Height);
        }

        private List<Keys> keys = new List<Keys>();
        public void KeyDown(Keys key)
        {
            if (!keys.Contains(key))
            {
                if (key == Keys.A || key == Keys.W || key == Keys.D || key == Keys.S|| key == Keys.Space)
                {
                    keys.Add(key);
                }
            }
        }

        public void KeyUp(Keys key)
        {
            if (keys.Contains(key))
                keys.Remove(key);
        }

        public override void Show(Graphics g)
        {
            base.Show(g);
            if (Health <= 0 || score <= 0)
            {
                g.DrawImage(PlaneImg, 0, -300);
                g.DrawImage(gameOver, 10, 260);
            }
            else if (isGetBlood && Health < 90)
            {
                Health = 100;
            }
        }
        public override void Fire()
        {
            IBullet bullet = new MBullet(Position, 0, this);
            Bullets.TryAdd(bullet.GetID(),bullet);
        }
        public override void Move()
        {
            base.Move();
            if (keys.Contains(Keys.A))
            {
                PlaneImg = Resource.planeLeft;
                if (Position.X < 5)
                {
                    Position = new Point { X = 5, Y = Position.Y };
                }
                Position = new Point { X = Position.X - OFFSET, Y = Position.Y };
            }
            if (keys.Contains(Keys.D))
            {
                PlaneImg = Resource.planeRight;
                if (Position.X > 370)
                    Position = new Point { X = 370, Y = Position.Y };
                Position = new Point { X = Position.X + OFFSET, Y = Position.Y };
            }
            if (keys.Contains(Keys.W))
            {
                if (Position.Y < 5)
                    Position = new Point { X = Position.X, Y = 5 };
                Position = new Point { X = Position.X, Y = Position.Y - OFFSET };
            }
            if (keys.Contains(Keys.S))
            {
                if (Position.Y > 530)
                    Position = new Point { X = Position.X, Y = 530 };
                Position = new Point { X = Position.X, Y = Position.Y + OFFSET };
            }
            if (keys.Contains(Keys.Space))
            {
                Fire();
            }
            if (keys.Count <= 0)
            {
                PlaneImg = Resource.plane;
            }
        }
    }
}
