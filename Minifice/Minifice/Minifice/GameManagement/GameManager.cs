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
            camera.Pos = new Vector2(150f, 150f);
            camera.Zoom = 4f;
        }

        public GameManager(Difficulty difficulty,ScreenManager screenManager)
        {
            this.difficulty = difficulty;
            this.screenManager = screenManager;
            camera.Pos = new Vector2(200f, 200f);
            camera.Zoom = 4f;
            GameMap = new GameMap(10,20);

            content = screenManager.Game.Content;

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 20; j++)
                {
                    GameMap.mapTiles[i][j] = new MapTile(new BackgroundSprite(@"Game\texture", new Rectangle(152, 379, (int)GameMap.TileShift.X, (int)GameMap.TileShift.Y)), new List<MapObject>());
                    GameMap.mapTiles[i][j].Load(content);
                }



            Fighters = new List<Fighter>();
            Enemies = new List<Enemy>();
            Bonuses = new List<Bonus>();
            Missiles = new List<Weapon>();
            weaponsArsenal = new Dictionary<Weapon,int>();

            FileManager fileManager = new FileManager();
            fileManager.Serialize<GameManager>(@"mission1", this);

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
            
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(screenManager.GraphicsDevice));

            //spriteBatch.Begin();

            Texture2D texture = content.Load<Texture2D>(@"Game\texture");
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

            // TODO - rysuj interfejs po lewej stronie

            spriteBatch.End();

        }

        #endregion
    }
}
