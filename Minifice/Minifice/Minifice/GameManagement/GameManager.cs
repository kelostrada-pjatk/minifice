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
        public List<Weapon> Missiles;
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
            camera.Pos = new Vector2(700f, 700f);
            camera.Zoom = 1.2f;
        }

        public GameManager(Difficulty difficulty,ScreenManager screenManager) : this()
        {
            this.difficulty = difficulty;
            this.screenManager = screenManager;

            int tileCount = 50;
            camera.Zoom = screenManager.Settings.Resolution.Y / tileCount / GameMap.TileShift.Y * 2;
            
            GameMap = new GameMap(10,20);
            
            content = screenManager.Game.Content;

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 20; j++)
                    GameMap.mapTiles[i][j] = new MapTile(new BackgroundSprite(@"Game\texture", new Rectangle(152, 379, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());

            GameMap.Load(content);

            GameInterface = new GameInterface();
            GameInterface.Load(content);

            Fighters = new List<Fighter>();
            Enemies = new List<Enemy>();
            Bonuses = new List<Bonus>();
            Missiles = new List<Weapon>();
            weaponsArsenal = new Dictionary<Weapon,int>();

            //FileManager fileManager = new FileManager();
            //fileManager.Serialize<GameManager>(@"mission1", this);

            MapObject mo = new MapObject();
            List<Vector2> punkty = new List<Vector2>();
            punkty.Add(new Vector2(0, 11));
            punkty.Add(new Vector2(4, 13));
            punkty.Add(new Vector2(8, 13));
            punkty.Add(new Vector2(29, 2));
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

            for (int i = 0; i < 10; i++)
                GameMap.mapTiles[i][0].mapObjects.Add(mo);

            mo = new MapObject();
            punkty.Clear();
            punkty.Add(new Vector2(40, 13));
            punkty.Add(new Vector2(43, 13));
            punkty.Add(new Vector2(47, 11));
            punkty.Add(new Vector2(22, 0));
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
            

            for (int i = 0; i < 10; i++)
                GameMap.mapTiles[i][0].mapObjects.Add(mo);

            Fighter a = new Fighter(true);
            a.Load(content);

            Fighters.Add(a);


        }

        #endregion

        #region Metody Publiczne

        /// <summary>
        /// Metoda statyczna generujaca cala klase GameManager
        /// </summary>
        public static GameManager Load(ContentManager content,Difficulty difficulty,ScreenManager screenManager)
        {
            FileManager fileManager = new FileManager();
            GameManager newInstance = fileManager.Deserialize<GameManager>(@"Missions\Mission1");
            newInstance.Fighters = new List<Fighter>();
            newInstance.Bonuses = new List<Bonus>();
            newInstance.Missiles = new List<Weapon>();
            newInstance.GameMap.Load(content);
            foreach (Enemy e in newInstance.Enemies)
                e.Load(content);
            foreach (Fighter f in newInstance.Fighters)
                f.Load(content);
            foreach (KeyValuePair<Weapon,int> w in newInstance.weaponsArsenal)
                w.Key.Load(content);

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
            GameManager newInstance = fileManager.Deserialize<GameManager>(@"Missions\Mission"+missionId);
            newInstance.GameMap.Load(content);
            foreach (Enemy e in newInstance.Enemies)
                e.Load(content);
            foreach (Fighter f in newInstance.Fighters)
                f.Load(content);
            foreach (KeyValuePair<Weapon, int> w in newInstance.weaponsArsenal)
                w.Key.Load(content);

            MissionId = missionId;
            GameMap = newInstance.GameMap;
            Fighters = newInstance.Fighters;
            Enemies = newInstance.Enemies;
            Bonuses = newInstance.Bonuses;
            WeaponsArsenal = new List<DataItem<Weapon,int>>();
            Missiles = new List<Weapon>();
            
        }

        public void HandleInput(InputState input)
        {
            if (input.IsMouseLeftClickHold() || input.IsMouseRightClickHold())
            {
                // Myszka na interfejsie
                if (input.CurrentMouseState.X < GameInterface.Width)
                {
                    GameInterface.HandleInput(input);
                }
                // Myszka w grze
                else
                {
                    foreach (Fighter f in Fighters)
                    {
                        Vector2? pos = f.Move(screenManager.Settings.Resolution, GameMap, Fighters, Enemies, input);
                        if (pos != null)
                        {
                            camera.Pos = (Vector2)pos;
                        }
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Fighter f in Fighters)
            {
                f.Update(gameTime);
                f.Move(GameMap, Fighters, Enemies);
            }
            foreach (Enemy e in Enemies)
            {
                e.Update(gameTime);
                e.Move(GameMap, Fighters, Enemies);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(screenManager.GraphicsDevice));

            //spriteBatch.Begin();

            //SpriteFont spriteFont = content.Load<SpriteFont>(@"Menu\menufont");
            //spriteBatch.DrawString(spriteFont, "TESTTESTTEST", new Vector2(100, 100), Color.White);
            //spriteBatch.Draw(texture, new Rectangle(100,100,1000,1000), Color.White);

            //spriteBatch.Draw(texture, new Rectangle(0, 0, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y), new Rectangle(152, 379, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y), Color.White, 0, new Vector2(), SpriteEffects.None, 0);

            GameMap.Draw(gameTime, spriteBatch);
            foreach (Fighter f in Fighters)
                f.Draw(gameTime, spriteBatch);
            foreach (Enemy e in Enemies)
                e.Draw(gameTime, spriteBatch);
            foreach (Weapon w in Missiles)
                w.Draw(gameTime, spriteBatch);

            spriteBatch.End();


            // Inicjuje nowego spriteBatcha, żeby interfejs nie przesuwal sie z kamerą
            spriteBatch.Begin();

            GameInterface.Draw(spriteBatch);

            spriteBatch.End();

        }

        #endregion
    }
}
