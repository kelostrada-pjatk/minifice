using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Minifice.FileManagement
{
    public class Settings
    {
        Vector2 resolution;
        bool soundEffects;
        bool backgroundMusic;
        bool fullScreen;

        public Vector2 Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        public bool SoundEffects
        {
            get { return soundEffects; }
            set { soundEffects = value; }
        }

        public bool BackgroundMusic
        {
            get { return backgroundMusic; }
            set { backgroundMusic = value; }
        }

        public bool FullScreen
        {
            get { return fullScreen; }
            set { fullScreen = value; }
        }

        public Settings()
        {
            resolution = new Vector2();
            soundEffects = true;
            backgroundMusic = true;
            fullScreen = false;
        }

        public Settings(Vector2 resolution, bool soundEffects, bool backgroundMusic, bool fullScreen)
        {
            this.resolution = resolution;
            this.soundEffects = soundEffects;
            this.backgroundMusic = backgroundMusic;
            this.fullScreen = fullScreen;
        }
    }
}
