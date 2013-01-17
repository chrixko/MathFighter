using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MathFighterXNA {

    public static class Assets {

        public static Texture2D NumberBackgroundSprite { get; set; }
        public static Texture2D JointSprite { get; set; }        
        public static Texture2D NumberSlotSprite { get; set; }

        public static SpriteFont DebugFont { get; set; }
        public static SpriteFont SmallDebugFont { get; set; }

        public static void LoadContent(ContentManager content) {
            NumberBackgroundSprite = content.Load<Texture2D>("number_background");
            JointSprite = content.Load<Texture2D>("Joint");
            NumberSlotSprite = content.Load<Texture2D>("number_slot");

            DebugFont = content.Load<SpriteFont>("DebugFont");
            SmallDebugFont = content.Load<SpriteFont>("SmallDebugFont");            
        }
    }
}