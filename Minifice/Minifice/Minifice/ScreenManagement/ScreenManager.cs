#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using Minifice.FileManagement;
using Minifice.Enums;
#endregion

namespace Minifice.ScreenManagement
{
    /// <summary>
    /// G³ówna klasa zarz¹dzaj¹ca wyœwietlanymi warstwami
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Pola

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        InputState input;
        FileManager fileManager = new FileManager();

        Settings settings;

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        GraphicsDeviceManager graphics;

        bool isInitialized;
        bool traceEnabled;

        #endregion

        #region W³aœciwoœci


        /// <summary>
        /// Wspólny SpriteBatch dzielony pomiedzy wszystkimi warstwami.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }


        /// <summary>
        /// Wspólna czcionka dzielona pomiedzy wszystkimi warstwami.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }


        /// <summary>
        /// Do debugowania, wyœwietla listê okien w menad¿erze
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        /// <summary>
        /// Ustawienia gry
        /// </summary>
        public Settings Settings
        {
            get
            {
                settings = fileManager.Deserialize<Settings>(@"Data\Settings");
                if (settings == default(Settings))
                {
                    settings = new Settings(new Vector2(1024, 768), true, true, false);
                    fileManager.Serialize<Settings>(@"Data\Settings", settings);
                }
                return settings;
            }
            set
            {
                Settings pom = fileManager.Deserialize<Settings>(@"Data\Settings");

                if (pom != default(Settings))
                {
                    settings = value;
                    fileManager.Serialize<Settings>(@"Data\Settings", value);
                }
                else
                {
                    fileManager.Serialize<Settings>(@"Data\Settings", this.Settings);
                }

                SetResolution();
            }
        }

        #endregion

        #region Inicjalizacja

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ScreenManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            this.graphics = graphics;
            input = new InputState();
            this.Settings = this.Settings;
        }


        /// <summary>
        /// Inicjowanie menad¿era
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }


        /// <summary>
        /// £adowanie Contentu
        /// </summary>
        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>(@"Menu\menufont");
            blankTexture = content.Load<Texture2D>(@"Menu\blank");

            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }


        /// <summary>
        /// Unload Content
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }


        #endregion

        #region Update i Draw


        /// <summary>
        /// Metoda Update
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            input.Update();

            screensToUpdate.Clear();
            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (screensToUpdate.Count > 0)
            {
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            if (traceEnabled)
                TraceScreens();
        }


        /// <summary>
        /// Wyœwietlanie listy warstw - podczas debugowania
        /// </summary>
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Trace.WriteLine(string.Join(", ", screenNames.ToArray()));
        }


        /// <summary>
        /// Metoda Draw
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }


        #endregion

        #region Metody Publiczne

        /// <summary>
        /// Ustawienia ekranu
        /// </summary>
        public void SetResolution()
        {
            graphics.PreferredBackBufferHeight = (int)settings.Resolution.Y;
            graphics.PreferredBackBufferWidth = (int)settings.Resolution.X;
            graphics.IsFullScreen = settings.FullScreen;
            graphics.ApplyChanges();
        }
        
        /// <summary>
        /// Dodawanie nowej warstwy do listy
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            if (isInitialized)
            {
                screen.LoadContent();
            }

            screens.Add(screen);
        }


        /// <summary>
        /// Usuwanie warstwy z listy. Raczej powinno u¿ywaæ siê 
        /// GameScreen.ExitScreen ¿eby warstwa mia³a czas na uruchomienie 
        /// efektu TransitionOff
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }


        /// <summary>
        /// Zwracanie kopii tablicy warstw
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }


        /// <summary>
        /// Funkcja pomocnicza rysujaca czarny prostokat ktory ma tworzyc 
        /// efekt Transition i dodatkowo zaciemniaæ okna typu Popup
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin();

            spriteBatch.Draw(blankTexture,
                                new Rectangle(0, 0, viewport.Width, viewport.Height),
                                Color.Black * alpha);

            spriteBatch.End();
        }


        #endregion
    }
}
