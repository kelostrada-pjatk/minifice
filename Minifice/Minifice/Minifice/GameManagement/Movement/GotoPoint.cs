using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ASD.Graph;
using Minifice.Enums;

namespace Minifice.GameManagement.Movement
{
    public class GotoPoint : MoveStrategy
    {
        public GotoPoint()
            : base()
        {

        }

        public GotoPoint(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Fighter unit)
            : base(gameMap, fighters, enemies, unit)
        {

        }

        public override Vector2? Move(GameTime gameTime)
        {

            float s = unit.speed * (gameTime.ElapsedGameTime.Ticks) * 0.00001f;


            Vector2 shift = new Vector2((unit as Fighter).Destination.X - unit.position.X, (unit as Fighter).Destination.Y - unit.position.Y);

            if (shift != Vector2.Zero)
                shift.Normalize();
            shift *= s;

            if ((unit.position + shift).Similar((unit as Fighter).Destination,s)) return null;

            Vector2 oldPosition = unit.position;
            unit.position += shift;

            
            
            Direction direction = unit.animation.direction;

            if (!Collision(gameMap, fighters, enemies))
            {
                // Kierunek animacji
                Vector2 kier = unit.position - oldPosition;
                if (kier.X > 0)
                {
                    if (kier.X > Math.Abs(kier.Y))
                        direction = Direction.Right;
                    else
                        direction = (kier.Y > 0) ? Direction.Down : Direction.Up;
                }
                else
                {
                    if (Math.Abs(kier.X) > Math.Abs(kier.Y))
                        direction = Direction.Left;
                    else
                        direction = (kier.Y > 0) ? Direction.Down : Direction.Up;
                }
            }
            else
            {
                bool stop = true;
                unit.position -= shift;

                // Helper do poruszania sie wzdłuż boundaries?
                float degrees = 5;
                while (degrees < 90)
                {
                    degrees *= -1;
                    degrees += Math.Sign(degrees) * 5;

                    shift = Vector2.Transform(shift, Matrix.CreateRotationZ(MathHelper.ToRadians(degrees)));

                    unit.position += shift;

                    if (!Collision(gameMap, fighters, enemies))
                    {
                        // Kierunek animacji
                        Vector2 kier = unit.position - oldPosition;
                        if (kier.X > 0)
                        {
                            if (kier.X > Math.Abs(kier.Y))
                                direction = Direction.Right;
                            else
                                direction = (kier.Y > 0) ? Direction.Down : Direction.Up;
                        }
                        else
                        {
                            if (Math.Abs(kier.X) > Math.Abs(kier.Y))
                                direction = Direction.Left;
                            else
                                direction = (kier.Y > 0) ? Direction.Down : Direction.Up;
                        }
                        degrees = 100;
                        stop = false;
                    }
                    else
                    {
                        unit.position -= shift;
                        
                    }

                }

                if (stop) (unit as Fighter).Destination = new Vector2(unit.position.X, unit.position.Y);



            }
            unit.animation.Update(direction, gameTime);
            return unit.position;
            
        }

        private bool Collision(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            int i = (int)unit.GetMapPosition(gameMap).X;
            int j = (int)unit.GetMapPosition(gameMap).Y;
            
            for (int k = -2; k < 3; k++)
            {
                for (int l = -2; l < 3; l++)
                {
                    if (i + k >= 0 && j + l >= 0 && i + k < gameMap.width && j + l < gameMap.height)
                    {
                        foreach (var mo in gameMap.mapTiles[i + k][j + l].mapObjects)
                        {
                            if ((j + l) % 2 == 0)
                            {
                                if ((mo.boundaries + new Vector2((i + k) * GameMap.TileShift.X, (j + l) * GameMap.TileShift.Y / 2)).Intersects(unit.boundaries + unit.position)) return true;
                            }
                            else
                            {
                                if ((mo.boundaries + new Vector2((i + k) * GameMap.TileShift.X + GameMap.TileShift.X / 2, (j + l) * GameMap.TileShift.Y / 2)).Intersects(unit.boundaries + unit.position)) return true;
                            }
                        }
                    }
                }
            }

            if (!(unit as Fighter).PlayerControlled)
                foreach (Fighter f in fighters)
                {
                    if (!f.Equals(unit))
                        if ((unit.boundaries + unit.position).Intersects(f.boundaries + f.position))
                            return true;
                }

            foreach (Enemy e in enemies)
            {
                if (!e.Equals(unit))
                    if ((unit.boundaries + unit.position).Intersects(e.boundaries + e.position))
                        return true;
            }

            return false;
        }
            
    }
}
