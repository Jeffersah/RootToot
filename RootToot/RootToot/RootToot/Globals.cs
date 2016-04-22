using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RootToot
{
    class Globals
    {
        public static Rectangle FullScreen = new Rectangle(0, 0, 256, 256);
        public static Rectangle Tile = new Rectangle(0, 0, 16, 16);
        public static Point StdFrame = new Point(16,16);

        public static int stdTile = StdFrame.X;

        public static int Std_Move = 1;
        public static int Prj_Move = 4;
    }
}
