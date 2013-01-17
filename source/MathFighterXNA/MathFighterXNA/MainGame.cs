using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using System.Threading;
using MathFighterXNA.Screens;

namespace MathFighterXNA
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KinectContext kinectContext;

        DebugComponent debugComponent;


        private readonly Rectangle viewPortRectangle;

        public static int Width = 640;
        public static int Height = (Width / 4) * 3;

        public GameScreen CurrentScreen;             

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this); 
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.viewPortRectangle = new Rectangle(0, 0, Width, Height);
        }

        protected override void Initialize()
        {           
            kinectContext = new KinectContext(graphics.GraphicsDevice);
            kinectContext.Initialize();

            CurrentScreen = new Playground(kinectContext);
            CurrentScreen.Init();

            debugComponent = new DebugComponent(this);
           
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Assets.LoadContent(Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);            
        }

        protected override void UnloadContent()
        {
            kinectContext.Sensor.Stop();
        }

        protected override void Update(GameTime gameTime)
        {           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            debugComponent.Update(gameTime);
            
            kinectContext.Update();

            CurrentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();            

            if (kinectContext.CurrentBitmap != null)
            {
                spriteBatch.Draw(kinectContext.CurrentBitmap, new Rectangle(0, 0, Width, Height), Color.White);
            }                       

            CurrentScreen.Draw(spriteBatch);


            debugComponent.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
