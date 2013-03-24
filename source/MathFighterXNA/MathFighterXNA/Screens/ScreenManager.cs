using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ClownSchool.Screens {
    public class ScreenManager {
        private List<GameScreen> Screens = new List<GameScreen>();

        public GameScreen TopScreen {
            get {
                return Screens.Last();
            }
        }

        public ScreenManager() {

        }

        public void AddScreen(GameScreen screen) {
            Screens.Add(screen);
            screen.Manager = this;
            
            if (!screen.Inited)
                screen.Init();            
        }

        public void RemoveScreen(GameScreen screen) {
            Screens.Remove(screen);
        }

        public void Update(GameTime gameTime) {
            Screens.Last().Update(gameTime);
        }

        public void Draw(ExtendedSpriteBatch spriteBatch) {
            foreach (var screen in Screens) {
                screen.Draw(spriteBatch);
            }
        }        
    }
}
