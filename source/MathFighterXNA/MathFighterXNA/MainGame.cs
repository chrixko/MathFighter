using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ClownSchool.Screens;
using ClownSchool.Physics;

namespace ClownSchool {

    public class MainGame : Microsoft.Xna.Framework.Game {

        GraphicsDeviceManager graphics;
        ExtendedSpriteBatch spriteBatch;
        KinectContext kinectContext;

        DebugComponent debugComponent;

        private readonly Rectangle viewPortRectangle;

        public static int Width = 1324;
        public static int Height = 768;

        public static int KinectWidth = Width - 300;
        public static int KinectHeight = Height;

        public static float KinectScaleX = 640f / KinectWidth;
        public static float KinectScaleY = 480f / KinectHeight;

        public static int KinectOffsetX = 150;
        public static int KinectOffsetY = 0;

        public ScreenManager ScreenManager;

        public bool DebugView = false;

        Matrix spriteScale;

        public MainGame() {
            graphics = new GraphicsDeviceManager(this); 
            Content.RootDirectory = "Content";
                       
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            this.graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
            this.viewPortRectangle = new Rectangle(0, 0, Width, Height);
        }

        protected override void Initialize() {
            Assets.LoadContent(Content);
            kinectContext = new KinectContext(graphics.GraphicsDevice);
            kinectContext.Initialize();

            ScreenManager = new ScreenManager();
            ScreenManager.AddScreen(new MenuScreen(kinectContext));

            debugComponent = new DebugComponent(this);

            base.Initialize();
        }

        protected override void LoadContent() {           
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);

            float scaleX = graphics.GraphicsDevice.Viewport.Width / 1324f;
            float scaleY = graphics.GraphicsDevice.Viewport.Height / 768f;

            spriteScale = Matrix.CreateScale(scaleX, scaleY, 1);
        }

        protected override void UnloadContent() {
            kinectContext.StopSensor();
        }

        protected override void Update(GameTime gameTime) {           
            debugComponent.Update(gameTime);
            
            kinectContext.Update();

            ScreenManager.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F12)) {
                DebugView = !DebugView;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale);

            //TODO move kinectContext into the corresponding screens
            if (kinectContext.CurrentBitmap != null) {
                spriteBatch.Draw(kinectContext.CurrentBitmap, new Rectangle(KinectOffsetX, KinectOffsetY, KinectWidth, KinectHeight), Color.White);
            }

            ScreenManager.Draw(spriteBatch);

            if (DebugView) {
                debugComponent.Draw(spriteBatch, gameTime);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
