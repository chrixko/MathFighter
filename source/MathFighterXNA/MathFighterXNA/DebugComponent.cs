﻿using System.Linq;
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
            
            spriteBatch.DrawString(Assets.SmallDebugFont, "FPS: " + fps.ToString(), new Vector2(MainGame.Width - 150, 0), Color.Lime);
            spriteBatch.DrawString(Assets.SmallDebugFont, "Entities: " + Game.CurrentScreen.Entities.Count.ToString(), new Vector2(MainGame.Width - 150, 10), Color.Lime);
            spriteBatch.DrawString(Assets.SmallDebugFont, "Skeletons: " + Game.CurrentScreen.Context.Skeletons.Count, new Vector2(MainGame.Width - 150, 20), Color.Lime);
            spriteBatch.DrawString(Assets.SmallDebugFont, "LeftSkeleton: " + (Game.CurrentScreen.Context.GetLeftSkeleton() != null).ToString(), new Vector2(MainGame.Width - 150, 30), Color.Lime);
            spriteBatch.DrawString(Assets.SmallDebugFont, "RightSkeleton: " + (Game.CurrentScreen.Context.GetRightSkeleton() != null).ToString(), new Vector2(MainGame.Width - 150, 40), Color.Lime);

            foreach (var ent in Game.CurrentScreen.Entities.Where(ent => ent.collidable)) {
                (spriteBatch as ExtendedSpriteBatch).DrawRectangle(ent.BoundingBox, Color.Red);
            }
        }
    }
}
