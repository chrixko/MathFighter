using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ClownSchool.Bang;
using Microsoft.Xna.Framework.Media;
using ClownSchool.Bang.Actions;

namespace ClownSchool.Screens {
    public class ScreenManager {
        private List<GameScreen> Screens = new List<GameScreen>();

        public ActionList Actions = new ActionList();

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

        public void FadeInSong(Song song, bool repeat) {
            Actions.AddAction(new FadeInSong(song, repeat), true);
        }

        public void RemoveScreen(GameScreen screen) {
            Screens.Remove(screen);
        }

        public void SwitchScreen(GameScreen screen) {
            Screens.Remove(Screens.Last());
            AddScreen(screen);
        }

        public void Update(GameTime gameTime) {
            Actions.Update(gameTime);
            Screens.Last().Update(gameTime);
        }

        public void Draw(ExtendedSpriteBatch spriteBatch) {
            foreach (var screen in Screens) {
                screen.Draw(spriteBatch);
            }
        }        
    }
}
