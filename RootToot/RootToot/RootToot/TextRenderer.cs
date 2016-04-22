using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NCodeRiddian;

namespace RootToot
{
    class TextRenderer
    {
        static Image myImage;
        static Image minidig;

        public static void Render(SpriteBatch sb, Point p, int value, Color c)
        {
            if (myImage == null || myImage.getTexture() == null)
                myImage = new Image("digits");
            Rectangle singleDigit = new Rectangle(0, 0, 7, 7);
            Rectangle current = new Rectangle(p.X, p.Y, 7, 7);
            if(value != 0)
            { 
                while(value > 0)
                {
                    int digit = value % 10;
                    singleDigit.X = 7 * digit;
                    sb.Draw(myImage.getTexture(), current, singleDigit, c);
                    value /= 10;
                    current.X -= 8;
                }
            }
            else
            {
                sb.Draw(myImage.getTexture(), current, singleDigit, c);
            }
        }

        public static void Render_mini(SpriteBatch sb, Point p, int value, Color c)
        {
            if (minidig == null || minidig.getTexture() == null)
                minidig = new Image("5dig");

            Rectangle singleDigit = new Rectangle(0, 0, 5, 5);
            Rectangle current = new Rectangle(p.X, p.Y, 5, 5);

            if (value != 0)
            {
                while (value > 0)
                {
                    int digit = value % 10;
                    singleDigit.X = 5 * digit;
                    Camera.draw(sb, minidig, current, c, singleDigit, 0, Vector2.Zero, SpriteEffects.None, 0);
                    value /= 10;
                    current.X -= 6;
                }
            }
            else
            {
                Camera.draw(sb, minidig, current, c, singleDigit, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
    }
}
