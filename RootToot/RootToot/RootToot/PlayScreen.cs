﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NCodeRiddian;

namespace RootToot
{
    class PlayScreen : GameScreen
    {
        Map world;
        int currentLevel;

        public int lives;
        public int score;

        Image overlay;
        Image life;

        Game1 mymain;

        public PlayScreen()
        {
            lives = 3;
            currentLevel = 1;
            world = new Map(LevelData.AllData[currentLevel], this);
            Camera.addX(-Globals.stdTile);
            Camera.addY(-Globals.stdTile * 2);
            overlay = new Image("overlay");
            life = new Image("life");
            score = 0;
        }

        public override void Update(Game1 main)
        {
            mymain = main;
            base.Update(main);
            world.Update();
        }

        public void PlayerDied()
        {
            lives--;
            if (lives < 0)
            {
                Camera.addX(Globals.stdTile);
                Camera.addY(Globals.stdTile * 2);
            
                mymain.currentScreen = new MainMenu();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            sb.Draw(overlay.getTexture(), Globals.FullScreen, Color.White);
            TextRenderer.Render(sb, new Point(121, 18), score, Color.White);
            for(int i = 0; i < lives; i++)
            {
                int x = 200 + i * 8;
                int y = 8;
                sb.Draw(life.getTexture(), new Rectangle(x, y, 8, 8), Color.White);
            }
            world.Draw(sb);
        }
    }
}
