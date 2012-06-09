using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minifice.Enums;

namespace Minifice.GameManagement.Movement
{
    public class Patrol : MoveStrategy
    {
        Vector2 point1, point2, direction;
        TimeSpan arrived;
        float waitTime;
        bool waiting = false;

        public Patrol()
            : base()
        {

        }

        public Patrol(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Unit unit, Vector2 p1, Vector2 p2, float wait)
            : base(gameMap, fighters, enemies, unit)
        {
            point1 = p1;
            point2 = p2;
            direction = p1;
            waitTime = wait;
        }

        public override Vector2? Move(GameTime gameTime)
        {
            float s = unit.speed * (gameTime.ElapsedGameTime.Ticks) * 0.00001f;

            Vector2 shift = new Vector2(direction.X - unit.position.X, direction.Y - unit.position.Y);

            if (shift != Vector2.Zero)
                shift.Normalize();
            shift *= s;

            if ((unit.position + shift).Similar(direction, s))
            {
                if (waiting)
                {
                    if (gameTime.TotalGameTime.TotalSeconds - arrived.TotalSeconds > waitTime)
                    {
                        waiting = false;
                        direction = (direction == point1) ? point2 : point1;
                    }
                }
                else
                {
                    arrived = new TimeSpan(gameTime.TotalGameTime.Days, gameTime.TotalGameTime.Hours, gameTime.TotalGameTime.Minutes, gameTime.TotalGameTime.Seconds, gameTime.TotalGameTime.Milliseconds);
                    waiting = true;
                }
                return null;
            } 

            Vector2 oldPosition = unit.position;
            unit.position += shift;

            Direction dir = unit.animation.direction;

            if (!Collision(gameMap, fighters, enemies))
            {
                // Kierunek animacji
                Vector2 kier = unit.position - oldPosition;
                if (kier.X > 0)
                {
                    if (kier.X > Math.Abs(kier.Y))
                        dir = Direction.Right;
                    else
                        dir = (kier.Y > 0) ? Direction.Down : Direction.Up;
                }
                else
                {
                    if (Math.Abs(kier.X) > Math.Abs(kier.Y))
                        dir = Direction.Left;
                    else
                        dir = (kier.Y > 0) ? Direction.Down : Direction.Up;
                }
            }
            else
            {
                unit.position -= shift;
                return null;
            }

            unit.animation.Update(dir, gameTime);
            return unit.position;
        }


        // Patrol sprawdza kolizje tylko z graczem
        private bool Collision(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            int i = unit.position.GetMapPosition(gameMap).X;
            int j = unit.position.GetMapPosition(gameMap).Y;

            bool intersects = false;
            
            foreach (Fighter f in fighters)
            {
                if (!f.Equals(unit))
                    if ((unit.boundaries + unit.position).Intersects(f.boundaries + f.position))
                        intersects = true;
            }
            
            return intersects;
        }
    }
}
