using MathFighterXNA.Entity;
using Microsoft.Xna.Framework;
using MathFighterXNA.Tweening;

namespace MathFighterXNA.Bang.Actions {
    public class EndEquationInput : IAction {

        public EquationInput Input { get; set; }        

        private bool isComplete { get; set; }

        public EndEquationInput(EquationInput input) {
            Input = input;
        }

        public bool IsBlocking() {
            return true;
        }

        public bool IsComplete() {
            return isComplete;
        }

        public void Block() {            
        }

        public void Unblock() {            
        }

        public void Update(GameTime gameTime) {
            if (Input.IsAnswerCorrect) {
                Input.Actions.AddAction(new TweenPositionTo(Input, new Vector2(Input.X, -300), 2f, Back.EaseOut), true);
            } else {
                Input.Actions.AddAction(new TweenPositionTo(Input, new Vector2(Input.X, 800), 2f, Back.EaseOut), true);
            }

            Complete();
        }

        public void Complete() {
            isComplete = true;
        }
    }
}
