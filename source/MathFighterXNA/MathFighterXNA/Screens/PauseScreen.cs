using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ClownSchool.Bang.Actions;
using ClownSchool.Entity;

namespace ClownSchool.Screens {
    public class PauseScreen : GameScreen {

        public PauseState State;

        public PauseScreen(KinectContext context): base(context) {
            State = PauseState.Default;
        }

        public void WaitForPlayerCount(int count) {
            State = PauseState.WaitingForPlayers;
            Actions.AddAction(new WaitForPlayerCount(count, Context), true);
            Actions.AddAction(new CallFunction(delegate() { State = PauseState.Countdown; }), true);
        }
    
        private float countDownTimer = 6;
        private float secondTimer = 0;
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            switch (State) {
                case PauseState.Countdown:
                    countDownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    secondTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (secondTimer <= 0) {
                        Assets.TimeShort.Play();
                        secondTimer = 1f;
                    }

                    if(countDownTimer <= 0) {
                        Manager.RemoveScreen(this);
                    }

                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Assets.PauseBackground, new Rectangle(0, 0, MainGame.Width, MainGame.Height), new Color(255, 255, 255, 200));

            switch (State) {
                case PauseState.WaitingForPlayers:
                    spriteBatch.Draw(Assets.TextPlayerLeft, new Rectangle((MainGame.Width / 2), 50, 717, 83), null, Color.White, 0, new Vector2(717 / 2, 83 / 2), SpriteEffects.None, 0);
                    spriteBatch.Draw(Assets.TextPleaseComeBack, new Rectangle((MainGame.Width / 2), MainGame.Height - 100, 814, 58), null, Color.White, 0, new Vector2(814 / 2, 58 / 2), SpriteEffects.None, 0);

                    break;
                case PauseState.Countdown:
                    var countdown = new FontNumber((int)countDownTimer, (MainGame.Width / 2) - 50, (MainGame.Height / 2) - 50, new Point(100, 100), FontNumber.FontNumberColor.Red);
                    countdown.Draw(spriteBatch);

                    break;
                default:
                    break;
            }
        }

        public enum PauseReason {
            PlayerLeft,
            PauseOnPurpose
        }

        public enum PauseState {
            Default,
            WaitingForPlayers,
            Countdown
        }
    }
}
