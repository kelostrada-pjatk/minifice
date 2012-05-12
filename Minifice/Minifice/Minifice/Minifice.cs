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
using Minifice.ScreenManagement;
using Minifice.MenuScreens;
#endregion

namespace Minifice
{
    /// <summary>
    /// Minifice - Cannon Fodder w nowym gmachu Mini.
    /// </summary>
    public class Minifice : Microsoft.Xna.Framework.Game
    {

        #region Pola

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        #endregion

        #region Inicjalizacja

        /// <summary>
        /// Konstruktor aplikacji
        /// </summary>
        public Minifice()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferWidth = 1024;
            //graphics.PreferredBackBufferHeight = 768;

            // Tworzenie Menadzera ekranow
            screenManager = new ScreenManager(this, graphics);
            screenManager.TraceEnabled = true;

            Components.Add(screenManager); // Dodanie screenManagera jako komponentu gry. Wiêcej wyjaœnione w odpowiednim rozdziale.

            screenManager.AddScreen(new BackgroundScreen(@"Menu\background"));
            screenManager.AddScreen(new MainMenuScreen());            
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        #endregion

        #region Draw


        /// <summary>
        /// Draw
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // Prawdziwe rysowanie i tak zachodzi w klasie ScreenManager
            base.Draw(gameTime);
        }


        #endregion
    }


    #region Program
    static class Program
    {
        static void Main(string[] args)
        {
            using (Minifice game = new Minifice())
            {
                game.Run();
            }
        }
    }
    #endregion
}
