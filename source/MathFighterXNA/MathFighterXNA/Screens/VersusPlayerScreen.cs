using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using ClownSchool.Bang.Actions;
using ClownSchool.Tweening;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System;
using System.Linq;

namespace ClownSchool.Screens {

    public class VersusPlayerScreen : GameScreen {

        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player CurrentPlayer { get; set; }

        public Clock PlayerOneClock { get; set; }
        public Clock PlayerTwoClock { get; set; }        

        public EquationInput Input { get; set; }

        private Dictionary<DragableNumber, Vector2> Numbers { get; set; }

        private bool Ended = false;

        public VersusPlayerScreen(KinectContext context) : base(context) {
        }

        public override void Init() {
            AddCurtain();
            OpenCurtain();

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

            AddEntity(new Scissors(120, MainGame.Height - 350, Scissors.ScissorPosition.Left));
            AddEntity(new Scissors(MainGame.Width - 120, MainGame.Height - 350, Scissors.ScissorPosition.Right));

            base.Init();
        }

        private void LoadNumbersFromFile() {
            Numbers = new Dictionary<DragableNumber, Vector2>();

            var rand = new Random();

            var values = new List<int>();
            for (int i = 0; i < 11; i++) {
                values.Add(i);
            }

            using (StreamReader reader = new StreamReader(@"BalloonArrangements\VersusPlayerScreen.csv")) {

                while (!reader.EndOfStream) {
                    string[] data = reader.ReadLine().Split(';');
                    if (data.Length == 2) {
                        int posX = int.Parse(data[0]);
                        int posY = int.Parse(data[1]);

                        var value = values[rand.Next(0, values.Count)];
                        values.Remove(value);

                        var num = new DragableNumber(CurrentPlayer, posX, posY, value);
                        Numbers.Add(num, new Vector2(posX, posY));

                        num.ZDepth = -1;

                        AddEntity(num);
                    }
                }
            } 
        }

        public void PauseClocks() {
            PlayerOneClock.Paused = true;
            PlayerTwoClock.Paused = true;
        }

        public void ResumeCurrentClock() {
            if (CurrentPlayer == PlayerOne) {
                PlayerOneClock.Paused = false;
            } else {
                PlayerTwoClock.Paused = false;
            }            
        }

        public void SwitchCurrentPlayer() {
            if (CurrentPlayer == PlayerOne) {
                CurrentPlayer = PlayerTwo;
            } else {
                CurrentPlayer = PlayerOne;
            }

            shuffleNumberPositions();

            foreach (var num in Numbers.Keys) {
                var posX = Numbers[num].X;
                var posY = Numbers[num].Y;

                if (CurrentPlayer == PlayerTwo) {
                    posX = MainGame.Width - Numbers[num].X - 62;
                }

                var tweenTo = new Vector2(posX, posY);

                var that = num;

                num.State = num.IdleState;
                num.Actions.AddAction(new TweenPositionTo(num, tweenTo, 1.5f, Back.EaseInOut), true);
                num.Actions.AddAction(new CallFunction(delegate() { that.State = new ClownSchool.Entity.NumberState.DefaultState(that); }), true);

                num.Owner = CurrentPlayer;
            }
        }

        private void shuffleNumberPositions() {            
            var posList = new List<Vector2>(Numbers.Values.ToArray());

            var rand = new Random();

            foreach (var key in Numbers.Keys.ToArray()) {
                var randVector = posList[rand.Next(0, posList.Count)];
                posList.Remove(randVector);

                Numbers[key] = randVector;                
            }
        }

        private void AddInput() {
            Input = new EquationInput((MainGame.Width / 2) - 337 / 2, MainGame.Height);

            Input.CurrentPlayer = CurrentPlayer;

            var left = new Vector2(Input.X - 100, 300);
            var right = new Vector2(Input.X + 80, 300);

            Input.Actions.AddAction(new CallFunction(delegate() { PauseClocks(); }), true);
            Input.Actions.AddAction(new TweenPositionTo(Input, CurrentPlayer == PlayerOne ? left : right, 2f, Tweening.Back.EaseOut), true);
            Input.Actions.AddAction(new CallFunction(delegate() { ResumeCurrentClock(); }), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Equation), true);
            Input.Actions.AddAction(new CallFunction(delegate() { PauseClocks(); }), true);
            Input.Actions.AddAction(new CallFunction(delegate() { SwitchCurrentPlayer(); }), true);

            Input.Actions.AddAction(new TweenPositionTo(Input, CurrentPlayer == PlayerOne ? right : left, 2f, Tweening.Back.EaseOut), true);
            Input.Actions.AddAction(new CallFunction(delegate() { ResumeCurrentClock(); }), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Product), true);
            Input.Actions.AddAction(new CallFunction(delegate() { PauseClocks(); }), true);

            Input.Actions.AddAction(new EndEquationInput(Input), true);

            Input.Actions.AddAction(new CallFunction(delegate() {
                if (!Input.IsAnswerCorrect) {
                    if (CurrentPlayer == PlayerOne) {
                        PlayerOneClock.Value -= 5f;
                    } else {
                        PlayerTwoClock.Value -= 5f;
                    }
                }
                RemoveEntity(Input);
                AddInput();
            }), true);

            AddEntity(Input);

            Input.FirstEquationSlot.Player = Input.SecondEquationSlot.Player = CurrentPlayer == PlayerOne ? PlayerOne : PlayerTwo;
            Input.FirstProductSlot.Player = Input.SecondProductSlot.Player = CurrentPlayer == PlayerOne ? PlayerTwo : PlayerOne;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (!PlayerOne.IsReady) {
                AddPauseScreen();
            }

            if (!Ended) {
                if (PlayerOneClock.Value == 0) {
                    EndGame(PlayerTwo);
                } else if (PlayerTwoClock.Value == 0) {
                    EndGame(PlayerOne);
                }
            }

        }

        public void EndGame(Player winner) {
            Ended = true;

            Actions.AddAction(new EndEquationInput(Input), true);

            foreach (DragableNumber num in Entities.Where(ent => ent.CollisionType == "number")) {
                Actions.AddAction(new TweenPositionTo(num, new Vector2(1300, -200), 1f, Back.EaseIn), false);
            }

            for (int i = 0; i < 4; i++) {               
                var hand = i < 2 ? winner.LeftHand : winner.RightHand;

                var balloon = new Balloon(100 * i, 0, 11);
                AddEntity(balloon);

                balloon.AttachTo(hand);
            }
        }

        public void AddPauseScreen() {
            var pauseScreen = new PauseScreen(Context);
            var sil1 = new SimpleGraphic(Assets.PlayerSilhouette, 250, 75, 480, 588);
            var sil2 = new SimpleGraphic(Assets.PlayerSilhouette, 650, 75, 480, 588);
            pauseScreen.AddEntity(sil1);
            pauseScreen.AddEntity(sil2);
            pauseScreen.WaitForPlayerCount(1);

            Manager.AddScreen(pauseScreen);

        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }
    }
}