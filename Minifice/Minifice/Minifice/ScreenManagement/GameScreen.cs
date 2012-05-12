#region Using
using System;
using Microsoft.Xna.Framework;
using Minifice.Enums;
#endregion

namespace Minifice.ScreenManagement
{
    /// <summary>
    /// Klasa Screena. Kazdy Screen jest oddzielna warstwa wyswietlana aplikacji.
    /// Na przyklad kazde menu jest typu GameScreen, sama gra tez jest typu GameScreen.
    /// Cala aplikacja sklada sie z nalozonych na siebie Screenow i wyswietlanych w opdowiedni
    /// sposob przez ScreenManager.
    /// </summary>
    public abstract class GameScreen
    {
        #region Właściowości

        /// <summary>
        /// Sprawdzanie czy Screen jest typu PopUp. Chodzi o to, zeby warstwa nizej wiedziala 
        /// czy trzeba sie chowac calkowicie, czy nie, bo okna typu PopUp nie zaslaniaja calego okna.
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;

        /// <summary>
        /// Jak długo warstwie zajmie pojawienie sie (ustawienie dlugosci efektu FadeIn)
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        TimeSpan transitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// Jak długo warstwie zajmie pojawienie sie (ustawienie dlugosci efektu FadeOut)
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        TimeSpan transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// "Pozycja" warstwy, oznacza etap na jakim jest efekt FadeIn lub FadeOut.
        /// Zero oznacza, że okno jest w pełni aktywne, bez Fade'a, Jeden oznacza,
        /// że warstwa się nie wyświetla. (Efekt Fade w pełni się wykonał).
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;

        /// <summary>
        /// Aktualna Alpha efektu Fade. Odwrotność TransistionPosition.
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        /// <summary>
        /// Pobieranie aktualnego stanu warstwy (na bazie enum ScreenState).
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransitionOn;

        /// <summary>
        /// Są dwa powody dla których warstwa może znikać.
        /// Pierwszy to znikanie tymczasowe, żeby ustąpić miejsca innemu oknu
        /// (np. Popup).
        /// Drugi to znikanie "na dobre". Wtedy chcemy na końcu usunąć warstwę.
        /// Ustawienie IsExiting powoduje, że na końcu Transtition warstwa zostanie
        /// usunięta na dobre.
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;

        /// <summary>
        /// Sprawdzanie czy warstwa jest aktywna i może odpowiedzieć na Input użytkownika
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }

        bool otherScreenHasFocus;

        /// <summary>
        /// Pobieranie menadżera warstw
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;

        #endregion

        #region Inicjalizacja


        /// <summary>
        /// Ładowanie Contentu dla warstwy
        /// </summary>
        public virtual void LoadContent() { }


        /// <summary>
        /// Unload Contentu dla warstwy
        /// </summary>
        public virtual void UnloadContent() { }


        #endregion

        #region Update i Draw


        /// <summary>
        /// Pozwala warstwie uruchamiać operacje takie jak aktualizacja efektu Fade.
        /// Metoda Update wykonywana jest zawsze (nawet jak okno jest nieaktywne)
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                // Jeśli warstwa ma zostać usunięta to powinna też zacząć znikać
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // Kiedy efekt znikania się skończy to usuwamy warstwe
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // Jeśli warstwa jest przykryta przez inną to powinna zacząć znikać
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Wyświetlamy warstwe żeby uczynić ją aktywną
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    screenState = ScreenState.Active;
                }
            }
        }


        /// <summary>
        /// Metoda pomocnicza do aktualizacji efektu Fade
        /// </summary>
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            transitionPosition += transitionDelta * direction;

            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Pozwala warstwie obsługiwać Input użytkownika. 
        /// Działa tylko jeśli warstwa jest w stanie Active.
        /// </summary>
        public virtual void HandleInput(InputState input) { }

        /// <summary>
        /// Metoda uruchamiana gdy warstwa ma siebie narysować.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }

        #endregion

        #region Metody Publiczne


        /// <summary>
        /// Metoda służy do poinformowania warstwy że ma się usunąć. 
        /// Nie usuwa jej od razu jak ScreenManager.RemoveScreen tylko 
        /// informuje, że efekt TransitionOff powinien wykonać się do końca
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                isExiting = true;
            }
        }


        #endregion

    }
}
