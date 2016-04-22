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
    class Player : Entity
    {
        public const int GhostTime = 64;
        public const int GhostCool = 32;

        PlayerImage myImage;
        public bool isGhost;
        public int GhostCooldown;

        public Player(Point location):base(location, Globals.Std_Move, 0)
        {
            myImage = new PlayerImage();
            isGhost = false;
            GhostCooldown = 0;
        }

        public override void Update(Map m)
        {
            if(direction == 0)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Left))
                    direction = 2;
            }
            else if (direction == 1)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Down))
                    direction = 3;
            }
            else if (direction == 2)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Right))
                    direction = 0;
            }
            else if (direction == 3)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Up))
                    direction = 1;
            }
            
            if(GhostCooldown == 0 && !isGhost && KeyboardInputManager.isKeyPressed(Keys.Space))
            {
                isGhost = true;
                GhostCooldown = GhostTime;
            }
            else if (isGhost && KeyboardInputManager.isKeyPressed(Keys.Space))
            {
                isGhost = false;
                GhostCooldown = GhostCool;
            }
            if (GhostCooldown == 0 && isGhost)
            {
                isGhost = false;
                GhostCooldown = GhostCool;
            }
            if (GhostCooldown != 0)
                GhostCooldown--;

            if (base.currentPos.X % Globals.stdTile == 0 && base.currentPos.Y % Globals.stdTile == 0)
                TryTurn();

            if(isInBounds(TargetTile()) && m.myData.Map[TargetTile().X, TargetTile().Y] != 0)
                base.Update(m);
        }


        public override void OnReachedTile()
        {
            TryTurn();  
        }

        public void TryTurn()
        {
            if (base.direction == 0 || base.direction == 2)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Up))
                {
                    base.direction = 1;
                }
                if (KeyboardInputManager.isKeyDown(Keys.Down))
                {
                    base.direction = 3;
                }
            }
            else
            {
                if (KeyboardInputManager.isKeyDown(Keys.Left))
                {
                    base.direction = 2;
                }
                if (KeyboardInputManager.isKeyDown(Keys.Right))
                {
                    base.direction = 0;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if(!isGhost)
                myImage.DrawAt(sb, new Rectangle(currentPos.X + Globals.stdTile / 2, currentPos.Y + Globals.stdTile / 2, Globals.stdTile, Globals.stdTile), direction);
            else
                myImage.DrawGhostAt(sb, new Rectangle(currentPos.X + Globals.stdTile / 2, currentPos.Y + Globals.stdTile / 2, Globals.stdTile, Globals.stdTile), direction);
            base.Draw(sb);
        }
    }
}
