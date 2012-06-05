#region Using
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Minifice.ScreenManagement;
#endregion

namespace Minifice.MenuScreens
{
    /// <summary>
    /// Popup MessageBox, tyle że w stylu warstwowania zawartego w aplikacji
    /// </summary>
    class LostGameScreen : GameScreen
    {
        #region Pola

        string message;
        Texture2D gradientTexture;
        List<MenuEntry> choices = new List<MenuEntry>();
        int selected=0;

        SoundEffect soundEffect;

        #endregion

        #region Właściwości

        /// <summary>
        /// Pobiera liste pozycji w menu
        /// </summary>
        protected List<MenuEntry> MenuEntries
        {
            get { return choices; }
        }

        #endregion

        #region Eventy

        public event EventHandler<EventArgs> Accepted;

        #endregion

        #region Inicjalizacja

        /// <summary>
        /// Konstruktor
        /// </summary>
        public LostGameScreen(string message)
        {
            this.message = message;
            choices.Add(new MenuEntry("OK"));

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        /// <summary>
        /// Korzystamy z ContentManagera ze ScreenManagera. Dzieki temu nie musimy za kazdym razem ladowac tekstury do pamieci
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>(@"Menu\gradient");
            soundEffect = content.Load<SoundEffect>(@"Menu\thozi_daClick");
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// HandleInput
        /// </summary>
public override void HandleInput(InputState input)
{
    choices[0].Selected += Accepted;

    if (input.IsMenuLeft())
    {
        selected--;

        if (selected < 0)
            selected = choices.Count - 1;
    }

    if (input.IsMenuRight())
    {
        selected++;

        if (selected >= choices.Count)
            selected = 0;
    }

    int select;

    if (IsMenuHovered(input, out select))
    {
        selected = select;
    }

    // Sprawdzanie czy wcisnieta została akceptacja jakiejś pozycji w menu

    if (input.IsMenuKeySelect())
    {
        OnSelectEntry(selected);
    }
    else if (IsMenuMouseSelect(input, out select))
    {
        selected = select;
        OnSelectEntry(selected);
    }
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
                        if (!M.IsContained(new Vector2(input.LastMouseState.X, input.LastMouseState.Y), this.ScreenManager.Font) && ScreenManager.Settings.SoundEffects && selected != this.selected)
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
        /// Handler dla potrzeb MenuEntry
        /// </summary>
        private void OnSelectEntry(int entryIndex)
        {
            choices[entryIndex].OnSelectEntry();
        }

        #endregion

        #region Update i Draw

        /// <summary>
        /// Update
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < choices.Count; i++)
            {
                bool isSelected = IsActive && (i == selected);

                choices[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            textSize.Y += choices[0].GetHeight(ScreenManager.Font);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;

            

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                            (int)textPosition.Y - vPad,
                                                            (int)textSize.X + hPad * 2,
                                                            (int)textSize.Y + vPad * 2);

            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);
            spriteBatch.DrawString(font, message, textPosition, color);


            // Rysowanie pozycji menu
            Vector2 position = textPosition;
            position.X += textSize.X / 2 - choices[0].GetWidth(ScreenManager.Font)/2;
            position.Y += textSize.Y / 2 + 2*vPad;
            choices[0].Position = position;

            for (int i = 0; i < choices.Count; i++)
            {
                MenuEntry menuEntry = choices[i];

                bool isSelected = IsActive && (i == selected);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.End();
        }


        #endregion
    }
}
