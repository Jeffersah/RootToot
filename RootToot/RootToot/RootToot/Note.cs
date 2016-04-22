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
    class Note : Entity
    {
        AnimatedImage NoteImage;
        public Note(Point location):base(location, 0, 0)
        {
            NoteImage = new AnimatedImage("note", Globals.StdFrame, 32, 2);
        }
        
        public override void Draw(SpriteBatch sb)
        {
            NoteImage.applyStep();
            Camera.draw(sb, NoteImage, new Rectangle(base.currentPos.X, base.currentPos.Y, Globals.stdTile, Globals.stdTile));
            base.Draw(sb);
        }
    }

    class NoteProjectile : Entity
    {
        AnimatedImage NoteImage;

        public int multiplier;
        public NoteProjectile(Point location, int dir):base(location, Globals.Prj_Move, dir)
        {
            NoteImage = new AnimatedImage("note", Globals.StdFrame, 16, 2);
            multiplier = 1;
        }

        public override void Update(Map m)
        {
            base.Update(m);
        }

        public override void Draw(SpriteBatch sb)
        {
            NoteImage.applyStep();
            Camera.draw(sb, NoteImage, new Rectangle(base.currentPos.X, base.currentPos.Y, Globals.stdTile, Globals.stdTile));
            base.Draw(sb);
        }
    }
}
