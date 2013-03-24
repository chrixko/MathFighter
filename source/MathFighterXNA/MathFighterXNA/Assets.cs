using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ClownSchool {

    public static class Assets {

        public static Texture2D NumberBackgroundSprite { get; set; }
        public static Texture2D JointSprite { get; set; }        
        public static Texture2D NumberSlotSprite { get; set; }
        public static Texture2D BalloonSpritesheet { get; set; }
        public static Texture2D CactusSprite { get; set; }
        public static Texture2D EquationInputSprite { get; set; }
        public static Texture2D ClockFrameSprite { get; set; }
        public static Texture2D ClockFillSprite { get; set; }
        public static Texture2D FontNumberSpritesheet { get; set; }

        public static Texture2D BalloonBoom { get; set; }
        public static Texture2D RopeSection { get; set; }
        public static Texture2D RopeKnot { get; set; }

        public static Texture2D Indicator { get; set; }

        public static Texture2D InputTop { get; set; }
        public static Texture2D InputBottom { get; set; }

        public static Texture2D ScissorBottomRight { get; set; }
        public static Texture2D ScissorTopRight{ get; set; }
        public static Texture2D ScissorBottomLeft { get; set; }
        public static Texture2D ScissorTopLeft { get; set; }
        
        public static Texture2D CurtainTopLeft { get; set; }
        public static Texture2D CurtainTopRight { get; set; }
        public static Texture2D CurtainBottomLeft { get; set; }
        public static Texture2D CurtainBottomRight { get; set; }

        public static Texture2D CirclePartEmpty { get; set; }
        public static Texture2D CirclePartFilled { get; set; }

        public static Texture2D PauseBackground { get; set; }
        public static Texture2D TextPlayerLeft { get; set; }
        public static Texture2D TextPleaseComeBack { get; set; }
        public static Texture2D PlayerSilhouette { get; set; }
        
        public static SpriteFont DebugFont { get; set; }
        public static SpriteFont SmallDebugFont { get; set; }

        public static SoundEffect BalloonGrab { get; set; }
        public static SoundEffect BalloonDrop { get; set; }
        public static SoundEffect MenuChoose { get; set; }
        public static SoundEffect MenuOver { get; set; }
        public static SoundEffect BalloonPop { get; set; }
        public static SoundEffect AnswerCorrect { get; set; }
        public static SoundEffect TimeShort { get; set; }
        public static SoundEffect AnswerWrong { get; set; }
        public static SoundEffect ScissorsSnip { get; set; }
        
        public static void LoadContent(ContentManager content) {
            NumberSlotSprite = content.Load<Texture2D>("balloon_gray");
            BalloonSpritesheet = content.Load<Texture2D>("balloon_spritesheet_blue");
            CactusSprite = content.Load<Texture2D>("cactus");
            EquationInputSprite = content.Load<Texture2D>("equals");
            ClockFrameSprite = content.Load<Texture2D>("timer_out");
            ClockFillSprite = content.Load<Texture2D>("timer_in");
            FontNumberSpritesheet = content.Load<Texture2D>("font_numbers");

            BalloonBoom = content.Load<Texture2D>("balloon_boom");
            RopeSection = content.Load<Texture2D>("rope_section");
            RopeKnot = content.Load<Texture2D>("rope_knot");

            Indicator = content.Load<Texture2D>("indicator_green");

            InputTop = content.Load<Texture2D>("input_top");
            InputBottom = content.Load<Texture2D>("input_bottom");

            ScissorBottomRight = content.Load<Texture2D>("scissors_bot_right");
            ScissorTopRight = content.Load<Texture2D>("scissors_top_right");

            ScissorBottomLeft = content.Load<Texture2D>("scissors_bot_left");
            ScissorTopLeft = content.Load<Texture2D>("scissors_top_left");

            CurtainTopLeft = content.Load<Texture2D>("curtain_top_left");
            CurtainTopRight = content.Load<Texture2D>("curtain_top_right");
            CurtainBottomLeft = content.Load<Texture2D>("curtain_bot_left");
            CurtainBottomRight = content.Load<Texture2D>("curtain_bot_right");

            CirclePartEmpty = content.Load<Texture2D>("circle_part_empty");
            CirclePartFilled = content.Load<Texture2D>("circle_part_filled");

            PauseBackground = content.Load<Texture2D>("pause_background");
            TextPlayerLeft = content.Load<Texture2D>("text_left");
            TextPleaseComeBack = content.Load<Texture2D>("text_come_back");
            PlayerSilhouette = content.Load<Texture2D>("silhouette");
            
            DebugFont = content.Load<SpriteFont>("DebugFont");
            SmallDebugFont = content.Load<SpriteFont>("SmallDebugFont");

            BalloonGrab = content.Load<SoundEffect>("sounds/BalloonGrab"); //http://opengameart.org/content/battle-sound-effects
            BalloonDrop = content.Load<SoundEffect>("sounds/BalloonDrop"); //http://opengameart.org/content/battle-sound-effects
            MenuChoose = content.Load<SoundEffect>("sounds/MenuChoose"); //http://www.flashkit.com/soundfx/Interfaces/deep_pon-xrikazen-7422/index.php
            MenuOver = content.Load<SoundEffect>("sounds/MenuOver"); // http://www.flashkit.com/soundfx/Interfaces/cordless-xrikazen-7428/index.php
            BalloonPop = content.Load<SoundEffect>("sounds/BalloonPop"); //http://labs.petegoodman.com/ghetto_blaster/_includes/sfx/worms/WORMPOP.WAV
            AnswerCorrect = content.Load<SoundEffect>("sounds/AnswerCorrect"); //http://opengameart.org/content/completion-sound
            TimeShort = content.Load<SoundEffect>("sounds/TimeShort"); //http://www.flashkit.com/soundfx/Cartoon/Timer-GamePro9-8160/index.php
            AnswerWrong = content.Load<SoundEffect>("sounds/AnswerWrong"); //http://www.flashkit.com/soundfx/Cartoon/Slide_fl-Texavery-8987/index.php
            ScissorsSnip = content.Load<SoundEffect>("sounds/scissors_snip");
        }
    }
}