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

        public GameScreen CurrentScreen;

        public MainGame() {
            graphics = new GraphicsDeviceManager(this); 
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = true;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
            this.viewPortRectangle = new Rectangle(0, 0, Width, Height);
        }

        protected override void Initialize() {
            Assets.LoadContent(Content);
            kinectContext = new KinectContext(graphics.GraphicsDevice);
            kinectContext.Initialize();

            CurrentScreen = new VersusPlayerScreen(kinectContext);
            CurrentScreen.Init();

            debugComponent = new DebugComponent(this);

            base.Initialize();
        }

        protected override void LoadContent() {           
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);            
        }

        protected override void UnloadContent() {
            kinectContext.Sensor.Stop();
        }

        protected override void Update(GameTime gameTime) {           
            debugComponent.Update(gameTime);
            
            kinectContext.Update();

            CurrentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();

            //TODO move kinectContext into the corresponding screens
            if (kinectContext.CurrentBitmap != null) {
                spriteBatch.Draw(kinectContext.CurrentBitmap, new Rectangle(KinectOffsetX, KinectOffsetY, KinectWidth, KinectHeight), Color.White);
            }

            CurrentScreen.Draw(spriteBatch);

            //debugComponent.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
