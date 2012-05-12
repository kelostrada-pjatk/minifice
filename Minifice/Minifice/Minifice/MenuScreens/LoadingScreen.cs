#region Using
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.ScreenManagement;
using Minifice.Enums;
#endregion

namespace Minifice.MenuScreens
{
    /// <summary>
    /// LoadingScreen dziala tak:
    /// - Każe wszystkim warstwom się wyłączyć.
    /// - Aktywuje LoadingScreen, ktory najpierw czeka aż wszystkie warstwy się wyłączą,
    /// a potem czeka aż nowa załaduje cały content. Zazwyczaj będzie to GamePlayScreen, więc
    /// będzie dużo do ładowania.
    /// </summary>
    class LoadingScreen : GameScreen
    {
        #region Pola

        bool loadingIsSlow;
        bool otherScreensAreGone;

        GameScreen[] screensToLoad;

        #endregion

        #region Inicjalizacja


        /// <summary>
        /// Konstruktor jest prywatny, bo LoadingScreen powinno być aktywowane tylko
        /// przez metodę statyczną Load.
        /// </summary>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, params GameScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);

            screenManager.AddScreen(loadingScreen);
        }


        #endregion

        #region Update i Draw

        /// <summary>
        /// Update
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen);
                    }
                }

                // Po skończeniu ładowania używam ResetElapsedTime, żeby powiedzieć grze
                // że skończone zostało ładowanie długiego frame'a i nie powinna próbować nadrabiać wyświetlania.
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Draw
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == ScreenState.Active) && (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Ładowanie...";

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }


        #endregion
    }
}
