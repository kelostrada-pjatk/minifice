using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Minifice.GameManagement.Movement
{
    public static class Vector2Extender
    {
        public static Point GetMapPosition(this Vector2 v, GameMap gameMap)
        {
            /*
            int j = (int)Math.Floor((2 * (int)v.Y) / GameMap.TileShift.Y);
            int i = (int)Math.Floor((int)v.X / GameMap.TileShift.X - ((j % 2 == 1) ? 1 / 2 : 0));
            if (i < 0) i = 0;
            if (i > gameMap.width) i = gameMap.width - 1;
            if (j < 0) j = 0;
            if (j > gameMap.height - 1) j = gameMap.height - 1;
            */

            // Małe współrzędne
            int x = (int)Math.Floor(v.X * 2 / GameMap.TileShift.X);
            int y = (int)Math.Floor(v.Y * 2 / GameMap.TileShift.Y);

            // Duże współrzędne
            int i = (int)Math.Floor(((double)x) / 2);
            int j = 2 * (int)Math.Floor((double)y / 2);


            // 4 przypadki i w każdym jeszcze po 2
            // TODO: do poprawy, maja byc uwzglednione punkty na calej mapie a nie cos dziwnego co tu zrobilem
            
            if (x % 2 == 0 && y % 2 == 0)
            {
                float a = (GameMap.TileShift.Y) / (-GameMap.TileShift.X);
                float b = GameMap.TileShift.Y / 2 * (y + 1) - a * GameMap.TileShift.X / 2 * x;
                if (v.Y < a * v.X + b)
                {
                    i--;
                    j--;
                }                
            }
            else if (x % 2 == 1 && y % 2 == 0)
            {
                float a = GameMap.TileShift.Y / GameMap.TileShift.X;
                float b = GameMap.TileShift.Y / 2 * y - a * GameMap.TileShift.X / 2 * x; 
                if (v.Y < a * v.X + b)
                    j--;
            }
            else if (x % 2 == 0 && y % 2 == 1)
            {
                float a = GameMap.TileShift.Y / GameMap.TileShift.X;
                float b = GameMap.TileShift.Y / 2 * y - a * GameMap.TileShift.X / 2 * x;
                if (v.Y > a * v.X + b)
                {
                    i--;
                    j++;
                }
            }
            else if (x % 2 == 1 && y % 2 == 1)
            {
                float a = (GameMap.TileShift.Y) / (-GameMap.TileShift.X);
                float b = GameMap.TileShift.Y / 2 * (y + 1) - a * GameMap.TileShift.X / 2 * x; 
                if (v.Y > a * v.X + b)
                    j++;
            }
            
            return new Point(i, j);
        }

        public static bool Similar(this Vector2 v1, Vector2 v2, float comparer)
        {
            return (Math.Abs(v1.X - v2.X) <= comparer && Math.Abs(v1.Y - v2.Y) <= comparer) ;
        }
    }
}
