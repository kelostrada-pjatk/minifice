using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Minifice.GameManagement.Movement
{
    public static class Vector2Extender
    {
        public static Vector2 GetMapPosition(this Vector2 v, GameMap gameMap)
        {
            int j = (int)Math.Floor((2 * (int)v.Y) / GameMap.TileShift.Y);
            int i = (int)Math.Floor((int)v.X / GameMap.TileShift.X - ((j % 2 == 1) ? 1 / 2 : 0));
            if (i < 0) i = 0;
            if (i > gameMap.width) i = gameMap.width - 1;
            if (j < 0) j = 0;
            if (j > gameMap.height - 1) j = gameMap.height - 1;
            return new Vector2(i, j);
        }

        public static bool Similar(this Vector2 v1, Vector2 v2, float comparer)
        {
            return (Math.Abs(v1.X - v2.X) <= comparer && Math.Abs(v1.Y - v2.Y) <= comparer) ;
        }
    }
}
