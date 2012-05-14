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
            this.animation = new Animation();
            this.animationDeath = new Animation();
            this.health = 2;
            this.isActive = true;
            this.position = new Vector2();
            this.speed = 1f;
            this.timeLastShot = new TimeSpan();
        }

        #endregion

        #region Implementacje Metod

        public override void Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, InputState input, GameTime gameTime)
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
            }
            else
            {
                this.moveStrategy.Move(gameTime);
            }


        }

        public override void Shoot(InputState input, Weapon weapon, GameTime gameTime, List<Weapon> weapons)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

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
