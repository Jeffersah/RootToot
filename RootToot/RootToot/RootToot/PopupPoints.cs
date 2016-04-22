using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NCodeRiddian;


namespace RootToot
{
    class PopupPoints
    {
        int value;
        Point position;
        int life;

        public bool isAlive
        {
            get
            {
                return life > 0;
            }
        }

        public PopupPoints(int v, Point p, int life)
        {
            value = v;
            position = p;
            this.life = life;
        }

        public void update()
        {
            life--;
        }
        
        public void Draw(SpriteBatch sb)
        {
            TextRenderer.Render_mini(sb, position, value, Color.DarkOrange);
        }
    }
}
