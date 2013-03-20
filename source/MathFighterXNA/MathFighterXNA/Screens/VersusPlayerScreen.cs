using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using ClownSchool.Bang.Actions;
using ClownSchool.Tweening;
using System.IO;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System;

namespace ClownSchool.Screens {

    public class VersusPlayerScreen : GameScreen {

        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player CurrentPlayer { get; set; }

        public Clock PlayerOneClock { get; set; }
        public Clock PlayerTwoClock { get; set; }        

        public EquationInput Input { get; set; }

        private Dictionary<DragableNumber, Vector2> Numbers { get; set; }

        public VersusPlayerScreen(KinectContext context) : base(context) {
        }

        public override void Init() {
            base.Init();

            PlayerOne = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            PlayerTwo = new Player(Context, SkeletonPlayerAssignment.RightSkeleton);

            CurrentPlayer = PlayerOne;

            AddEntity(PlayerOne);
            AddEntity(PlayerTwo);

            PlayerOneClock = new Clock(20, 20, 90);
            PlayerTwoClock = new Clock(MainGame.Width - 130, 20, 90);

            PlayerTwoClock.Paused = true;

            AddEntity(PlayerOneClock);
            AddEntity(PlayerTwoClock);

            LoadNumbersFromFile();

            AddInput();

            var cactusOne = new Cactus(80, MainGame.Height - 250, 0f);
            var cactusTwo = new Cactus(MainGame.Width - 230, MainGame.Height - 250, -0f);

            AddEntity(cactusOne);
            AddEntity(cactusTwo);           
        }

        private void LoadNumbersFromFile() {
            Numbers = new Dictionary<DragableNumber, Vector2>();

            var rand = new Random();

            using (StreamReader reader = new StreamReader(@"BalloonArrangements\VersusPlayerScreen.csv")) {

                while(!reader.EndOfStream) {
                    string[] data = reader.ReadLine().Split(';');
                    if(data.Length == 2) {
                        int posX = int.Parse(data[0]);
                        int posY = int.Parse(data[1]);

                        var num = new DragableNumber(CurrentPlayer, posX, posY, rand.Next(1, 11));
                        Numbers.Add(num, new Vector2(posX, posY));

                        num.ZDepth = -1;

                        AddEntity(num);   
                    }                
                }
            } 
        }

        public void SwitchCurrentPlayer() {
            if (CurrentPlayer == PlayerOne) {
                CurrentPlayer = PlayerTwo;
            } else {
                CurrentPlayer = PlayerOne;
            }

            PlayerOneClock.Switch();
            PlayerTwoClock.Switch();

            foreach (var num in Numbers.Keys) {
                var posX = Numbers[num].X;

                if (CurrentPlayer == PlayerTwo) {
                    posX = MainGame.Width - Numbers[num].X - 62;
                }

                var tweenTo = new Vector2(posX, num.Y);
                num.Actions.AddAction(new TweenPositionTo(num, tweenTo, 1.5f, Back.EaseInOut), true);

                num.Owner = CurrentPlayer;
            }
        }

        private void AddInput() {
            Input = new EquationInput((MainGame.Width / 2) - 337 / 2, MainGame.Height);

            var left = new Vector2(Input.X - 100, 300);
            var right = new Vector2(Input.X + 80, 300);
            
            Input.Actions.AddAction(new TweenPositionTo(Input, CurrentPlayer == PlayerOne ? left : right, 2f, Tweening.Back.EaseOut), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Equation), true);
            Input.Actions.AddAction(new CallFunction(delegate() { SwitchCurrentPlayer(); }), true);

            Input.Actions.AddAction(new TweenPositionTo(Input, CurrentPlayer == PlayerOne ? right : left, 2f, Tweening.Back.EaseOut), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Product), true);

            Input.Actions.AddAction(new EndEquationInput(Input), true);

            Input.Actions.AddAction(new CallFunction(delegate() { 
                RemoveEntity(Input); 
                AddInput(); 
            }), true);

            AddEntity(Input);

            Input.FirstEquationSlot.Player = Input.SecondEquationSlot.Player = CurrentPlayer == PlayerOne ? PlayerOne : PlayerTwo;
            Input.FirstProductSlot.Player = Input.SecondProductSlot.Player = CurrentPlayer == PlayerOne ? PlayerTwo : PlayerOne;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);            
        }        

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }
    }
}