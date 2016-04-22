using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NCodeRiddian;

namespace RootToot
{
    class Entity
    {

        public static bool EntitiesIntersect(Entity e1, Entity e2)
        {
            if (e1.tile.Equals(e2.GetTarget()) && e2.tile.Equals(e1.GetTarget()))
                return true;
            if(e1.TargetTile().Equals(e2.TargetTile()))
            {
                if(e1.GetDistance() < 8 && e2.GetDistance() < 8)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool EntitiesIntersect_Rect(Entity e1, Entity e2)
        {
            Rectangle r1 = new Rectangle(e1.currentPos.X, e1.currentPos.Y, 16, 16);
            Rectangle r2 = new Rectangle(e2.currentPos.X, e2.currentPos.Y, 16, 16);
            return r1.Intersects(r2);
        }

        public static bool EntitiesIntersect_Rect(Entity e1, Entity e2, int reqarea)
        {
            Rectangle r1 = new Rectangle(e1.currentPos.X, e1.currentPos.Y, 16, 16);
            Rectangle r2 = new Rectangle(e2.currentPos.X, e2.currentPos.Y, 16, 16);
            if(r1.Intersects(r2))
            {
                Rectangle intersection = Rectangle.Intersect(r1, r2);
                return intersection.Width * intersection.Height >= reqarea;
            }
            return false;
        }

        static int[] xadj = new int[] { 1, 0, -1, 0 };
        static int[] yadj = new int[] { 0, -1, 0, 1 };

        public Point tile;
        public int speed;
        public Point currentPos;
        public int direction;
        public bool isAlive;

        public Entity(Point pos, int speed, int dir)
        {
            currentPos = pos;
            this.speed = speed;
            direction = dir;
            tile = new Point(pos.X / Globals.stdTile, pos.Y / Globals.stdTile);
            isAlive = true;
        }

        public Point TargetTile()
        {
            if (speed == 0)
                return tile;
            return new Point(tile.X + xadj[direction], tile.Y + yadj[direction]);
        }

        public Point GetTarget()
        {
            return new Point(TargetTile().X * Globals.stdTile, TargetTile().Y * Globals.stdTile );
        }

        public int GetDistance()
        {
            return Math.Abs(GetTarget().X - currentPos.X) + Math.Abs(GetTarget().Y - currentPos.Y);
        }

        public Point MoveDir(Point p, int dir)
        {
            return new Point(p.X + xadj[dir], p.Y + yadj[dir]);
        }
        public int MoveDirX(int dir)
        {
            return xadj[dir];
        }
        public int MoveDirY(int dir)
        {
            return yadj[dir];
        }

        public bool isInBounds(Point p)
        {
            return p.X >= 0 && p.X < 14 && p.Y >= 0 && p.Y < 13;
        }
        public virtual void Update(Map m)
        {
            int distance = GetDistance();

            if(distance <= speed)
            {
                currentPos = GetTarget();
                tile = TargetTile();
                OnReachedTile();
            }
            else
            {
                currentPos.X += xadj[direction] * speed;
                currentPos.Y += yadj[direction] * speed;
            }
        }

        public virtual void OnReachedTile()
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
