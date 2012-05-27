#region Using
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
#endregion

namespace Minifice.ScreenManagement
{
    public class InputState
    {
        #region Pola

        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;
        MouseState currentMouseState;
        MouseState lastMouseState;
        
        #endregion

        #region Właściwości

        public KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }

        public KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }

        public MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }

        public MouseState LastMouseState
        {
            get { return lastMouseState; }
        }
        
        #endregion

        #region Inicjalizacja

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            currentKeyboardState = new KeyboardState();
            lastKeyboardState = new KeyboardState();
            currentMouseState = new MouseState();
            lastMouseState = new MouseState();
        }

        #endregion

        #region Metody Publiczne

        /// <summary>
        /// Zczytuje ostatnie stany klawiatury i myszki
        /// </summary>
        public void Update()
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Sprawdzanie czy zostal wcisniety nowy guzik na klawiaturze
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Sprawdzanie czy nacisnieto (i odcisnieto) lewy guzik myszki
        /// W parametrze Coord zwracam pozycje klikniecia
        /// </summary>
        public bool IsNewMouseLeftClick(out Vector2 Coord)
        {
            Coord = new Vector2(currentMouseState.X,currentMouseState.Y);
            return (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Sprawdzanie czy nacisnieto (i odcisnieto) prawy guzik myszki
        /// W parametrze Coord zwracam pozycje klikniecia
        /// </summary>
        public bool IsNewMouseRightClick(out Vector2 Coord)
        {
            Coord = new Vector2(currentMouseState.X, currentMouseState.Y);
            return (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released);
        }

        /// <summary>
        /// Sprawdzanie czy myszka sie przesunela
        /// W parametrze Coord zwracam nowa pozycje myszki
        /// </summary>
        public bool IsNewMouseMove(out Vector2 Coord)
        {
            Coord = new Vector2(currentMouseState.X, currentMouseState.Y);
            return (currentMouseState.X != lastMouseState.X || currentMouseState.Y != lastMouseState.Y);
        }

        /// <summary>
        /// Sprawdzanie czy lewy klawisz myszki jest przytrzymany
        /// </summary>
        public bool IsMouseLeftClickHold()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Sprawdzanie czy lewy klawisz myszki jest przytrzymany
        /// </summary>
        /// <param name="Coord">zwracana pozycja myszki</param>
        public bool IsMouseLeftClickHold(out Vector2 Coord)
        {
            Coord = new Vector2(currentMouseState.X, currentMouseState.Y);
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Sprawdzanie czy prawy klawisz myszki jest przytrzymany
        /// </summary>
        public bool IsMouseRightClickHold()
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Sprawdzanie czy prawy klawisz myszki jest przytrzymany
        /// </summary>
        /// <param name="Coord">zwracana pozycja myszki</param>
        public bool IsMouseRightClickHold(out Vector2 Coord)
        {
            Coord = new Vector2(currentMouseState.X, currentMouseState.Y);
            return currentMouseState.RightButton == ButtonState.Pressed;
        }



        /// <summary>
        /// IsMenuUp
        /// </summary>
        public bool IsMenuUp()
        {
            return IsNewKeyPress(Keys.Up);
        }

        /// <summary>
        /// IsMenuDown
        /// </summary>
        public bool IsMenuDown()
        {
            return IsNewKeyPress(Keys.Down);
        }

        /// <summary>
        /// IsMenuLeft
        /// </summary>
        public bool IsMenuLeft()
        {
            return IsNewKeyPress(Keys.Left);
        }

        /// <summary>
        /// IsMenuRight
        /// </summary>
        public bool IsMenuRight()
        {
            return IsNewKeyPress(Keys.Right);
        }

        /// <summary>
        /// IsMenuKeySelect
        /// </summary>
        public bool IsMenuKeySelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter);
        }

        /// <summary>
        /// IsMenuCancel
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }

        /// <summary>
        /// IsPauseGame
        /// </summary>
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }

        #endregion
    }
}
