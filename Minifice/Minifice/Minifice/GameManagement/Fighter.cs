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
using Minifice.GameManagement.Movement;
#endregion

namespace Minifice.GameManagement
{
    public class Fighter : Unit
    {
        #region Pola

        bool playerControlled;
        bool isActive;
        
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

        public Fighter(bool playerControlled, Vector2 position, Camera2d camera)
            : base(camera)
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

            this.animationDeath = new Animation(20, 15, Direction.Left);

            AnimationFrame frame;
            for (int i = 1; i <= 10; i++)
            {
                frame = new AnimationFrame(@"Game\AnimationDeath\mini1_klatka" + i.ToString(), new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
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
            
            this.health = 5;
            this.isActive = true;
            this.speed = 0.95f;
            this.timeLastShot = new TimeSpan();

            this.destination = new Vector2(position.X, position.Y);

        }

        #endregion

        #region Implementacje Metod

        public void Move(Vector2 resolution, GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, InputState input)
        {
            if (playerControlled && !isDying)
                Destination = new Vector2(input.CurrentMouseState.X - GameInterface.Width - (resolution.X - GameInterface.Width) / 2 + camera.Pos.X, input.CurrentMouseState.Y - resolution.Y / 2 + camera.Pos.Y);
        }

        
        public override Vector2? Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            if (moveStrategy != null && !isDying)
                return moveStrategy.Move(currentTime);

            return null;
        }

        public void Shoot(ScreenManager screenManager, InputState input, Weapon weapon, GameTime gameTime, List<Missile> missiles)
        {
            if (gameTime.TotalGameTime.TotalSeconds - timeLastShot.TotalSeconds > Unit.shotFrequency && !isDying)
            {
                timeLastShot = new TimeSpan(gameTime.TotalGameTime.Days, gameTime.TotalGameTime.Hours, gameTime.TotalGameTime.Minutes, gameTime.TotalGameTime.Seconds, gameTime.TotalGameTime.Milliseconds);
                missiles.Add(Missile.FromWeapon(weapon, position, new Vector2(input.CurrentMouseState.X - GameInterface.Width - (screenManager.Settings.Resolution.X - GameInterface.Width) / 2 + camera.Pos.X, input.CurrentMouseState.Y - screenManager.Settings.Resolution.Y / 2 + camera.Pos.Y), timeLastShot, Faction.Fighters, gameTime));
                missiles.Last().Load(screenManager.Game.Content);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, position.Y /(GameMap.TileShift.Y / 2) * 0.001f + 0.001f);
        }

        public override void Die(List<Fighter> fighters)
        {
            base.Die(fighters);
            
            if (isDying)
            {
                for (int i = 0; i < fighters.Count; i++)
                {
                    if (fighters[i].Equals(this))
                    {
                        if (fighters[i].PlayerControlled)
                        {
                            bool b = false;
                            foreach (var f in fighters)
                            {
                                if (!f.Equals(this) && f.IsAlive && !f.isDying)
                                {
                                    this.PlayerControlled = false;
                                    
                                    f.PlayerControlled = true;
                                    f.moveStrategy = moveStrategy;
                                    f.moveStrategy.unit = f;
                                    f.Destination = this.Destination;
                                    this.moveStrategy = null;
                                    this.Destination = this.position;
                                    b = true;
                                }
                                if (b) break;
                            }
                        }
                        else
                        {
                            foreach (var f in fighters)
                            {
                                if (f.moveStrategy is Follow)
                                    if (((Follow)f.moveStrategy).destination.Equals(this))
                                    {
                                        ((Follow)f.moveStrategy).destination = ((Follow)this.moveStrategy).destination;
                                    }
                            }
                        }
                    }
                }
            }

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
