using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace Myplanegame
{
    public class BasePlane: IPlane
    {
        protected string ID { get; set; }
        public ConcurrentDictionary<string, IBullet> Bullets { get; set; }
        protected Point Position { get; set; }
        protected int OFFSET { get; set; }
        protected Image PlaneImg { get; set; }
        public bool IsEnemy { get; set; }

        protected int Health = 100; //血量
        protected int HitPlaneHealth = 100;
        protected Timer timer;
        protected bool AutoMove = false;
        protected Size PlaneSize;

        public BasePlane(Point startPos)
        {
            ID = Guid.NewGuid().ToString();
            Position = startPos;
            timer = new Timer(50);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            Bullets = new ConcurrentDictionary<string, IBullet>();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            if (AutoMove)
                Move();
            timer.Start();
        }

        public virtual void Show(Graphics g)
        {
            if (Health > 0)
            {
                g.DrawImage(PlaneImg, Position.X, Position.Y);
            }
        }

        public virtual void Move()
        {

        }
        public virtual void Fire()
        {
        }

        public int GetHealth()
        {
            return Health;
        }

        public Point GetPosition()
        {
            return Position;
        }

        public bool IsHitTarget(IPlane target)
        {
            BasePlane tartPlane = (BasePlane)target;
            Rectangle myRect = new Rectangle(Position.X, Position.Y, GetSize().Width, GetSize().Height);
            Rectangle targetRect = new Rectangle(tartPlane.Position.X, tartPlane.Position.Y, target.GetSize().Width, target.GetSize().Height);
            bool result= myRect.IntersectsWith(targetRect);
            return result;
        }
        public virtual Size GetSize()
        {
            return PlaneSize;
        }

        public void Start()
        {
            timer.Enabled = true;
            timer.Start();
        }

        public void Stop()
        {
            timer.Enabled = false;
            timer.Stop();
        }

        public void Pause()
        {
            timer.Stop();
        }

        public void HitPlane()
        {
            Health -= HitPlaneHealth;
        }

        public void Boom()
        {
            throw new NotImplementedException();
        }

        public string GetID()
        {
            return ID;
        }
    }
}
