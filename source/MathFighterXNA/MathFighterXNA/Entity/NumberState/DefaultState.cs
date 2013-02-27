using MathFighterXNA.Tweening;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA.Entity.NumberState {

    public class DefaultState : INumberState {
        public DragableNumber Owner;

        private Tweener defaultMoveTweener;

        double maxHoverTime = .7;
        double hoverTime = 0;

        public DefaultState(DragableNumber owner) {
            Owner = owner;

            defaultMoveTweener = new Tweener(owner.Y, owner.Y + 10, 1f, MathFighterXNA.Tweening.Quadratic.EaseInOut);
            defaultMoveTweener.Ended += delegate() { defaultMoveTweener.Reverse(); };
        }

        void INumberState.OnHandCollide(PlayerHand hand) {
        }

        void INumberState.OnSlotCollide(NumberSlot slot) {
        }

        void INumberState.Update(Microsoft.Xna.Framework.GameTime gameTime) {           
            defaultMoveTweener.Update(gameTime);
            Owner.Y = (int)defaultMoveTweener.Position;

            //TODO: Maybe dirty, should use OnHandCollide somehow, because I query the colliding hand two times, once in number and then here
            var hand = (PlayerHand)Owner.GetFirstCollidingEntity(Owner.X, Owner.Y, "hand");

            if (hand != null && hand.Player == Owner.Owner && !hand.IsDragging) {
                hoverTime += gameTime.ElapsedGameTime.TotalSeconds;
            } else {
                hoverTime = 0;
            }

            if (hoverTime > maxHoverTime) {
                var copy = new DragableNumber(hand.Player, Owner.X, Owner.Y, Owner.Value);
                hand.Screen.AddEntity(copy);

                copy.State = copy.DraggedState;

                copy.DraggedState.DraggedBy = hand;
                hand.IsDragging = true;
            }
        }

        void INumberState.Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (hoverTime > 0 && hoverTime <= maxHoverTime) {
                for (int i = 0; i <= 360; i++) {
                    var destRect = new Rectangle(Owner.BoundingBox.Center.X - 2, Owner.BoundingBox.Center.Y + 6, 1, 14);

                    var asset = Assets.CirclePartFilled;
                    if ((360 / maxHoverTime) * hoverTime <= i) {
                        asset = Assets.CirclePartEmpty;
                    } 
                    
                    spriteBatch.Draw(asset, destRect, null, Color.White, MathHelper.ToRadians(i), new Vector2(0, 13), SpriteEffects.None, 0);                    
                }
            }
        }
    }
}
