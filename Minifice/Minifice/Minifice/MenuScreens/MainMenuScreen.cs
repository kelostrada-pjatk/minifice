#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace Minifice.MenuScreens
{
    /// <summary>
    /// Główne Menu
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Inicjalizacja

        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainMenuScreen()
            : base("Menu Główne")
        {
            MenuEntry NewGameMenuEntry = new MenuEntry("Nowa Gra");
            MenuEntry Top5MenuEntry = new MenuEntry("TOP 5");
            MenuEntry OptionsMenuEntry = new MenuEntry("Opcje");
            MenuEntry ExitMenuEntry = new MenuEntry("Wyjście");

            NewGameMenuEntry.Selected += NewGameMenuEntrySelected;
            Top5MenuEntry.Selected += Top5MenuEntrySelected;
            OptionsMenuEntry.Selected += OptionsMenuEntrySelected;
            ExitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(NewGameMenuEntry);
            MenuEntries.Add(Top5MenuEntry);
            MenuEntries.Add(OptionsMenuEntry);
            MenuEntries.Add(ExitMenuEntry);
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler dla otwierania warsty wyboru poziomu trudności
        /// </summary>
        void NewGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new NewGameMenuScreen());
        }

        /// <summary>
        /// Event handler dla otwierania warsty Top 5
        /// </summary>
        void Top5MenuEntrySelected(object sender, EventArgs e)
        {
            //ScreenManager.AddScreen(new Top5MenuScreen(ScreenManager));
        }


        /// <summary>
        /// Event handler dla otwierania menu opcji
        /// </summary>
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(ScreenManager));
        }


        /// <summary>
        /// Kiedy gracz wybiera opcję zamknięcia gry zapytanie czy na pewno chce wyjść
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Czy na pewno chcesz wyjść?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }


        /// <summary>
        /// Event handler dla sytuacji gdy gracz potwiedzi wyjście z gry
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
