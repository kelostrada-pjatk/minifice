using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.ScreenManagement;
using Minifice.Enums;
using Minifice.GameManagement.Shooting;
using Minifice.GameManagement.Movement;

namespace Minifice.GameManagement
{
    public class Enemy : Unit
    {
        #region Pola

        bool aware = false;
        Fighter target;

        #endregion

        #region Inicjalizacja

        public Enemy(Vector2 position, Camera2d camera) : base(camera)
        {
            this.position = position;

            // Definicja animacji postaci
            this.animation = new Animation(2, 5, Direction.Left);
            AnimationFrame eiti1 = new AnimationFrame(@"Game\eiti1", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            eiti1.origin = new Vector2(100, 180);
            AnimationFrame eiti2 = new AnimationFrame(@"Game\eiti2", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            eiti2.origin = new Vector2(100, 180);
            this.animation.framesLeft.Add(eiti1);
            this.animation.framesLeft.Add(eiti2);
            this.animation.framesRight.Add(eiti1);
            this.animation.framesRight.Add(eiti2);
            this.animation.framesUp.Add(eiti1);
            this.animation.framesUp.Add(eiti2);
            this.animation.framesDown.Add(eiti1);
            this.animation.framesDown.Add(eiti2);

            this.animationDeath = new Animation(20, 15, Direction.Left);

            AnimationFrame frame;
            for (int i = 1; i <= 10; i++)
            {
                frame = new AnimationFrame(@"Game\AnimationDeath\eiti1_klatka" + i.ToString(), new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
                frame.origin = new Vector2(100, 180);
                if (i == 10)
                    for (int j = 0; j < 11; j++)
                    {
                        this.animationDeath.framesLeft.Add(frame);
                        this.animationDeath.framesRight.Add(frame);
                        this.animationDeath.framesUp.Add(frame);
                        this.animationDeath.framesDown.Add(frame);
                    }
                else
                {
                    this.animationDeath.framesLeft.Add(frame);
                    this.animationDeath.framesRight.Add(frame);
                    this.animationDeath.framesUp.Add(frame);
                    this.animationDeath.framesDown.Add(frame);
                }
            }
            
            List<Vector2> points = new List<Vector2>();

            points.Add(new Vector2(-7, 0));
            points.Add(new Vector2(0, -7));
            points.Add(new Vector2(7, 0));
            points.Add(new Vector2(0, 7));

            this.boundaries = Boundaries.CreateFromPoints(points);
            
            this.health = 6;
            this.speed = 0.75f;
            this.timeLastShot = new TimeSpan();

        }

        #endregion

        #region Metody publiczne

        public override Vector2? Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            if (moveStrategy != null && !isDying)
                return moveStrategy.Move(currentTime);

            return null;
        }

        public void Shoot(ScreenManager screenManager, Weapon weapon, GameTime gameTime, List<Missile> missiles)
        {
            if (gameTime.TotalGameTime.TotalSeconds - timeLastShot.TotalSeconds > Unit.shotFrequency && !isDying && aware)
            {
                timeLastShot = new TimeSpan(gameTime.TotalGameTime.Days, gameTime.TotalGameTime.Hours, gameTime.TotalGameTime.Minutes, gameTime.TotalGameTime.Seconds, gameTime.TotalGameTime.Milliseconds);
                missiles.Add(Missile.FromWeapon(weapon, position, new Vector2(target.position.X, target.position.Y), timeLastShot, Faction.Enemies, gameTime));
                missiles.Last().Load(screenManager.Game.Content);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, position.Y / (GameMap.TileShift.Y / 2) * 0.001f + 0.001f);
        }

        public void Notice(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            
            foreach (var f in fighters)
            {
                if (!f.IsAlive) continue;

                List<Vector2> points = new List<Vector2>();
                points.Add(Vector2.Zero);
                Vector2 v = new Vector2(f.position.X - position.X, f.position.Y - position.Y);
                if (v != Vector2.Zero)
                    v.Normalize();
                points.Add(new Vector2(20*v.X,20*v.Y));
                boundaries = Boundaries.CreateFromPoints(points);

                
                Vector2 current = new Vector2(position.X, position.Y);
                bool intersects = false;
                int i = 0, j = 0;
                while (!current.Similar(f.position, 5) && i>= 0 && j>=0)
                {
                    i = (int)current.GetMapPosition(gameMap).X;
                    j = (int)current.GetMapPosition(gameMap).Y;

                    foreach (var mo in gameMap[i,j].mapObjects)
                        if ((mo.boundaries + new Vector2(i * GameMap.TileShift.X, j * GameMap.TileShift.Y / 2)).Intersects(boundaries + current))
                            intersects = true;

                    if (intersects)
                        break;

                    current += v * 5;
                }

                if (!intersects)
                {
                    aware = true;
                    this.moveStrategy = new PathFind(gameMap, fighters, enemies, this, f);
                    target = f;
                    break;
                }

                
                
            }
            
        }

        #endregion
    }
}
