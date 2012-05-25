#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.ScreenManagement;
using System.Xml.Serialization;
using Minifice.Enums;
#endregion

namespace Minifice.GameManagement
{
    public class Fighter : Unit
    {
        #region Pola

        bool playerControlled;
        bool isActive;
        int health;
        Vector2 destination;

        #endregion

        #region Właściwości

        [XmlIgnore]
        public bool IsAlive
        {
            get { return health > 0;     }
        }

        public Vector2 Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }

        #endregion

        #region Inicjalizacja

        public Fighter(bool playerControlled, Vector2 position)
        {
            this.playerControlled = playerControlled;
            this.position = position;

            // Definicja animacji postaci
            this.animation = new Animation(2, 5, Direction.Left);
            AnimationFrame mini1 = new AnimationFrame(@"Game\mini1", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            mini1.origin = new Vector2(100, 180);
            AnimationFrame mini2 = new AnimationFrame(@"Game\mini2", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            mini2.origin = new Vector2(100, 180);
            this.animation.framesLeft.Add(mini1);
            this.animation.framesLeft.Add(mini2);
            this.animation.framesRight.Add(mini1);
            this.animation.framesRight.Add(mini2);
            this.animation.framesUp.Add(mini1);
            this.animation.framesUp.Add(mini2);
            this.animation.framesDown.Add(mini1);
            this.animation.framesDown.Add(mini2);

            this.animationDeath = new Animation();

            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(-7, 0));
            points.Add(new Vector2(0, -7));
            points.Add(new Vector2(7, 0));
            points.Add(new Vector2(0, 7));

            this.boundaries = Boundaries.CreateFromPoints(points);
            
            this.health = 2;
            this.isActive = true;
            this.speed = 0.7f;
            this.timeLastShot = new TimeSpan();
        }

        #endregion

        #region Implementacje Metod

        public Vector2? Move(Vector2 resolution, GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, InputState input)
        {
            if (playerControlled)
            {
                Vector2 posClick = new Vector2(input.CurrentMouseState.X - GameInterface.Width - (resolution.X - GameInterface.Width) / 2, input.CurrentMouseState.Y - resolution.Y/2 );

                float s = speed * (currentTime.ElapsedGameTime.Ticks) * 0.00001f;


                if (posClick != Vector2.Zero)
                    posClick.Normalize();
                posClick *= s;

                Vector2 oldPosition = position;
                position += posClick;



                Direction direction=animation.direction;

                if (!Collision(gameMap, fighters, enemies))
                {
                    // Kierunek animacji
                    Vector2 kier = position - oldPosition;
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
                    position -= posClick;
                    
                    // Helper do poruszania sie wzdłuż boundaries?
                    float degrees = 5;
                    while (degrees < 90)
                    {
                        degrees *= -1;
                        degrees += Math.Sign(degrees) * 5;

                        posClick = Vector2.Transform(posClick, Matrix.CreateRotationZ(MathHelper.ToRadians(degrees)));

                        position += posClick;

                        if (!Collision(gameMap, fighters, enemies))
                        {
                            // Kierunek animacji
                            Vector2 kier = position - oldPosition;
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
                        }
                        else
                        {
                            position -= posClick;
                        }
                     
                    }
                    


                }
                animation.Update(direction, currentTime);
                return position;
            }
            return null;
        }

        private bool Collision(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            int i = (int)GetMapPosition(gameMap).X;
            int j = (int)GetMapPosition(gameMap).Y;

            bool intersects = false;

            for (int k = -3; k < 4; k++)
            {
                for (int l = -3; l < 4; l++)
                {
                    if (i + k >= 0 && j + l >= 0 && i + k < gameMap.width && j + l < gameMap.height)
                    {
                        foreach (var mo in gameMap.mapTiles[i + k][j + l].mapObjects)
                        {
                            if ((j + l) % 2 == 0)
                            {
                                if ((mo.boundaries + new Vector2((i + k) * GameMap.TileShift.X, (j + l) * GameMap.TileShift.Y / 2)).Intersects(boundaries + position)) intersects = true;
                            }
                            else
                            {
                                if ((mo.boundaries + new Vector2((i + k) * GameMap.TileShift.X + GameMap.TileShift.X / 2, (j + l) * GameMap.TileShift.Y / 2)).Intersects(boundaries + position)) intersects = true;
                            }
                        }
                    }
                }
            }

            foreach (Fighter f in fighters)
            {
                if (!f.Equals(this))
                    if ((boundaries + position).Intersects(f.boundaries + f.position))
                        intersects = true;
            }

            foreach (Enemy e in enemies)
            {
                if (!e.Equals(this))
                    if ((boundaries + position).Intersects(e.boundaries + e.position))
                        intersects = true;
            }

            return intersects;
        }

        public override void Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            if (moveStrategy != null)
                moveStrategy.Move(currentTime);
        }

        public override void Shoot(InputState input, Weapon weapon, GameTime gameTime, List<Weapon> weapons)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            animation.Draw(spriteBatch, position, position.Y /(GameMap.TileShift.Y / 2) * 0.001f + 0.001f);
        }

        #endregion

        #region Metody Publiczne

        public void Activate()
        {

        }

        public void Deactivate()
        {

        }

        #endregion

    }
}
