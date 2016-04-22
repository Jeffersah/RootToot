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
    class Enemy : Entity
    {
        public Map myMap;
        public Player player;
        public bool respawns;
        public int respawntime;
        public int etype;

        public int PointValue;

        public Enemy(Point pos, int speed, int dir, Player target, Map m, int type, int points ) : base(pos, speed, dir)
        {
            player = target;
            myMap = m;
            respawns = true;
            respawntime = 64;
            etype = type;
            PointValue = points;
        }
    }
}
