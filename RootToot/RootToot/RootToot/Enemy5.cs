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
    class EnemyMine : Enemy
    {
        Image spike;
        public EnemyMine(Point p, Map m) : base(p, 0, 0, m.player, m, -1, 20)
        {
            spike = new Image("spike");
        }

        public override void Draw(SpriteBatch sb)
        {
            Camera.draw(sb, spike, new Rectangle(currentPos.X, currentPos.Y, 16, 16));
            base.Draw(sb);
        }
    }
    class Enemy5 : Enemy
    {
        int minespawntime;
        const int MAX_MINE = 128;
        AnimatedImage image;
        bool[] valid = new bool[] { false, false, false, false };
        public Enemy5(Point p, Map m, Player target) : base(p, 1, 0, target, m, 5, 600)
        {
            image = new AnimatedImage("e5", Globals.StdFrame, 16, 3);
        }

        public override void Update(Map m)
        {
            image.applyStep();
            base.Update(m);
        }

        public override void OnReachedTile()
        {
            if (minespawntime != 0)
                minespawntime--;
            if (GlobalRandom.random.NextDouble() < .15f && minespawntime == 0)
            {
                bool occu = false;
                foreach (Note n in myMap.allNotes)
                {
                    if (n.currentPos.Equals(currentPos))
                    {
                        occu = true;
                    }
                }
                if (!occu)
                {
                    myMap.AllEnemies.Add(new EnemyMine(currentPos, myMap));
                    minespawntime = MAX_MINE;
                }
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
            Camera.draw(sb, image,  Color.White, new Rectangle(currentPos.X + 8, currentPos.Y + 8, 16, 16), -MathHelper.PiOver2 * direction, new Vector2(8,8));
            base.Draw(sb);
        }
    }
}
