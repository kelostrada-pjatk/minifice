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
using Minifice.GameManagement.Shooting;
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

        [XmlIgnore]
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

        [XmlAttribute]
        public bool PlayerControlled
        {
            get { return playerControlled; }
            set { playerControlled = value; }
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
            this.speed = 0.55f;
            this.timeLastShot = new TimeSpan();

            this.destination = new Vector2(position.X, position.Y);
        }

        #endregion

        #region Implementacje Metod

        public void Move(Vector2 resolution, GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, InputState input)
        {
            if (playerControlled)
                Destination = new Vector2(input.CurrentMouseState.X - GameInterface.Width - (resolution.X - GameInterface.Width) / 2 + position.X, input.CurrentMouseState.Y - resolution.Y / 2 + position.Y);
        }

        
        public override Vector2? Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            if (moveStrategy != null)
                return moveStrategy.Move(currentTime);

            return null;
        }

        public void Shoot(InputState input, Weapon weapon, GameTime gameTime, List<Missile> missiles)
        {
            if (gameTime.TotalGameTime.TotalSeconds - timeLastShot.TotalSeconds > Unit.shotFrequency)
            {
                timeLastShot = new TimeSpan(gameTime.TotalGameTime.Days, gameTime.TotalGameTime.Hours, gameTime.TotalGameTime.Minutes, gameTime.TotalGameTime.Seconds, gameTime.TotalGameTime.Milliseconds);
                missiles.Add(Missile.FromWeapon(weapon,position,new Vector2(input.CurrentMouseState.X,input.CurrentMouseState.Y),timeLastShot,Faction.Fighters));
            }
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
