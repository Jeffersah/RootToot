using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NCodeRiddian;

namespace RootToot
{
    class MainMenu : GameScreen
    {
        AnimatedImage Note;
        AnimatedImage E1;
        AnimatedImage E2;
        AnimatedImage E3;
        AnimatedImage Bonus;
        AnimatedImage E4;
        AnimatedImage E5;
        AnimatedImage E6;
        PlayerImage pimg;

        Rectangle pimgloc;
        bool dir;

        int penter;

        public MainMenu()
        {
            Note = new AnimatedImage("note", Globals.StdFrame, 32, 2);
            E1 = new AnimatedImage("e1", Globals.StdFrame, 16, 4);
            E2 = new AnimatedImage("e2", Globals.StdFrame, 32, 2);
            E3 = new AnimatedImage("e3", Globals.StdFrame, 16, 4);
            Bonus = new AnimatedImage("bonus", Globals.StdFrame, 32, 1);
            E4 = new AnimatedImage("e4", Globals.StdFrame, 8, 4);
            E5 = new AnimatedImage("e5", Globals.StdFrame, 8, 3);
            E6 = new AnimatedImage("e6", Globals.StdFrame, 8, 5);
            pimg = new PlayerImage();
            dir = false;
            pimgloc = new Rectangle(Globals.FullScreen.Width, 169, Globals.stdTile, Globals.stdTile);
            dir = true;
        }
        public override void Update(Game1 main)
        {
            base.Update(main);
            Note.applyStep();
            E1.applyStep();
            E2.applyStep();
            E3.applyStep();
            Bonus.applyStep();
            E4.applyStep();
            E5.applyStep();
            E6.applyStep();
            if(dir)
            {
                pimgloc.X -= Globals.Std_Move;
                if(pimgloc.X <= -16)
                {
                    pimgloc.X = -16;
                    dir = false;
                }
            }
            else
            {
                pimgloc.X += Globals.Std_Move;
                if (pimgloc.X >= Globals.FullScreen.Width)
                {
                    pimgloc.X = Globals.FullScreen.Width;
                    dir = true;
                }
            }
            penter++;
            if (penter > 30)
                penter = 0;

            if(KeyboardInputManager.isKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                main.currentScreen = new PlayScreen();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            Camera.draw(sb, new Image("mmen"), Globals.FullScreen);
            Camera.draw(sb, Note, new Rectangle(42, 49, Globals.stdTile, Globals.stdTile));
            Camera.draw(sb, E1, new Rectangle(42, 72, Globals.stdTile, Globals.stdTile));
            Camera.draw(sb, E2, new Rectangle(42, 96, Globals.stdTile, Globals.stdTile));
            Camera.draw(sb, E3, new Rectangle(42, 120, Globals.stdTile, Globals.stdTile));
            Camera.draw(sb, Bonus, new Rectangle(145, 49, Globals.stdTile, Globals.stdTile));
            Camera.draw(sb, E4, new Rectangle(145, 72, Globals.stdTile, Globals.stdTile));
            Camera.draw(sb, E5, new Rectangle(145, 96, Globals.stdTile, Globals.stdTile));
            Camera.draw(sb, E6, new Rectangle(145, 120, Globals.stdTile, Globals.stdTile));
            pimg.DrawAt(sb, pimgloc, dir ? 2 : 0);
            if(penter > 15)
            {
                Camera.draw(sb, new Image("penter"), new Rectangle(84, 240, 88, 7));
            }
        }
    }
}
