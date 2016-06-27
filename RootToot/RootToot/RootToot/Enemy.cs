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
    abstract class Enemy : Entity
    {
        public Map myMap;
        public Player player;
        public bool respawns;
        public int respawntime;
        public int etype;

        public int freeze;

        public int PointValue;

        public static Image subimg;

        public static void Load()
        {
            subimg = new Image("bonus");
        }

        public Enemy(Point pos, int speed, int dir, Player target, Map m, int type, int points ) : base(pos, speed, dir)
        {
            player = target;
            myMap = m;
            respawns = true;
            respawntime = 240;
            etype = type;
            PointValue = points;
            freeze = 0;
        }

        public void Freeze(int i)
        {
            freeze = i;
        }

        public void Draw_Froze(SpriteBatch sb)
        {
            Camera.draw(sb, subimg, Color.White, new Rectangle(currentPos.X + 8, currentPos.Y + 8, 16, 16), 0, new Vector2(8, 8));
        }
    }
}
