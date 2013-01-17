using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA {

    public class DebugComponent {

        private MainGame Game;

        private float fps = 0;
        private float updateInterval = 1.0f;        
        private float timeSinceLastDraw = 0.0f;                
        private float frameCount = 0;

        public DebugComponent(MainGame game) {
            Game = game;
        }

        public void Update(GameTime gameTime) {            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;
            timeSinceLastDraw += elapsed;
            if (timeSinceLastDraw > updateInterval) {
                fps = frameCount / timeSinceLastDraw;

                Game.Window.Title = "FPS: " + fps.ToString();

                frameCount = 0;
                timeSinceLastDraw -= updateInterval;
            }
            
            spriteBatch.DrawString(Assets.SmallDebugFont, "FPS: " + fps.ToString(), new Vector2(MainGame.Width - 100, 0), Color.Lime);
            spriteBatch.DrawString(Assets.SmallDebugFont, "Entities: " + Game.CurrentScreen.Entities.Count.ToString(), new Vector2(MainGame.Width - 100, 10), Color.Lime);
        }
    }
}
