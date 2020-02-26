using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Myplanegame
{
    public interface IPlane
    {
        string GetID();
        ConcurrentDictionary<string, IBullet> Bullets { get; set; }
        bool IsEnemy { get; set; }
        void Start();
        void Stop();
        void Pause();
        void Show(Graphics g);
        void Move();
        void Fire();
        int GetHealth();
        Point GetPosition();
        bool IsHitTarget(IPlane plane);
        void HitPlane();
        void Boom();
        Size GetSize();
    }
}
