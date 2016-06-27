using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NCodeRiddian;

namespace RootToot
{
    class ExtraLife : Entity
    {
        public int Lifetime;
        public Image myImage;

        public ExtraLife(Point pos, int life) : base(pos, 0, 0)
        {
            Lifetime = life;
            myImage = new Image("e_life");
        }

        public override void Update(Map m)
        {
            if (Lifetime > 0)
                Lifetime--;
            if (Lifetime == 0)
                m.E_Life = null;
        }

        public override void Draw(SpriteBatch sb)
        {
            Camera.draw(sb, myImage, new Rectangle(currentPos.X, currentPos.Y, 16, 16));
            base.Draw(sb);
        }
    }
}
