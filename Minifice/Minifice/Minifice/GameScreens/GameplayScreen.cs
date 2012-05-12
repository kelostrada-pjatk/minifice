#region Using
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minifice.ScreenManagement;
using Minifice.MenuScreens;
using Minifice.Enums;
using Minifice.GameManagement;
#endregion

namespace Minifice.GameScreens
{

    /// <summary>
    /// Główny ekran gry
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Pola

        ContentManager content;

        GameManager gameManager;
        Difficulty difficulty;

        float pauseAlpha;

        #endregion

        #region Inicjalizacja


        /// <summary>
        /// Konstruktor
        /// </summary>
        public GameplayScreen(Difficulty difficulty)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.difficulty = difficulty;
        }


        /// <summary>
        /// Ładowanie contentu
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //gameManager = GameManager.Load(content,difficulty,ScreenManager);
            gameManager = new GameManager(Difficulty.Easy, ScreenManager);

            Thread.Sleep(1000);

            ScreenManager.Game.ResetElapsedTime();
        }



        /// <summary>
        /// Unload
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update i Draw

        /// <summary>
        /// Update
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                gameManager.Update(gameTime);
            }
        }


        /// <summary>
        /// Handle Input
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            KeyboardState keyboardState = input.CurrentKeyboardState;

            if (input.IsPauseGame())
            {
                MessageBoxScreen ExitGame = new MessageBoxScreen("Wyjść z gry?");
                ExitGame.Accepted += delegate(object sender, EventArgs e)
                {
                    LoadingScreen.Load(ScreenManager, false, new BackgroundScreen(@"Menu\background"), new MainMenuScreen());                    
                };
                ScreenManager.AddScreen(ExitGame);
            }
            else
            {
                gameManager.HandleInput(input);
            }
        }


        /// <summary>
        /// Draw
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            gameManager.Draw(gameTime, spriteBatch);


            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
