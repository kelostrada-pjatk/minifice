#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minifice.ScreenManagement;
#endregion

namespace Minifice.MenuScreens
{
    /// <summary>
    /// Klasa odpowiadająca za pozycje w menu
    /// </summary>
    class MenuEntry
    {

        #region Pola

        /// <summary>
        /// Tekst w menu
        /// </summary>
        string text;

        /// <summary>
        /// 
        /// </summary>
        float selectionFade;

        /// <summary>
        /// Pozycja wyświetlania menu na ekranie. Ustawiane w Update przez MenuScreen
        /// </summary>
        Vector2 position;

        #endregion

        #region Właściwości


        /// <summary>
        /// Ustawianie/pobieranie tekstu z tej pozycji menu
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        /// <summary>
        /// Ustawianie/pobieranie pozycji
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        #endregion

        #region Eventy


        /// <summary>
        /// Event uruchamia się gdy pozycja zostanie wybrana
        /// </summary>
        public event EventHandler<EventArgs> Selected;


        /// <summary>
        /// Metoda uruchamiająca Event
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }


        #endregion

        #region Inicjalizacja


        /// <summary>
        /// Konstruktor
        /// </summary>
        public MenuEntry(string text)
        {
            this.text = text;
        }


        #endregion

        #region Update i Draw


        /// <summary>
        /// Update
        /// </summary>
        public virtual void Update(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        /// <summary>
        /// Draw
        /// </summary>
        public virtual void Draw(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.OrangeRed : Color.Black;

            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            color *= screen.TransitionAlpha;

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;


            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Sprawdza jak wysoka jest ta pozycja w menu
        /// </summary>
        public virtual int GetHeight(SpriteFont font)
        {
            return font.LineSpacing;
        }

        /// <summary>
        /// Sprawdza jak szeroka jest ta pozycja w menu
        /// </summary>
        public virtual int GetWidth(SpriteFont font)
        {
            return (int)font.MeasureString(Text).X;
        }


        #endregion

        #region Metody Publiczne

        /// <summary>
        /// Sprawdza czy punkt należy do obszaru zajmowanego przez MenuEntry
        /// </summary>
        public bool IsContained(Vector2 point, SpriteFont font)
        {
            float Left, Right, Bottom, Top;
            int Width = GetWidth(font);
            int Height = GetHeight(font);
            Left = position.X;
            Right = position.X + Width;
            Top = position.Y - Height/2;
            Bottom = position.Y + Height/2;
            return (point.X > Left && point.X < Right && point.Y > Top && point.Y < Bottom);
        }


        #endregion
    }
}
