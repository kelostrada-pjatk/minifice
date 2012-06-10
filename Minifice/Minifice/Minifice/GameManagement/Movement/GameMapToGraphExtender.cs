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

        public static IGraph CreateAdvancedGraph(this GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Vector2 start, Vector2 end, out Vector2 startGraph, out int beginning, out int destination)
        {
            // Punkt startowy i końcowy na siatce o gęstości wyznaczonej przez VerticesDensity
            Vector2 startPoint = new Vector2((float)Math.Round(start.X / VerticesDensity)*VerticesDensity, (float)Math.Round(start.Y / VerticesDensity)*VerticesDensity);
            Vector2 endPoint = new Vector2((float)Math.Round(end.X / VerticesDensity) * VerticesDensity, (float)Math.Round(end.Y / VerticesDensity) * VerticesDensity);

            // Jak dużo punktów mieści się na jednym Tile'u
            Vector2 tileDensity = new Vector2((float)Math.Ceiling(GameMap.TileShift.X / VerticesDensity), (float)Math.Ceiling(GameMap.TileShift.Y / VerticesDensity));

            // W którym miejscu na ekranie zaczyna się i kończy graf
            startGraph = new Vector2(Math.Min(startPoint.X, endPoint.X) - VerticesDensity * tileDensity.X, Math.Min(startPoint.Y, endPoint.Y) - VerticesDensity * tileDensity.Y);
            Vector2 endGraph = new Vector2(Math.Max(startPoint.X, endPoint.X) + VerticesDensity * tileDensity.X, Math.Max(startPoint.Y, endPoint.Y) + VerticesDensity * tileDensity.Y);

            // Rozmiar grafu
            Point size = new Point((int)((endGraph.X - startGraph.X) / VerticesDensity + 1), (int)((endGraph.Y - startGraph.Y) / VerticesDensity + 1));

            // ustalenie punktow poczatkowego i docelowego, na potrzeby wyszukiwania sciezek
            beginning = (int)((startPoint.X - startGraph.X) / VerticesDensity) + (int)((startPoint.Y - startGraph.Y) / VerticesDensity) * (size.X - 1);
            destination = (int)((endGraph.X - endPoint.X) / VerticesDensity) + (int)((endGraph.Y - endPoint.Y) / VerticesDensity) * (size.X - 1);

            // Tworzenie grafu o rozmiarze prostokata
            IGraph g = new AdjacencyMatrixGraph(false, (int)(size.X*size.Y));

            #region Boundaries
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
            #endregion
            
            // Tworzenie grafu, TODO: do poprawki ta pętla
            for (int i = 1; i < size.X - 1; i+=2)
            {
                for (int j = 1; j < size.Y - 1; j+=2)
                {
                    g.UpdateGraph(i, j, i - 1, j - 1, startGraph, size, VerticesDensity, gameMap, b[0]);
                    g.UpdateGraph(i, j, i, j - 1, startGraph, size, VerticesDensity, gameMap, b[1]);
                    g.UpdateGraph(i, j, i + 1, j - 1, startGraph, size, VerticesDensity, gameMap, b[2]);
                    g.UpdateGraph(i, j, i + 1, j, startGraph, size, VerticesDensity, gameMap, b[3]);
                    g.UpdateGraph(i, j, i + 1, j + 1, startGraph, size, VerticesDensity, gameMap, b[4]);
                    g.UpdateGraph(i, j, i, j + 1, startGraph, size, VerticesDensity, gameMap, b[5]);
                    g.UpdateGraph(i, j, i - 1, j + 1, startGraph, size, VerticesDensity, gameMap, b[6]);
                    g.UpdateGraph(i, j, i - 1, j, startGraph, size, VerticesDensity, gameMap, b[7]);

                    #region komentarz
                    /*
                    Vector2 mapPosition = new Vector2(startGraph.X + i * VerticesDensity, startGraph.Y + j * VerticesDensity);
                    bool intersects = false;
                    int x = startGraph.GetMapPosition(gameMap).X;
                    int y = startGraph.GetMapPosition(gameMap).Y;

                    foreach (var mo in gameMap[x + i - 1, y + j - 1].mapObjects)
                        if ((b[0] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i , y + j].mapObjects)
                        if ((b[0] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i - 1 + (j - 1) * (size.X - 1), i + j * (size.X - 1));
                    else
                        g.DelEdge(i - 1 + (j - 1) * (size.X - 1), i + j * (size.X - 1));

                    intersects = false;
                    foreach (var mo in gameMap[x + i, y + j - 1].mapObjects)
                        if ((b[1] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i, y + j].mapObjects)
                        if ((b[1] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i + (j - 1) * (size.X - 1), i + j * (size.X - 1));

                    intersects = false;
                    foreach (var mo in gameMap[x + i + 1, y + j - 1].mapObjects)
                        if ((b[2] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i, y + j].mapObjects)
                        if ((b[2] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i + 1 + (j - 1) * (size.X - 1), i + j * (size.X - 1));

                    intersects = false;
                    foreach (var mo in gameMap[x + i + 1, y + j].mapObjects)
                        if ((b[3] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i, y + j].mapObjects)
                        if ((b[3] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i + 1 + j * (size.X - 1), i + j * (size.X - 1));

                    intersects = false;
                    foreach (var mo in gameMap[x + i + 1, y + j + 1].mapObjects)
                        if ((b[4] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i, y + j].mapObjects)
                        if ((b[4] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i + 1 + (j + 1) * (size.X - 1), i + j * (size.X - 1));

                    intersects = false;
                    foreach (var mo in gameMap[x + i, y + j + 1].mapObjects)
                        if ((b[5] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i, y + j].mapObjects)
                        if ((b[5] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i + (j + 1) * (size.X - 1), i + j * (size.X - 1));

                    intersects = false;
                    foreach (var mo in gameMap[x + i - 1, y + j + 1].mapObjects)
                        if ((b[6] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i, y + j].mapObjects)
                        if ((b[6] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i - 1 + (j + 1) * (size.X - 1), i + j * (size.X - 1));

                    intersects = false;
                    foreach (var mo in gameMap[x + i - 1, y + j].mapObjects)
                        if ((b[7] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    foreach (var mo in gameMap[x + i, y + j].mapObjects)
                        if ((b[7] + mapPosition).Intersects(mo.boundaries)) intersects = true;
                    if (!intersects)
                        g.AddEdge(i - 1 + j * (size.X - 1), i + j * (size.X - 1));
 */
                    #endregion
                }
            }

            return g;
        }

        /// <summary>
        /// Aktualizacja grafu - dodawanie krawedzi
        /// Metoda rozszerzająca
        /// </summary>
        /// <param name="g">graf</param>
        /// <param name="x1">Punkt x1 (startowy)</param>
        /// <param name="y1">Punkt y1 (startowy)</param>
        /// <param name="x2">Punkt x2</param>
        /// <param name="y2">Punkt y2</param>
        /// <param name="startGraph">Gdzie na mapie zaczyna się graf</param>
        /// <param name="size">Rozmiar grafu (w ilości punktów oddalonych od siebie o VerticesDensity)</param>
        /// <param name="VerticesDensity">Oddalenie punktów od siebie nawzajem</param>
        /// <param name="gameMap">mapa</param>
        /// <param name="b">ograniczenie dla tych dwóch konkretnych punktów</param>
        private static void UpdateGraph(this IGraph g, int x1, int y1, int x2, int y2, Vector2 startGraph, Point size, float VerticesDensity, GameMap gameMap, Boundaries b)
        {
            Vector2 mapPosition1 = new Vector2(startGraph.X + x1 * VerticesDensity, startGraph.Y + y1 * VerticesDensity);
            Vector2 mapPosition2 = new Vector2(startGraph.X + x2 * VerticesDensity, startGraph.Y + y2 * VerticesDensity);
            Point mapPoint1 = mapPosition1.GetMapPosition(gameMap);
            Point mapPoint2 = mapPosition2.GetMapPosition(gameMap);
            Boundaries shift = b.Clone();
            shift += mapPosition1;
            bool intersects = false;

            foreach (MapObject mo in gameMap[mapPoint1.X, mapPoint1.Y].mapObjects)
                if (shift.Intersects(mo.boundaries)) intersects = true;
            foreach (MapObject mo in gameMap[mapPoint2.X, mapPoint2.Y].mapObjects)
                if (shift.Intersects(mo.boundaries)) intersects = true;
            if (!intersects)
                g.AddEdge(x1 + y1 * (size.X - 1), x2 + y2 * (size.X - 1));
            else
                g.DelEdge(x1 + y1 * (size.X - 1), x2 + y2 * (size.X - 1));
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
    }
}
