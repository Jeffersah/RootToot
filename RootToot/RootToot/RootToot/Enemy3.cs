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
    class Enemy3 : Enemy
    {
        const int MIN_SPAWN_TIME = 16 * 16;

        AnimatedImage image;
        bool[] valid = new bool[] { false, false, false, false };
        int notespawntime;

        
        public Enemy3(Point p, Map m, Player target) : base(p, 1, 0, target, m, 3, 400)
        {
            image = new AnimatedImage("e3", Globals.StdFrame, 32, 4);
            respawns = true;
            respawntime *= 2;
            notespawntime = 0;
            while (!isInBounds(TargetTile()) || m.myData.Map[TargetTile().X, TargetTile().Y] == 0)
                direction++;
        }

        public override void Update(Map m)
        {
            image.applyStep();
            base.Update(m);
        }

        public override void OnReachedTile()
        {
            if (notespawntime != 0)
                notespawntime--;
            if(GlobalRandom.random.NextDouble() < .05f && notespawntime == 0)
            {
                bool occu = false;
                foreach(Note n in myMap.allNotes)
                {
                    if(n.currentPos.Equals(currentPos))
                    {
                        occu = true;
                    }
                }
                if(!occu)
                {
                    myMap.allNotes.Add(new Note(currentPos));
                    notespawntime = MIN_SPAWN_TIME;
                    SFXManager.PlayE3Spawn();
                }
            }

            for(int i = 0; i < 4; i++)
            {
                valid[i] = false;
                Point target = MoveDir(tile, i);
                if(isInBounds(target))
                {
                    if (myMap.myData.Map[target.X, target.Y] != 0)
                        valid[i] = true;
                }
                if ((direction == 0 && i == 2) || (direction == 2 && i == 0) || (direction == 1 && i == 3) || (direction == 3 && i == 1))
                    valid[i] = false;
            }

            int xDist = player.currentPos.X - currentPos.X;
            int yDist = player.currentPos.Y - currentPos.Y;
            if(Math.Abs(xDist) > Math.Abs(yDist))
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

            if(!valid[direction])
            {
                for(int i = 0; i < 4; i++)
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
