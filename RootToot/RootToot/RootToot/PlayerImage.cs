using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCodeRiddian;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace RootToot
{
    class PlayerImage
    {
        AnimatedImage baseImage;
        AnimatedImage ghostImage;
        public PlayerImage()
        {
            baseImage = new AnimatedImage("RT_Player", Globals.StdFrame, 15, 2);
            ghostImage = new AnimatedImage("RT_PlayerGhost", Globals.StdFrame, 15, 2);
        }

        public void DrawAt(SpriteBatch sb, Rectangle r, int direction)
        {
            baseImage.applyStep();
            Camera.draw(sb, baseImage, r, Color.White, null, (direction == 1 || direction == 3) ? MathHelper.PiOver2 * (direction == 1 ? 3 : 1): 0, new Vector2(8,8), direction == 2 ? SpriteEffects.FlipHorizontally : /*direction == 1 ? SpriteEffects.FlipVertically :*/ SpriteEffects.None, 0);
        }
        public void DrawGhostAt(SpriteBatch sb, Rectangle r, int direction)
        {
            ghostImage.applyStep();
            Camera.draw(sb, ghostImage, r, Color.White, null, (direction == 1 || direction == 3) ? MathHelper.PiOver2 * (direction == 1 ? 3 : 1) : 0, new Vector2(8, 8), direction == 2 ? SpriteEffects.FlipHorizontally : /*direction == 1 ? SpriteEffects.FlipVertically :*/ SpriteEffects.None, 0);
        }
    }
}
