using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using NCodeRiddian;

namespace RootToot
{
    class E2Bullet : Enemy
    {
        AnimatedImage proj;
        public E2Bullet(Point p, Map m, Player target, int dir) : base(p, 2, dir, target, m, -1, 1000)
        {
            respawns = false;
            proj = new AnimatedImage("projectile", Globals.StdFrame, 8, 4);
        }
        public override void Update(Map m)
        {
            if (isInBounds(TargetTile()))
            {
                base.Update(m);
            }
            else
                m.AllEnemies.Remove(this);
            proj.applyStep();
            
        }
        public override void Draw(SpriteBatch sb)
        {
            Camera.draw(sb, proj, new Rectangle(currentPos.X, currentPos.Y, 16, 16));
            base.Draw(sb);
        }
    }

    class Enemy2 : Enemy
    {
        const int MIN_SPAWN_TIME = 16 * 16;

        AnimatedImage image;
        bool[] valid = new bool[] { false, false, false, false };
        int notespawntime;

        int shootdir;

        int stoptime;

        
        public Enemy2(Point p, Map m, Player target) : base(p, 1, 0, target, m, 2, 400)
        {
            image = new AnimatedImage("e2", Globals.StdFrame, 32, 4);
            respawns = true;
            respawntime *= 4;
            notespawntime = 0;
            stoptime = 0;
            shootdir = 0;
        }

        public override void Update(Map m)
        {
            image.applyStep();
            if (stoptime == 0)
                base.Update(m);
            else
            { 
                stoptime--;
                if(stoptime == 0)
                {
                    m.AllEnemies.Add(new E2Bullet(currentPos, m, m.player, shootdir));
                }
            }
        }

        public override void OnReachedTile()
        {
            if (notespawntime != 0)
                notespawntime--;
            if(GlobalRandom.random.NextDouble() < .15f && notespawntime == 0)
            {
                stoptime = 16;
                shootdir = GlobalRandom.random.Next(4);
            }
            for (int i = 0; i < 4; i++)
            {
                valid[i] = false;
                Point target = MoveDir(tile, i);
                if (isInBounds(target))
                {
                    if (myMap.myData.Map[target.X, target.Y] != 0)
                        valid[i] = true;
                }
                if ((direction == 0 && i == 2) || (direction == 2 && i == 0) || (direction == 1 && i == 3) || (direction == 3 && i == 1))
                    valid[i] = false;
            }

            int xDist = player.currentPos.X - currentPos.X;
            int yDist = player.currentPos.Y - currentPos.Y;
            if (Math.Abs(xDist) > Math.Abs(yDist))
            {
                if (xDist > 0 && valid[2])
                    direction = 2;
                else if (xDist < 0 && valid[0])
                    direction = 0;
            }
            else
            {
                if (yDist > 0 && valid[1])
                    direction = 1;
                else if (yDist < 0 && valid[3])
                    direction = 3;
            }

            if (!valid[direction])
            {
                for (int i = 0; i < 4; i++)
                {
                    if (valid[i])
                        direction = i;
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            if(stoptime == 0)
                Camera.draw(sb, image,  Color.White, new Rectangle(currentPos.X + 8, currentPos.Y + 8, 16, 16), -MathHelper.PiOver2 * direction, new Vector2(8,8));
            else
                Camera.draw(sb, image, Color.White, new Rectangle(currentPos.X + 8, currentPos.Y + 8, 16, 16), -MathHelper.PiOver2 * shootdir, new Vector2(8, 8));
            base.Draw(sb);
        }
    }
}
