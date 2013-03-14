using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MathFighterXNA {

    public static class Assets {

        public static Texture2D NumberBackgroundSprite { get; set; }
        public static Texture2D JointSprite { get; set; }        
        public static Texture2D NumberSlotSprite { get; set; }
        public static Texture2D BalloonSpritesheet { get; set; }

        public static Texture2D CurtainTopLeft { get; set; }
        public static Texture2D CurtainTopRight { get; set; }


        public static Texture2D CirclePartEmpty { get; set; }
        public static Texture2D CirclePartFilled { get; set; }
        
        public static SpriteFont DebugFont { get; set; }
        public static SpriteFont SmallDebugFont { get; set; }

        

        public static void LoadContent(ContentManager content) {
            NumberBackgroundSprite = content.Load<Texture2D>("balloon_red");
            JointSprite = content.Load<Texture2D>("Joint");
            NumberSlotSprite = content.Load<Texture2D>("balloon_gray");
            BalloonSpritesheet = content.Load<Texture2D>("balloon_spritesheet");

            CurtainTopLeft = content.Load<Texture2D>("curtain_top_left");
            CurtainTopRight = content.Load<Texture2D>("curtain_top_right");

            CirclePartEmpty = content.Load<Texture2D>("circle_part_empty");
            CirclePartFilled = content.Load<Texture2D>("circle_part_filled");
            
            DebugFont = content.Load<SpriteFont>("DebugFont");
            SmallDebugFont = content.Load<SpriteFont>("SmallDebugFont");

            
        }
    }
}