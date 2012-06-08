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
        public static float VerticesDensity = 10;

        public static IGraph CreateAdvancedGraph(this GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Vector2 start, Vector2 end, out Vector2 startGraph)
        {
            // Punkt startowy i końcowy na siatce o gęstości wyznaczonej przez VerticesDensity
            Vector2 startPoint = new Vector2((float)Math.Round(start.X / VerticesDensity)*VerticesDensity, (float)Math.Round(start.Y / VerticesDensity)*VerticesDensity);
            Vector2 endPoint = new Vector2((float)Math.Round(end.X / VerticesDensity) * VerticesDensity, (float)Math.Round(end.Y / VerticesDensity) * VerticesDensity);

            // Jak dużo punktów mieści się na jednym Tile'u
            Vector2 tileDensity = new Vector2((float)Math.Ceiling(GameMap.TileShift.X / VerticesDensity), (float)Math.Ceiling(GameMap.TileShift.Y / VerticesDensity));

            // W którym miejscu na ekranie zaczyna się i kończy graf
            startGraph = new Vector2(Math.Min(startPoint.X, endPoint.X) - VerticesDensity * tileDensity.X, Math.Min(startPoint.Y, endPoint.Y) - VerticesDensity * tileDensity.Y);
            Vector2 endGraph = new Vector2(Math.Max(startPoint.X, endPoint.X) - VerticesDensity * tileDensity.X, Math.Max(startPoint.Y, endPoint.Y) - VerticesDensity * tileDensity.Y);

            // Rozmiar grafu
            Vector2 size = new Vector2((endGraph.X - startGraph.X) / VerticesDensity + 1, (endGraph.Y - startGraph.Y) / VerticesDensity + 1);

            IGraph g = new AdjacencyMatrixGraph(false, (int)(size.X*size.Y));


            // Utworzenie granic do sprawdzania czy da się przejść

            Boundaries[] b = new Boundaries[8];

            List<Vector2> points = new List<Vector2>();

            // Lewy górny
            points.Add(new Vector2(-Unit.BoundariesSize, 0));
            points.Add(new Vector2(0,Unit.BoundariesSize));
            points.Add(new Vector2(Unit.BoundariesSize, 0));
            points.Add(new Vector2(Unit.BoundariesSize - VerticesDensity, -VerticesDensity));
            points.Add(new Vector2(-VerticesDensity, -Unit.BoundariesSize - VerticesDensity));
            points.Add(new Vector2(-Unit.BoundariesSize - VerticesDensity, -VerticesDensity));

            b[0] = Boundaries.CreateFromPoints(points);

            // Górny
            points.Clear();
            points.Add(new Vector2(-Unit.BoundariesSize, 0));
            points.Add(new Vector2(0, Unit.BoundariesSize));
            points.Add(new Vector2(Unit.BoundariesSize, 0));
            points.Add(new Vector2(Unit.BoundariesSize, -VerticesDensity));
            points.Add(new Vector2(0, -Unit.BoundariesSize - VerticesDensity));
            points.Add(new Vector2(-Unit.BoundariesSize, -VerticesDensity));

            b[1] = Boundaries.CreateFromPoints(points);

            // Prawy górny
            points.Clear();
            points.Add(new Vector2(-Unit.BoundariesSize, 0));
            points.Add(new Vector2(0, Unit.BoundariesSize));
            points.Add(new Vector2(Unit.BoundariesSize, 0));
            points.Add(new Vector2(VerticesDensity + Unit.BoundariesSize, -VerticesDensity));
            points.Add(new Vector2(VerticesDensity, -Unit.BoundariesSize - VerticesDensity));
            points.Add(new Vector2(VerticesDensity - Unit.BoundariesSize, -VerticesDensity));

            b[2] = Boundaries.CreateFromPoints(points);

            // Prawy
            points.Clear();
            points.Add(new Vector2(0, Unit.BoundariesSize));
            points.Add(new Vector2(-Unit.BoundariesSize, 0));
            points.Add(new Vector2(0, -Unit.BoundariesSize));
            points.Add(new Vector2(VerticesDensity, -Unit.BoundariesSize));
            points.Add(new Vector2(VerticesDensity + Unit.BoundariesSize, 0));
            points.Add(new Vector2(VerticesDensity, Unit.BoundariesSize));
            b[3] = Boundaries.CreateFromPoints(points);

            // Prawy dolny
            points.Clear();
            points.Add(new Vector2(-Unit.BoundariesSize, 0));
            points.Add(new Vector2(0, -Unit.BoundariesSize));
            points.Add(new Vector2(Unit.BoundariesSize, 0));
            points.Add(new Vector2(VerticesDensity + Unit.BoundariesSize, VerticesDensity));
            points.Add(new Vector2(VerticesDensity, VerticesDensity + Unit.BoundariesSize));
            points.Add(new Vector2(VerticesDensity - Unit.BoundariesSize, VerticesDensity));

            b[4] = Boundaries.CreateFromPoints(points);

            // Dolny
            points.Clear();
            points.Add(new Vector2(-Unit.BoundariesSize, 0));
            points.Add(new Vector2(0, -Unit.BoundariesSize));
            points.Add(new Vector2(Unit.BoundariesSize, 0));
            points.Add(new Vector2(Unit.BoundariesSize, VerticesDensity));
            points.Add(new Vector2(0, VerticesDensity + Unit.BoundariesSize));
            points.Add(new Vector2(-Unit.BoundariesSize, VerticesDensity));

            b[5] = Boundaries.CreateFromPoints(points);

            // Lewy dolny
            points.Clear();
            points.Add(new Vector2(-Unit.BoundariesSize, 0));
            points.Add(new Vector2(0, -Unit.BoundariesSize));
            points.Add(new Vector2(Unit.BoundariesSize, 0));
            points.Add(new Vector2(-VerticesDensity + Unit.BoundariesSize, VerticesDensity));
            points.Add(new Vector2(-VerticesDensity, VerticesDensity + Unit.BoundariesSize));
            points.Add(new Vector2(-VerticesDensity - Unit.BoundariesSize, VerticesDensity));

            b[6] = Boundaries.CreateFromPoints(points);

            // Lewy
            points.Clear();
            points.Add(new Vector2(0, Unit.BoundariesSize));
            points.Add(new Vector2(Unit.BoundariesSize, 0));
            points.Add(new Vector2(0, -Unit.BoundariesSize));
            points.Add(new Vector2(-VerticesDensity, -Unit.BoundariesSize));
            points.Add(new Vector2(-VerticesDensity - Unit.BoundariesSize, 0));
            points.Add(new Vector2(-VerticesDensity, Unit.BoundariesSize));

            b[7] = Boundaries.CreateFromPoints(points);


            // Tworzenie grafu, TODO: do poprawki ta pętla
            for (int i = 1; i < size.X - 1; i+=2)
            {
                for (int j = 1; j < size.Y - 1; j+=2)
                {
                    Vector2 mapPosition = new Vector2(startGraph.X + i * VerticesDensity, startGraph.Y + j * VerticesDensity);
                    bool intersects = false;
                    //if ((b[0]+mapPosition).Intersects(
                }
            }


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
            Point p1 = start.GetMapPosition(gameMap);
            Point p2 = start.GetMapPosition(gameMap);
            IGraph g;
            if ((Math.Abs(p1.X - p2.X) + 4) * (Math.Abs(p1.Y - p2.Y) + 4) < 100)
                g = new AdjacencyListsGraph(false, (Math.Abs(p1.X - p2.X) + 4) * (Math.Abs(p1.Y - p2.Y) + 4));
            else
                g = new AdjacencyListsGraph(false, 100); // TODO do poprawy

            return g;
        }
    }
}
