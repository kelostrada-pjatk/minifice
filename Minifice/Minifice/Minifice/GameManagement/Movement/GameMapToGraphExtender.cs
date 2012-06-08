using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASD.Graph;
using Microsoft.Xna.Framework;

namespace Minifice.GameManagement.Movement
{
    public static class GameMapToGraphExtender
    {
        public static IGraph CreateAdvancedGraph(this GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Vector2 start, Vector2 end)
        {
            Point p1 = start.GetMapPosition(gameMap);
            Point p2 = start.GetMapPosition(gameMap);
            IGraph g;
            if ((Math.Abs(p1.X - p2.X) + 4) * (Math.Abs(p1.Y - p2.Y) + 4) < 100)
                g = new AdjacencyListsGraph(false, (Math.Abs(p1.X - p2.X) + 4) * (Math.Abs(p1.Y - p2.Y) + 4));
            else
                g = new AdjacencyListsGraph(false, 100); // TODO do poprawy















            /*
            Point sourceMap = source.GetMapPosition(gameMap);
            Point destinationMap = source.GetMapPosition(gameMap);
            Point shift = new Point((int)Math.Min(sourceMap.X, destinationMap.X), (int)Math.Min(sourceMap.Y, destinationMap.Y));

            IGraph g = new AdjacencyMatrixGraph(false,(int)(Math.Abs(destinationMap.X-sourceMap.X+4)*Math.Abs(destinationMap.Y-sourceMap.Y+4)));

            for (int i = -2; i < destinationMap.X - sourceMap.X + 2; i++)
            {
                for (int j = -2; j < destinationMap.Y - sourceMap.Y + 2; j++)
                {
                    if (shift.X+i >= 0 && shift.X+i < gameMap.width && shift.Y + j - 2 >=0 && shift.Y+j-2 < gameMap.height && gameMap[shift.X + i, shift.Y + j - 2].mapObjects.Count == 0)
                    {
                        //g.AddEdge((int)( j * (destinationMap.X - sourceMap.X + 4) + i ),(int)( (j - 2) * (destinationMap.X - sourceMap.X + 4) + i - 1 ));
                    }
                }
            }
            */

            return g;
        }

        public static IGraph CreateSimpleGraph(this GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Vector2 start, Vector2 end)
        {
            return new AdjacencyListsGraph(false, 10);
        }
    }
}
