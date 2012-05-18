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
            mini1.origin = new Vector2(100, 200);
            AnimationFrame mini2 = new AnimationFrame(@"Game\mini2", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            mini2.origin = new Vector2(100, 200);
            this.animation.framesLeft.Add(mini1);
            this.animation.framesLeft.Add(mini2);

            this.animationDeath = new Animation();
            
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

                //Vector2 posClick = new Vector2((resolution.X - GameInterface.Width) / 2 + GameInterface.Width - position.X - (input.CurrentMouseState.X - GameInterface.Width), resolution.Y / 2 - position.Y - input.CurrentMouseState.Y);
                float s = speed * (currentTime.ElapsedGameTime.Ticks) * 0.00001f;

                //posClick -= position;
                posClick.Normalize();
                posClick *= s;

                Vector2 oldPosition = position;

                // TODO: Sprawdzenie kolizji

                position += posClick;

                animation.Update(Direction.Left, currentTime);

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
