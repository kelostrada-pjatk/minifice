#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minifice.ScreenManagement;
#endregion

namespace Minifice.MenuScreens
{
    class BackgroundScreen : GameScreen
    {
        #region Pola

        ContentManager content;
        Texture2D backgroundTexture;

        string texture;

        #endregion

        #region Inicjalizacja


        /// <summary>
        /// Konstruktor
        /// </summary>
        public BackgroundScreen(string texture)
        {
            this.texture = texture;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Ładujemy grafiki do nowego ContentMenadżera, 
        /// żeby móc je usunąć gdy menu zostanie wyłączone. 
        /// W ten sposób nie musimy trzymać grafik w pamięci przez całe życie aplikacji.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>(texture);
        }


        /// <summary>
        /// Metoda UnloadContent
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update i Draw


        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draw
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }


        #endregion
    }
}
