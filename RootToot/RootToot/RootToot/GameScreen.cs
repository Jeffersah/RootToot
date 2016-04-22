using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NCodeRiddian;

namespace RootToot
{
    public class GameScreen
    {
        public virtual void Update(Game1 main)
        {
            Cursor.update();
            KeyboardInputManager.update();
        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
