#region Using
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Minifice.Enums;
using Minifice.GameScreens;
#endregion

namespace Minifice.MenuScreens
{
    class NewGameMenuScreen : MenuScreen
    {
        #region Pola

        #endregion

        #region Inicjalizacja

        public NewGameMenuScreen()
            : base("Wybór trudności")
        {
            MenuEntry easyMenuEntry;
            MenuEntry mediumMenuEntry;
            MenuEntry hardMenuEntry;
            MenuEntry back = new MenuEntry("Powrót");

            // Generowanie pozycji menu
            easyMenuEntry = new MenuEntry("Łatwy");
            mediumMenuEntry = new MenuEntry("Średni");
            hardMenuEntry = new MenuEntry("Trudny");

            // Eventy
            easyMenuEntry.Selected += EasyMenuEntrySelected;
            mediumMenuEntry.Selected += MediumMenuEntrySelected;
            hardMenuEntry.Selected += HardMenuEntrySelected;
            back.Selected += OnCancel;

            // Dodawanie pozycji do menu
            MenuEntries.Add(easyMenuEntry);
            MenuEntries.Add(mediumMenuEntry);
            MenuEntries.Add(hardMenuEntry);
            MenuEntries.Add(back);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler dla Easy Mode
        /// </summary>
        void EasyMenuEntrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(Difficulty.Easy));
        }

        /// <summary>
        /// Event handler dla Medium Mode
        /// </summary>
        void MediumMenuEntrySelected(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Event handler dla Hard Mode
        /// </summary>
        void HardMenuEntrySelected(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
