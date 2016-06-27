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

        public Map myMap;

        public Player(Point location, Map cur):base(location, Globals.Std_Move, 0)
        {
            myImage = new PlayerImage();
            isGhost = false;
            GhostCooldown = 0;
            myMap = cur;
        }

        public override void Update(Map m)
        {
            if(direction == 0)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Left))
                    Flip();
            }
            else if (direction == 1)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Down))
                    Flip();
            }
            else if (direction == 2)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Right))
                    Flip();
            }
            else if (direction == 3)
            {
                if (KeyboardInputManager.isKeyDown(Keys.Up))
                    Flip();
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
            Point tgt = base.TargetTile();
            if (isInBounds(tgt) && !isGhost)
            {
                if (myMap.myData.Map[tgt.X, tgt.Y] == 0)
                {
                    for (int i = myMap.allNotes.Count - 1; i >= 0; i--)
                    {
                        if (myMap.allNotes[i].TargetTile().Equals(tgt))
                        {
                            myMap.screen.score += 20;
                            myMap.NoteProj.Add(new NoteProjectile(myMap.allNotes[i].currentPos, direction));
                            myMap.allNotes.RemoveAt(i);
                        }
                    }
                    if(myMap.E_Life != null && myMap.E_Life.TargetTile().Equals(tgt))
                    {
                        myMap.myData.espawn_possible.Add(myMap.E_Life.currentPos);
                        myMap.E_Life = null;
                        myMap.screen.lives++;
                    }
                    if (myMap.bonus != null && myMap.bonus.TargetTile().Equals(tgt))
                    {
                        myMap.myData.espawn_possible.Add(myMap.bonus.currentPos);
                        myMap.bonus = null;
                        myMap.ActivateBonus();
                    }
                }
            }

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

            if(Globals.DEBUG)
            {
                GenericShapeHelper.DrawX(LocationManager.getVectorFromPoint(GetTarget()), 5, sb, Color.Orange);
                GenericShapeHelper.DrawX(new Vector2(base.tile.X * Globals.stdTile, base.tile.Y * Globals.stdTile), 5, sb, Color.Yellow);
            }
        }
    }
}
