#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Minifice.ScreenManagement;
using Minifice.Enums;
#endregion

namespace Minifice.MenuScreens
{
    abstract class MenuScreen : GameScreen
    {

        #region Pola

        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        string menuTitle;

        SoundEffect soundEffect;

        #endregion

        #region Właściwości


        /// <summary>
        /// Pobiera liste pozycji w menu
        /// </summary>
        protected List<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        #endregion

        #region Inicjalizacja


        /// <summary>
        /// Konstruktor
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            soundEffect = content.Load<SoundEffect>(@"Menu\thozi_daClick");
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// HandleInput
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input.IsMenuUp())
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            if (input.IsMenuDown())
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            int select;

            if (IsMenuHovered(input, out select))
            {
                selectedEntry = select;
            }

            // Sprawdzanie czy wcisnieta została akceptacja jakiejś pozycji w menu

            if (input.IsMenuKeySelect())
            {
                OnSelectEntry(selectedEntry);
            }
            else if (IsMenuMouseSelect(input, out select))
            {
                selectedEntry = select;
                OnSelectEntry(selectedEntry);
            }
            else if (input.IsMenuCancel())
            {
                OnCancel();
            }
            System.Diagnostics.Trace.Write(selectedEntry);
        }

        /// <summary>
        /// IsMenuMouseSelect
        /// </summary>
        private bool IsMenuMouseSelect(InputState input, out int selected)
        {
            Vector2 Coord;
            selected = 0;
            if (input.IsNewMouseLeftClick(out Coord))
            {
                foreach (MenuEntry M in MenuEntries)
                {
                    if (M.IsContained(Coord, this.ScreenManager.Font))
                    {
                        return true;
                    }
                    selected++;
                }
            }
            return false;
        }

        /// <summary>
        /// IsMenuHovered
        /// </summary>
        private bool IsMenuHovered(InputState input, out int selected)
        {
            Vector2 Coord;
            selected = 0;
            if (input.IsNewMouseMove(out Coord))
            {
                foreach (MenuEntry M in MenuEntries)
                {
                    if (M.IsContained(Coord, this.ScreenManager.Font))
                    {
                        if (!M.IsContained(new Vector2(input.LastMouseState.X, input.LastMouseState.Y), this.ScreenManager.Font) && ScreenManager.Settings.SoundEffects && selected != selectedEntry)
                        {
                            soundEffect.Play();
                        }
                        return true;
                    }
                    selected++;
                }
            }
            return false;
        }

        /// <summary>
        /// Handler dla wybrania pozycji w menu
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            menuEntries[entryIndex].OnSelectEntry();
        }


        /// <summary>
        /// Handler dla anulowania wyboru w menu
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper OnCancel
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Metoda sluzaca do umiejscawiania w odpowiedni sposob pozycji
        /// menu.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);


            Vector2 position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * 3 / 4, ScreenManager.GraphicsDevice.Viewport.Height *9/16);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];
                position.X = ScreenManager.GraphicsDevice.Viewport.Width * 3 / 4 - menuEntry.GetWidth(ScreenManager.Font) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;
                menuEntry.Position = position;
                position.Y += menuEntry.GetHeight(ScreenManager.Font);
            }
        }


        /// <summary>
        /// Update
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }


        /// <summary>
        /// Draw 
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Wyliczanie efektu "slide" który wykonują pozycje w menu
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * 3 / 4, ScreenManager.GraphicsDevice.Viewport.Height * 7 / 16);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(0, 0, 193) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                    titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion
    }
}
