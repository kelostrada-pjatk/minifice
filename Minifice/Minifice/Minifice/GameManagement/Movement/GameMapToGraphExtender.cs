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
        public static IGraph CreateGraph(this GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Vector2 source, Vector2 destination)
        {
            Vector2 sourceMap = source.GetMapPosition(gameMap);
            Vector2 destinationMap = source.GetMapPosition(gameMap);
            Point shift = new Point((int)Math.Min(sourceMap.X, destinationMap.X), (int)Math.Min(sourceMap.Y, destinationMap.Y));

            IGraph g = new AdjacencyMatrixGraph(false,(int)(Math.Abs(destinationMap.X-sourceMap.X+4)*Math.Abs(destinationMap.Y-sourceMap.Y+4)));

            for (int i = -2; i < destinationMap.X - sourceMap.X + 2; i++)
            {
                for (int j = -2; j < destinationMap.Y - sourceMap.Y + 2; j++)
                {
                    if (shift.X+i >= 0 && shift.X+i < gameMap.width && shift.Y + j - 2 >=0 && shift.Y+j-2 < gameMap.height && gameMap[shift.X + i, shift.Y + j - 2].mapObjects.Count == 0)
                    {
                        g.AddEdge((int)( j * (destinationMap.X - sourceMap.X + 4) + i ),(int)( (j - 2) * (destinationMap.X - sourceMap.X + 4) + i - 1 ));
                    }
                }
            }

            return g;
        }
    }
}
