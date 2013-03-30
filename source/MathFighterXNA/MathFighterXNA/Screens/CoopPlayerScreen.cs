using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using ClownSchool.Bang.Actions;
using ClownSchool.Tweening;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;
using ClownSchool.Entity.Menu;

namespace ClownSchool.Screens {

    public class CoopPlayerScreen : GameScreen {

        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public int NeededPlayerCount = 2;


        public Clock MainClock { get; set; }        
        public int Score = 0;
        
        public EquationInput Input { get; set; }

        private Dictionary<DragableNumber, Vector2> Numbers { get; set; }

        private bool Ended = false;        

        public CoopPlayerScreen(KinectContext context)
            : base(context) {
        }

        public override void Init() {
            base.Init();

            Manager.FadeInSong(Assets.GameSong, true, 0.2f);

            AddCurtain();
            OpenCurtain();

            AddPlayers();

            MainClock = new Clock(20, 20, 90);
            MainClock.Paused = true;
            
            AddEntity(MainClock);            

            Coroutines.Start(LoadNumbersFromFile());

            AddInput();

            if (!Configuration.GRABBING_ENABLED) {
                AddEntity(new Scissors(120, MainGame.Height - 350, Scissors.ScissorPosition.Left));
                AddEntity(new Scissors(MainGame.Width - 120, MainGame.Height - 350, Scissors.ScissorPosition.Right));
            }            
        }

        public virtual void AddPlayers() {
            PlayerOne = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            PlayerTwo = new Player(Context, SkeletonPlayerAssignment.RightSkeleton);

            AddEntity(PlayerOne);
            AddEntity(PlayerTwo);
        }

        private IEnumerator LoadNumbersFromFile() {
            Numbers = new Dictionary<DragableNumber, Vector2>();

            var rand = new Random();

            var values = new List<int>();
            for (int i = 0; i < 11; i++) {
                values.Add(i);
            }

            using (StreamReader reader = new StreamReader(@"BalloonArrangements\CoopPlayerScreen.csv")) {

                while (!reader.EndOfStream) {
                    string[] data = reader.ReadLine().Split(';');
                    if (data.Length == 2) {
                        int posX = int.Parse(data[0]);
                        int posY = int.Parse(data[1]);

                        var value = values[rand.Next(0, values.Count)];
                        values.Remove(value);

                        var num = new DragableNumber(PlayerOne, posX, posY, value);
                        Numbers.Add(num, new Vector2(posX, posY));

                        num.ZDepth = -1;

                        yield return Pause(0.1f);
                        Assets.BalloonPlace.Play(0.5f, 0, 0);
                        AddEntity(num);

                        if (Numbers.Count > 1) {
                            var value2 = values[rand.Next(0, values.Count)];
                            values.Remove(value2);

                            int posX2 = MainGame.Width - posX - 62;
                            int posY2 = posY;

                            var num2 = new DragableNumber(PlayerOne, posX2, posY2, value2);
                            Numbers.Add(num2, new Vector2(posX2, posY2));

                            num2.ZDepth = -1;

                            yield return Pause(0.1f);
                            Assets.BalloonPlace.Play(0.5f, 0, 0);
                            AddEntity(num2);                            
                        }
                    }
                }
            }   
        }

        private static IEnumerator Pause(float time) {
            var watch = Stopwatch.StartNew();
            while (watch.Elapsed.TotalSeconds < time)
                yield return 0;
        }

        public void PauseClock() {
            MainClock.Paused = true;
        }

        public void ResumeCurrentClock() {
            MainClock.Paused = false;
        }

        public void ShuffleBalloons() {
            shuffleNumberPositions();

            foreach (var num in Numbers.Keys) {
                var posX = Numbers[num].X;
                var posY = Numbers[num].Y;

                //if (CurrentPlayer == PlayerTwo) {
                //    posX = MainGame.Width - Numbers[num].X - 62;
                //}

                var tweenTo = new Vector2(posX, posY);

                var that = num;

                num.State = num.IdleState;
                num.Actions.AddAction(new TweenPositionTo(num, tweenTo, 1.5f, Back.EaseInOut), true);
                num.Actions.AddAction(new CallFunction(delegate() { that.State = new ClownSchool.Entity.NumberState.DefaultState(that); }), true);

                num.Owner = PlayerOne;
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

            //Input.CurrentPlayer = CurrentPlayer; 

            var center = new Vector2((MainGame.Width / 2) - (337 / 2), 200);

            Input.Actions.AddAction(new CallFunction(delegate() { PauseClock(); }), true);

            Input.Actions.AddAction(new CallFunction(delegate() { ShuffleBalloons(); }), true);
            Input.Actions.AddAction(new TweenPositionTo(Input, center, 2f, Tweening.Back.EaseOut), true);
            
            Input.Actions.AddAction(new CallFunction(delegate() { ResumeCurrentClock(); }), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Product), true);
            Input.Actions.AddAction(new CallFunction(delegate() { PauseClock(); }), true);

            Input.Actions.AddAction(new EndEquationInput(Input), true);

            Input.Actions.AddAction(new CallFunction(delegate() {
                if (!Input.IsAnswerCorrect) {
                    MainClock.Value -= 5f;
                } else {
                    MainClock.Value += 5f;
                    Score += 5;
                }
                RemoveEntity(Input);
                AddInput();
            }), true);

            AddEntity(Input);

            AddRandomBalloons(Input);

            Input.FirstEquationSlot.Player = Input.SecondEquationSlot.Player = null;
            Input.FirstProductSlot.Player = PlayerOne;
            Input.SecondProductSlot.Player = PlayerTwo;
        }

        private void AddRandomBalloons(EquationInput input) {
            var rand = new Random();
            var ball1 = new Balloon(input.X, input.Y, rand.Next(0, 11));
            var ball2 = new Balloon(input.X, input.Y, rand.Next(0, 11));

            AddEntity(ball1);
            AddEntity(ball2);

            ball1.AttachTo(Input.FirstEquationSlot);
            Input.FirstEquationSlot.Balloon = ball1;

            ball2.AttachTo(Input.SecondEquationSlot);
            Input.SecondEquationSlot.Balloon = ball2;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (!PlayerOne.IsReady || !PlayerTwo.IsReady) {
                AddPauseScreen();
            }

            if (!Ended) {
                if (MainClock.Value <= 0f) {
                    EndGame();
                }
            }

        }

        public void EndGame() {
            Ended = true;

            Manager.FadeInSong(Assets.WinSong, false, 0.8f);

            Actions.AddAction(new EndEquationInput(Input), true);

            foreach (DragableNumber num in Entities.Where(ent => ent.CollisionType == "number")) {
                Actions.AddAction(new TweenPositionTo(num, new Vector2(1300, -200), 1f, Linear.EaseIn), false);
            }


            AttachWinnerBalloon(PlayerOne);
            AttachWinnerBalloon(PlayerTwo);                       

            var posMenu = new Vector2(300, (MainGame.Height / 2) - 250);
            var menu = new MenuItem(Assets.MenuSignMenu, -100, -300, delegate() { Manager.SwitchScreen(new MenuScreen(Context)); Manager.FadeInSong(Assets.MenuSong, true, 0.5f); });
            menu.Actions.AddAction(new TweenPositionTo(menu, posMenu, 2f, Back.EaseOut), true);
            AddEntity(menu);

            var posRestart = new Vector2(MainGame.Width - 600, (MainGame.Height / 2) - 250);

            var restart = new MenuItem(Assets.MenuSignRestart, MainGame.Width + 100, -300, delegate() { Manager.SwitchScreen(new CoopPlayerScreen(Context)); Manager.FadeInSong(Assets.GameSong, true, 0.2f); });
            restart.Actions.AddAction(new TweenPositionTo(restart, posRestart, 2f, Back.EaseOut), true);
            AddEntity(restart);
        }

        private void AttachWinnerBalloon(Player player) {
            for (int i = 0; i < 2; i++) {
                var hand = i < 1 ? player.LeftHand : player.RightHand;

                var balloon = new Balloon(100, 0, 11);
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
            pauseScreen.WaitForPlayerCount(NeededPlayerCount);

            Manager.AddScreen(pauseScreen);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            foreach (var num in FontNumber.FromInteger(Score, MainGame.Width - 130, 50, new Point(27, 40), "0000")) {
                num.Draw(spriteBatch);
            }
        }
    }
}