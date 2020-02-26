using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace Myplanegame
{
    public class PlaneManager
    {
        ConcurrentDictionary<string, IPlane> planes;
        private IPlane MyPlane;
        private int maxWidth=350;
        private bool isGameOver=false;
        Timer timer;
        Timer createBotTimer;
        static object locker;
        public PlaneManager()
        {
            locker = new object();
            planes = new ConcurrentDictionary<string, IPlane>();
            timer = new Timer(30);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            createBotTimer = new Timer(1000);
            createBotTimer.AutoReset = true;
            createBotTimer.Elapsed += CreateBotTimer_Elapsed;
        }

        private void CreateBotTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            createBotTimer.Stop();
            CreateBotPlane(true);
            createBotTimer.Start();
        }

        public void Init()
        {
            RandomGenerateBotPlane(3);
            CreateMyJet();
        }
        public void Start()
        {
            timer.Start();
            createBotTimer.Start();
            foreach(var p in planes)
            {
                p.Value.Start();
            }
            MyPlane.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            CheckPlanesStatus();
            timer.Start();
        }

        public void RandomGenerateBotPlane(int number, bool startImmediately = false)
        {
            lock (locker)
            {
                for (int i = 0; i < number;)
                {
                    Random random = new Random((int)DateTime.Now.Ticks);
                    if (random.Next(18) == 0)
                    {
                        Point pos = new Point(random.Next(0, maxWidth), 0);
                        Random random1 = new Random();
                        if (random1.Next(18) == 0)
                        {
                            EnemyFighter.FighterType fighterType = (EnemyFighter.FighterType)random.Next(0, 3);
                            IPlane plane = new EnemyFighter(pos, fighterType);
                            if (startImmediately)
                            {
                                plane.Start();
                            }
                            if (planes.TryAdd(plane.GetID(), plane))
                                i++;
                        }
                    }
                }
            }
        }

        public void CreateBotPlane(bool startImmediately)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            Point pos = new Point(random.Next(0, maxWidth), 0);
            EnemyFighter.FighterType fighterType = (EnemyFighter.FighterType)random.Next(0, 3);
            IPlane plane = new EnemyFighter(pos, fighterType);
            if (startImmediately)
            {
                plane.Start();
            }
            lock (locker)
            {
                planes.TryAdd(plane.GetID(), plane);
            }
        }
        public void CreateMyJet()
        {
            if (MyPlane ==null)
            {
                MyPlane = new MyJet(new Point(180, 530));

            }
        }
        public void KeyDown(System.Windows.Forms.Keys key) 
        {
            if (MyPlane != null)
            {
                IControl control = (IControl)MyPlane;
                control.KeyDown(key);
            }
        }
        public void KeyUp(System.Windows.Forms.Keys key)
        {
            if (MyPlane != null)
            {
                IControl control = (IControl)MyPlane;
                control.KeyUp(key);
            }
        }
        public void CheckPlanesStatus()
        {
            if(MyPlane!=null)
            {
                foreach(IPlane plane in planes.Values)
                {
                    foreach (IBullet bullet in plane.Bullets.Values)
                    {
                        if (bullet.IsHitTarget(MyPlane))
                        {
                            bullet.Boom();
                        }

                    }
                    if (plane.IsEnemy)
                    {
                        if(MyPlane.IsHitTarget(plane))
                        {
                            MyPlane.HitPlane();
                            plane.HitPlane();
                            if (MyPlane.GetHealth() <= 0)
                            {
                                isGameOver = true;
                            }
                        }
                    }
                    foreach(IBullet bullet in MyPlane.Bullets.Values)
                    {
                        if(bullet.IsHitTarget(plane))
                        {
                            bullet.Boom();
                        }
                    }
                }
            }
        }

        public void ShowPlanes(Graphics g)
        {
            if (MyPlane != null && MyPlane.GetHealth() > 0)
            {
                MyPlane.Show(g);
                List<IBullet> tmpBullets = new List<IBullet>();
                foreach (IBullet bullet in MyPlane.Bullets.Values)
                {
                    if (bullet.IsNeedDestroy())
                    {
                        tmpBullets.Add(bullet);
                    }
                    else
                    {
                        bullet.Show(g);
                    }
                }
                tmpBullets.ForEach(b =>
                {
                    IBullet bullet;
                    MyPlane.Bullets.TryRemove(b.GetID(),out bullet);
                });
            }
            var planesTobeDisposed = planes.Values.Where(p => p.GetHealth() <= 0 || p.GetPosition().Y > 530+p.GetSize().Height).ToArray();
            lock (locker)
            {
                foreach (var plane in planesTobeDisposed)
                {
                    IPlane p;
                    planes.TryRemove(plane.GetID(),out p);
                }
                foreach(var p in planes.Values)
                {
                    if (p.GetHealth() > 0)
                        p.Show(g);
                    List<IBullet> tmpBullets = new List<IBullet>();
                    foreach (IBullet bullet in p.Bullets.Values)
                    {
                        if (bullet.IsNeedDestroy())
                        {
                            tmpBullets.Add(bullet);
                        }
                        else
                        {
                            bullet.Show(g);
                        }
                    }
                    tmpBullets.ForEach(b =>
                    {
                        IBullet bullet;
                        MyPlane.Bullets.TryRemove(b.GetID(),out bullet);
                    });
                }
            }

        }
    }
}
