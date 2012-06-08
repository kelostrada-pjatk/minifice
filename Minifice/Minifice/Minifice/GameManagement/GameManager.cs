#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.Xml.Serialization;
using Minifice.Enums;
using Minifice.ScreenManagement;
using Minifice.FileManagement;
using Microsoft.Xna.Framework.Input;
using Minifice.GameManagement.Movement;
using Minifice.GameManagement.Shooting;
using Minifice.MenuScreens;
#endregion

namespace Minifice.GameManagement
{
    public class GameManager
    {
        #region Pola

        public int MissionId = 1;
        [XmlIgnore]
        public int Points = 0;
        Difficulty difficulty;
        [XmlIgnore]
        ScreenManager screenManager;
        public GameMap GameMap;
        [XmlIgnore]
        public GameInterface GameInterface;
        [XmlIgnore]
        public List<Fighter> Fighters;
        public List<Enemy> Enemies;
        [XmlIgnore]
        public List<Bonus> Bonuses;
        Dictionary<Weapon, int> weaponsArsenal = new Dictionary<Weapon,int>();
        [XmlIgnore]
        public List<Missile> Missiles;
        [XmlIgnore]
        ContentManager content;
        [XmlIgnore]
        Camera2d camera = new Camera2d();

        #endregion

        #region Właściwości

        /// <summary>
        /// Oszukanczy properties, zeby mozna bylo serializowac Dictionary
        /// </summary>
        public List<DataItem<Weapon, int>> WeaponsArsenal
        {
            get
            {
                List<DataItem<Weapon, int>> s = new List<DataItem<Weapon, int>>();
                foreach (Weapon key in weaponsArsenal.Keys)
                {
                    s.Add(new DataItem<Weapon, int>(key, weaponsArsenal[key]));
                }
                return s;
            }
            set
            {
                foreach (var di in value)
                {
                    weaponsArsenal.Add(di.Key, di.Value);
                }
            }
        }

        #endregion

        #region Inicjalizacja

        public GameManager()
        {
            camera.Pos = new Vector2(200f, 200f);
            camera.Zoom = 1f;
        }

        public GameManager(Difficulty difficulty,ScreenManager screenManager) : this()
        {
            this.difficulty = difficulty;
            this.screenManager = screenManager;

            //int tileCount = 50;
            //camera.Zoom = screenManager.Settings.Resolution.Y / tileCount / GameMap.TileShift.Y * 2;
            
            content = screenManager.Game.Content;

            Level1();

            GameInterface = new GameInterface();
            GameInterface.Load(content);

            Fighters = new List<Fighter>();
            Enemies = new List<Enemy>();
            Bonuses = new List<Bonus>();
            Missiles = new List<Missile>();
            weaponsArsenal = new Dictionary<Weapon,int>();

            //FileManager fileManager = new FileManager();
            //fileManager.Serialize<GameManager>(@"mission1", this);

            

            Fighter a = new Fighter(true, new Vector2(700f,700f), camera);
            a.Load(content);
            a.moveStrategy = new GotoPoint(GameMap, Fighters, Enemies, a);

            Fighter b = new Fighter(false, new Vector2(730f, 730f), camera);
            b.Load(content);
            b.moveStrategy = new Follow(GameMap, Fighters, Enemies, b, a);

            Fighter c = new Fighter(false, new Vector2(760f, 760f), camera);
            c.Load(content);
            c.moveStrategy = new Follow(GameMap, Fighters, Enemies, c, b);

            Fighter d = new Fighter(false, new Vector2(790f, 790f), camera);
            d.Load(content);
            d.moveStrategy = new Follow(GameMap, Fighters, Enemies, d, c);

            Fighter e = new Fighter(false, new Vector2(820f, 820f), camera);
            e.Load(content);
            e.moveStrategy = new Follow(GameMap, Fighters, Enemies, e, d);

            Fighters.Add(a);
            Fighters.Add(b);
            Fighters.Add(c);
            Fighters.Add(d);
            Fighters.Add(e);

            camera.Pos = a.position;

            Enemy A = new Enemy(new Vector2(30f, 43f), camera);
            A.Load(content);
            A.moveStrategy = new Patrol(GameMap, Fighters, Enemies, A, new Vector2(230f, 220f), new Vector2(470f, 280f), 1.4f);

            Enemies.Add(A);

            A = new Enemy(new Vector2(40f, 23f), camera);
            A.Load(content);
            A.moveStrategy = new Patrol(GameMap, Fighters, Enemies, A, new Vector2(330f, 320f), new Vector2(570f, 380f), 1.7f);

            Enemies.Add(A);

            A = new Enemy(new Vector2(40f, 55f), camera);
            A.Load(content);
            A.moveStrategy = new Patrol(GameMap, Fighters, Enemies, A, new Vector2(400f, 220f), new Vector2(100f, 500f), 2.3f);

            Enemies.Add(A);

        }

        private void Level1()
        {
            
            GameMap = new GameMap(150, 150);

            MapTile t1 = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            MapTile t2 = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 291, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            MapTile t3 = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 317, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            MapObject mo = new MapObject();
            List<Vector2> punkty = new List<Vector2>();
            punkty.Add(new Vector2(-0.5f, 7));
            punkty.Add(new Vector2(24, 23));
            punkty.Add(new Vector2(48.5f, 7));
            punkty.Add(new Vector2(24, -2));

            mo.boundaries = Boundaries.CreateFromPoints(punkty);
            mo.collectible = false;
            mo.Source = new Source(539, 0, 48, 47);
            mo.textureName = @"Game\newtile";
            mo.throwableOver = true;
            mo.type = new Bonus();
            mo.viewBlocking = true;
            mo.origin = new Vector2(0, 24);
            mo.Load(content);
            
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    GameMap[i + j / 2, 40 - 2 * i + j] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
                    GameMap[i + j / 2 + 1, 40 - 2 * i + j] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 291, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
                }
            }

            for (int i = 0; i < 42; i++)
            {
                GameMap[i / 2, 42 - i].mapObjects.Add(mo);
                GameMap[i / 2 + 20, 82 - i].mapObjects.Add(mo);
            }
            
            GameMap[0, 43].mapObjects.Add(mo);
            GameMap[1, 44].mapObjects.Add(mo);

            GameMap[20, 30].mapObjects.Add(mo);

            GameMap[4, 60] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            GameMap[4, 60].mapObjects.Add(mo);
            
            for (int i = 0; i < 70; i++)
            {
                GameMap[7, i*5] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            }


            GameMap[0, 0] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            GameMap[1, 1] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            GameMap[10, 2] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            GameMap[10, 3] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            GameMap[13, 2] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
            GameMap[13, 3] = new MapTile(new BackgroundSprite(@"Game\fire1", new Rectangle(6, 267, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());


            GameMap.Load(content);
            
            /*
            GameMap = new GameMap(50, 100);

            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 100; j++)
                    GameMap.mapTiles[i][j] = new MapTile(new BackgroundSprite(@"Game\texture", new Rectangle(152, 379, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());

            GameMap.Load(content);

            MapObject mo = new MapObject();
            List<Vector2> punkty = new List<Vector2>();
            punkty.Add(new Vector2(1, 6));
            punkty.Add(new Vector2(4, 13));
            punkty.Add(new Vector2(8, 13));
            punkty.Add(new Vector2(25, 0));
            mo.boundaries = Boundaries.CreateFromPoints(punkty);
            mo.collectible = false;
            mo.Source = new Source(372, 429, 30, 38);
            mo.textureName = @"Game\texture";
            mo.throwableOver = true;
            mo.type = new Bonus();
            mo.viewBlocking = true;
            mo.origin = new Vector2(0, 24);
            mo.Load(content);

            for (int i = 0; i < 50; i++)
                GameMap.mapTiles[i][10].mapObjects.Add(mo);

            mo = new MapObject();
            punkty.Clear();
            punkty.Add(new Vector2(40, 13));
            punkty.Add(new Vector2(43, 13));
            punkty.Add(new Vector2(46, 6));
            punkty.Add(new Vector2(25, 0));
            //punkty.Add(new Vector2(18, 2));
            mo.boundaries = Boundaries.CreateFromPoints(punkty);
            mo.collectible = false;
            mo.Source = new Source(403, 429, 30, 38);
            mo.textureName = @"Game\texture";
            mo.throwableOver = true;
            mo.type = new Bonus();
            mo.viewBlocking = true;
            mo.origin = new Vector2(-18, 24);
            mo.Load(content);


            for (int i = 0; i < 50; i++)
                GameMap.mapTiles[i][10].mapObjects.Add(mo);
            */
        }


        /// <summary>
        /// Metoda statyczna generujaca cala klase GameManager
        /// </summary>
        public static GameManager Load(ContentManager content, Difficulty difficulty, ScreenManager screenManager)
        {
            FileManager fileManager = new FileManager();
            GameManager newInstance = fileManager.Deserialize<GameManager>(@"Missions\Mission1");
            newInstance.Fighters = new List<Fighter>();
            newInstance.Bonuses = new List<Bonus>();
            newInstance.Missiles = new List<Missile>();
            newInstance.GameMap.Load(content);
            foreach (Enemy e in newInstance.Enemies)
                e.Load(content);
            foreach (Fighter f in newInstance.Fighters)
                f.Load(content);

            newInstance.difficulty = difficulty;
            newInstance.screenManager = screenManager;
            newInstance.MissionId = 1;
            newInstance.content = content;

            newInstance.GameInterface = new GameInterface();
            newInstance.GameInterface.Load(content);

            return newInstance;
        }

        public void LoadMission(int missionId)
        {
            // TU EWENTUALNIE MOŻNA BY DODAC JAKIS KOD NA OKNO WCZYTYWANIA NOWEGO POZIOMU

            FileManager fileManager = new FileManager();
            GameManager newInstance = fileManager.Deserialize<GameManager>(@"Missions\Mission" + missionId);
            newInstance.GameMap.Load(content);
            foreach (Enemy e in newInstance.Enemies)
                e.Load(content);
            foreach (Fighter f in newInstance.Fighters)
                f.Load(content);

            MissionId = missionId;
            GameMap = newInstance.GameMap;
            Fighters = newInstance.Fighters;
            Enemies = newInstance.Enemies;
            Bonuses = newInstance.Bonuses;
            WeaponsArsenal = new List<DataItem<Weapon, int>>();
            Missiles = new List<Missile>();

        }

        #endregion

        #region Metody Publiczne

        public void HandleInput(InputState input)
        {
            if (input.IsNewMouseLeftClick(out MouseCord) || input.IsNewMouseRightClick(out MouseCord) || input.IsMouseLeftClickHold())
            {
                // Myszka na interfejsie
                if (input.CurrentMouseState.X < GameInterface.Width)
                {
                    GameInterface.HandleInput(input);
                }
                // Myszka w grze
                else
                {
                    if (input.IsNewMouseRightClick(out MouseCord))
                    {
                        foreach (Fighter f in Fighters)
                        {
                            if ((!f.IsAlive && f.isDying) || f.IsAlive)
                                f.Move(screenManager.Settings.Resolution, GameMap, Fighters, Enemies, input);
                        }
                    }
                    if (input.IsMouseLeftClickHold())
                    {
                        foreach (Fighter f in Fighters)
                        {
                            if ((!f.IsAlive && f.isDying) || f.IsAlive)
                                f.Shoot(screenManager, input, Weapon.Gun, screenManager.GameTime, Missiles);
                        }
                    }
                }
                
            }

            if (input.IsNewKeyPress(Keys.Z))
            {
                if (Fighters.Count>0)
                Fighters[0].Die(Fighters);
                if (Fighters.Count>2)
                Fighters[2].Die(Fighters);
            }
        }

        public void Update(GameTime gameTime)
        {
            int c=0;

            foreach (var f in Fighters)
            {
                if (!f.Update(gameTime) && ((!f.IsAlive && f.isDying) || f.IsAlive))
                {
                    Vector2? pos = f.Move(GameMap, Fighters, Enemies);
                    if (pos != null && f.PlayerControlled)
                        camera.Pos = (Vector2)pos;
                }
                else
                    f.isDying = false;
                if (f.IsAlive)
                    c++;
            }

            if (c == 0)
            {
                LostGameScreen lgs = new LostGameScreen("Przegrałeś!");
                lgs.Accepted += delegate(object sender, EventArgs e)
                {
                    LoadingScreen.Load(screenManager, false, new BackgroundScreen(@"Menu\background"), new MainMenuScreen());
                };
                screenManager.AddScreen(lgs);
            }

            c = Enemies.Count;
            for (int i = 0; i < c; i++)
            {
                if (Enemies[i].Update(gameTime))
                {
                    Enemies.RemoveAt(i);
                    i--;
                    c--;
                }
                else
                {
                    Enemies[i].Move(GameMap, Fighters, Enemies);
                    Enemies[i].Notice(GameMap, Fighters, Enemies);
                    Enemies[i].Shoot(screenManager, Weapon.Gun, gameTime, Missiles);
                }
            }

            c = Missiles.Count;
            for (int i = 0; i < c; i++)
            {
                if (Missiles[i].Update(gameTime, GameMap, Fighters, Enemies))
                {
                    Missiles.RemoveAt(i);
                    i--;
                    c--;
                }
            }
        }

        private Vector2 MouseCord = Vector2.Zero;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(screenManager.GraphicsDevice));

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //SpriteFont spriteFont = content.Load<SpriteFont>(@"Menu\menufont");
            //spriteBatch.DrawString(spriteFont, "TESTTESTTEST", new Vector2(100, 100), Color.White);
            //spriteBatch.Draw(texture, new Rectangle(100,100,1000,1000), Color.White);

            //spriteBatch.Draw(texture, new Rectangle(0, 0, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y), new Rectangle(152, 379, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y), Color.White, 0, new Vector2(), SpriteEffects.None, 0);

            GameMap.Draw(gameTime, spriteBatch);
            foreach (Fighter f in Fighters)
                if ((!f.IsAlive && f.isDying) || f.IsAlive)
                    f.Draw(gameTime, spriteBatch);
            foreach (Enemy e in Enemies)
                e.Draw(gameTime, spriteBatch);
            foreach (Missile m in Missiles)
                m.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            SpriteFont font = screenManager.Font;

            // Inicjuje nowego spriteBatcha, żeby interfejs nie przesuwal sie z kamerą
            spriteBatch.Begin();

            GameInterface.Draw(spriteBatch);
            //Vector2 posClick = new Vector2(MouseCord.X - GameInterface.Width - (screenManager.Settings.Resolution.X - GameInterface.Width) / 2 + GameInterface.Width - Fighters[0].position.X, MouseCord.Y - screenManager.Settings.Resolution.Y / 2 - Fighters[0].position.Y);
            Vector2 posClick = new Vector2(MouseCord.X - GameInterface.Width - (screenManager.Settings.Resolution.X - GameInterface.Width) / 2, MouseCord.Y - screenManager.Settings.Resolution.Y / 2 );
            posClick += camera.Pos;

            spriteBatch.DrawString(font, (MouseCord.X-GameInterface.Width).ToString() + "," + MouseCord.Y.ToString(), new Vector2(10,0), Color.Red);
            spriteBatch.DrawString(font, ((int)posClick.X).ToString() + "," + ((int)posClick.Y).ToString(), new Vector2(10, 50), Color.Red);
            spriteBatch.DrawString(font, ((int)Fighters[0].position.X).ToString() + "," + ((int)Fighters[0].position.Y).ToString(), new Vector2(10, 100), Color.Red);

            spriteBatch.DrawString(font, "{" + camera.Pos.GetMapPosition(GameMap).X + "," + camera.Pos.GetMapPosition(GameMap).Y + "}", new Vector2(10, 150), Color.Purple);

            spriteBatch.DrawString(font, "{" + posClick.GetMapPosition(GameMap).X + "," + posClick.GetMapPosition(GameMap).Y + "}", new Vector2(10, 200), Color.Plum);

            spriteBatch.DrawString(font, "{" + Math.Floor(posClick.X * 2 / GameMap.TileShift.X) + "," + Math.Floor(posClick.Y * 2 / GameMap.TileShift.Y) + "}", new Vector2(10, 250), Color.Maroon);

            Vector2 test = new Vector2(47, 3);
            test.GetMapPosition(GameMap);

            spriteBatch.End();

        }

        #endregion
    }
}
