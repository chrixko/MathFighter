using MathFighterXNA.Tweening;
using Microsoft.Xna.Framework;

namespace MathFighterXNA.Entity.NumberState {

    public class DefaultState : INumberState {
        public DragableNumber Owner;

        private Tweener defaultMoveTweener;

        double hoverTime = 0;

        float fullDragTime = 2f;
        float dragTime = 0f;

        public DefaultState(DragableNumber owner) {
            Owner = owner;

            defaultMoveTweener = new Tweener(owner.Y, owner.Y + 10, 1f, MathFighterXNA.Tweening.Quadratic.EaseInOut);
            defaultMoveTweener.Ended += delegate() { defaultMoveTweener.Reverse(); };
        }

        void INumberState.OnHandCollide(PlayerHand hand) {
            //if (hand.Player == Owner.Owner && !hand.IsDragging) {
            //    var copy = new DragableNumber(hand.Player, Owner.X, Owner.Y, Owner.Value);
            //    hand.Screen.AddEntity(copy);

            //    copy.State = copy.DraggedState;

            //    copy.DraggedState.DraggedBy = hand;
            //    hand.IsDragging = true;
            //}
        }

        void INumberState.OnSlotCollide(NumberSlot slot) {
        }

        void INumberState.Update(Microsoft.Xna.Framework.GameTime gameTime) {
            defaultMoveTweener.Update(gameTime);
            Owner.Y = (int)defaultMoveTweener.Position;

            var hand = (PlayerHand)Owner.GetFirstCollidingEntity(Owner.X, Owner.Y, "hand");
            if (hand != null) {
                hoverTime += gameTime.ElapsedGameTime.TotalSeconds;
            } else {
                hoverTime = 0;
            }

            if (hoverTime > 0) {
                Owner.Font = Assets.DebugFont;
            } else {
                Owner.Font = Assets.SmallDebugFont;
            }

            if (hoverTime > .7f) {
                Owner.Color = Color.Green;
                var copy = new DragableNumber(hand.Player, Owner.X, Owner.Y, Owner.Value);
                hand.Screen.AddEntity(copy);

                copy.State = copy.DraggedState;

                copy.DraggedState.DraggedBy = hand;
                hand.IsDragging = true;
            }
        }

        void INumberState.Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            
        }
    }
}
