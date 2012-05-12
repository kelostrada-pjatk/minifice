#region Using
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Minifice.ScreenManagement;
using Minifice.FileManagement;
#endregion

namespace Minifice.MenuScreens
{
    public static class MyExtensions
    {
        public static string ToString2(this Vector2 V)
        {
            return V.X + "x" + V.Y;
        }
    }

    class OptionsMenuScreen : MenuScreen
    {
        #region Pola

        List<Vector2> Resolutions = new List<Vector2>();

        int currentResolution = 0;

        #endregion

        #region Inicjalizacja


        /// <summary>
        /// Konstruktor
        /// </summary>
        public OptionsMenuScreen(ScreenManager screenManager)
            : base("Opcje")
        {
            // Inicjalizacje
            MenuEntry resolutionMenuEntry;
            MenuEntry fullScreenMenuEntry;
            MenuEntry soundEffectsMenuEntry;
            MenuEntry backgroundMusicMenuEntry;

            MenuEntry back = new MenuEntry("Powrót");

            this.ScreenManager = screenManager;
            Resolutions.Add(new Vector2(800, 600));
            Resolutions.Add(new Vector2(1024, 768));
            Resolutions.Add(new Vector2(1280, 720));
            Resolutions.Add(new Vector2(1280, 1024));
            Resolutions.Add(new Vector2(1680, 1050));
            Resolutions.Add(new Vector2(1920, 1080));
            currentResolution = Resolutions.LastIndexOf(ScreenManager.Settings.Resolution);

            // Generowanie pozycji menu
            resolutionMenuEntry = new MenuEntry(string.Empty);
            fullScreenMenuEntry = new MenuEntry(string.Empty);
            soundEffectsMenuEntry = new MenuEntry(string.Empty);
            backgroundMusicMenuEntry = new MenuEntry(string.Empty);

            // Eventy
            resolutionMenuEntry.Selected += ResolutionMenuEntrySelected;
            fullScreenMenuEntry.Selected += FullScreenMenuEntrySelected;
            soundEffectsMenuEntry.Selected += SoundEffectsMenuEntrySelected;
            backgroundMusicMenuEntry.Selected += BackgroundMusicMenuEntrySelected;
            back.Selected += OnCancel;

            // Dodawanie pozycji do menu
            MenuEntries.Add(resolutionMenuEntry);
            MenuEntries.Add(fullScreenMenuEntry);
            MenuEntries.Add(soundEffectsMenuEntry);
            MenuEntries.Add(backgroundMusicMenuEntry);
            MenuEntries.Add(back);
            SetMenuEntryText();
        }

        /// <summary>
        /// Aktualizacja tekstu w menu
        /// </summary>
        void SetMenuEntryText()
        {
            MenuEntries[0].Text = "Rozdzielczość: " + Resolutions[currentResolution].ToString2();
            MenuEntries[1].Text = "Pełny ekran: " + ScreenManager.Settings.FullScreen.ToString();
            MenuEntries[2].Text = "Efekty dźwiękowe: " + ScreenManager.Settings.SoundEffects.ToString();
            MenuEntries[3].Text = "Muzyka w tle: " + ScreenManager.Settings.BackgroundMusic.ToString();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler dla zmiany ustawienia rozdzielczości
        /// </summary>
        void ResolutionMenuEntrySelected(object sender, EventArgs e)
        {
            Settings pom = ScreenManager.Settings;
            currentResolution = (currentResolution + 1) % Resolutions.Count;
            
            ScreenManager.Settings = new Settings(Resolutions[currentResolution], pom.SoundEffects, pom.BackgroundMusic, pom.FullScreen);
            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler dla zmiany ustawienia pelnego ekranu
        /// </summary>
        void FullScreenMenuEntrySelected(object sender, EventArgs e)
        {
            Settings pom = ScreenManager.Settings;
            ScreenManager.Settings = new Settings(pom.Resolution, pom.SoundEffects, pom.BackgroundMusic, !pom.FullScreen);
            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler dla zmiany ustawienia efektów dzwiękowych
        /// </summary>
        void SoundEffectsMenuEntrySelected(object sender, EventArgs e)
        {
            Settings pom = ScreenManager.Settings;
            ScreenManager.Settings = new Settings(pom.Resolution, !pom.SoundEffects, pom.BackgroundMusic, pom.FullScreen);
            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler dla zmiany ustawienia muzyki w tle
        /// </summary>
        void BackgroundMusicMenuEntrySelected(object sender, EventArgs e)
        {
            Settings pom = ScreenManager.Settings;
            ScreenManager.Settings = new Settings(pom.Resolution, pom.SoundEffects, !pom.BackgroundMusic, pom.FullScreen);
            SetMenuEntryText();
        }

        #endregion
    }
}
