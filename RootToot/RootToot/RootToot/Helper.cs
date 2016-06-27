using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RootToot
{
    class Helper
    {
        public static int adjx(int x, int dir)
        {
            if (dir == 0)
                return x + 1;
            if (dir == 2)
                return x - 1;
            return x;
        }
        public static int adjy(int y, int dir)
        {
            if (dir == 1)
                return y - 1;
            if (dir == 3)
                return y + 1;
            return y;
        }
    }
}
