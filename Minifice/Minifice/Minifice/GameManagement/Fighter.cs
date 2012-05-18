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

        #endregion

        #region Właściwości

        [XmlIgnore]
        public bool IsAlive
        {
            get { return health > 0;     }
        }

        #endregion

        #region Inicjalizacja

        public Fighter(bool playerControlled)
        {
            this.playerControlled = playerControlled;

            // Definicja animacji postaci
            this.animation = new Animation(2, 5, Direction.Left);
            AnimationFrame mini1 = new AnimationFrame(@"Game\mini1", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            mini1.origin = new Vector2(100, 160);
            AnimationFrame mini2 = new AnimationFrame(@"Game\mini2", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            mini2.origin = new Vector2(100, 160);
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
            points.Add(new Vector2(-10, -10));
            points.Add(new Vector2(-10, 10));
            points.Add(new Vector2(10, 10));
            points.Add(new Vector2(10, -10));

            this.boundaries = Boundaries.CreateFromPoints(points);
            
            this.health = 2;
            this.isActive = true;
            this.position = new Vector2(100f, 100f);
            this.speed = 1.1f;
            this.timeLastShot = new TimeSpan();
        }

        #endregion

        #region Implementacje Metod

        public Vector2? Move(Vector2 resolution, GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, InputState input)
        {
            if (playerControlled)
            {

                /*
                Przelicz pozycję myszy na współrzędne z mapy; (CalculateCoord)
    Oblicz odległość od punktu docelowego;
    // Oblicz drogę jaką przeszła postać w czasie od ostatniego przesunięcia:
    s = prędkość postaci * (czas teraz – czas ostatniego przesunięcia);
    Normalizuj wektor pozycji gracza względem pozycji myszy;
    Przesuń skopiowana pozycje postaci o s * znormalizowany wektor;
    Dla każdego przeciwnika, wojownika i obiektu mapy
    {
    15
    Sprawdź czy nie zachodzi kolizja; if (koliduje z bonusem)
    Dodaj Bonus;
    }
    if (nie zachodzi kolizja)
    Przesuń postać(wektor);
    Animuj ruch(kierunek);
                */
                Vector2 posClick = new Vector2(input.CurrentMouseState.X - GameInterface.Width - (resolution.X - GameInterface.Width) / 2, input.CurrentMouseState.Y - resolution.Y/2 );

                float s = speed * (currentTime.ElapsedGameTime.Ticks) * 0.00001f;


                if (posClick != Vector2.Zero)
                    posClick.Normalize();
                posClick *= s;

                Vector2 oldPosition = position;
                position += posClick;

                // TODO: Sprawdzenie kolizji
                int j = (int)Math.Floor((2 * (int)position.Y) / GameMap.TileShift.Y);
                int i = (int)Math.Floor((int)position.X / GameMap.TileShift.X - ((j % 2 == 1) ? 1 / 2 : 0));
                if (i < 0) i = 0;
                if (i > gameMap.width - 1) i = gameMap.width - 1;
                if (j < 0) j = 0;
                if (j > gameMap.height - 1) j = gameMap.height - 1;

                bool intersects = false;

                for (int k = -3; k < 4; k++)
                {
                    for (int l = -3; l < 4; l++)
                    {
                        if (i + k >= 0 && j + l >= 0 && i + k < gameMap.width - 1 && j + l < gameMap.height - 1)
                        {
                            foreach (var mo in gameMap.mapTiles[i + k][j + l].mapObjects)
                            {
                                
                                if (mo.boundaries.Intersects(boundaries+position)) intersects = true;
                            }
                        }
                    }
                }

                Direction direction=animation.direction;

                if (!intersects)
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
                }
                animation.Update(direction, currentTime);
                return position;
            }
            return null;
        }

        public override void Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {

        }

        public override void Shoot(InputState input, Weapon weapon, GameTime gameTime, List<Weapon> weapons)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            animation.Draw(spriteBatch, position, position.Y /(GameMap.TileShift.Y / 2) * 0.001f + 0.002f);
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
