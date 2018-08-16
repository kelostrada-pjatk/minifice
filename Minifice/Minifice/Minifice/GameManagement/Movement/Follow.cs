using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ASD.Graph;
using Minifice.Enums;

namespace Minifice.GameManagement.Movement
{
    public class Follow : MoveStrategy
    {
        public Unit destination;

        public Follow()
            : base()
        {

        }

        public Follow(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Unit unit, Unit destination)
            : base(gameMap, fighters, enemies, unit)
        {
            this.destination = destination;
        }

        public override Vector2? Move(GameTime gameTime)
        {
            float s = unit.speed * (gameTime.ElapsedGameTime.Ticks) * 0.00001f;


            Vector2 shift = new Vector2(destination.position.X - unit.position.X, destination.position.Y - unit.position.Y);

            if (shift != Vector2.Zero)
                shift.Normalize();
            shift *= s;

            if ((unit.position + shift).Similar(destination.position,s+20f)) return null;

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
                
                if (stop) return null;



            }
            unit.animation.Update(direction, gameTime);
            return unit.position;
        }

        private bool Collision(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            int i = unit.position.GetMapPosition(gameMap).X;
            int j = unit.position.GetMapPosition(gameMap).Y;

            bool intersects = false;

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
                                if (mo.boundaries.Intersects(unit.boundaries + unit.position)) intersects = true;
                            }
                            else
                            {
                                if (mo.boundaries.Intersects(unit.boundaries + unit.position)) intersects = true;
                            }
                        }
                    }
                }
            }
            /*
            foreach (Fighter f in fighters)
            {
                if (!f.Equals(unit))
                    if ((unit.boundaries + unit.position).Intersects(f.boundaries + f.position))
                        intersects = true;
            }
            */
            /*
            foreach (Enemy e in enemies)
            {
                if (!e.Equals(unit))
                    if ((unit.boundaries + unit.position).Intersects(e.boundaries + e.position))
                        intersects = true;
            }
            */

            return intersects;
        }
    }
}
