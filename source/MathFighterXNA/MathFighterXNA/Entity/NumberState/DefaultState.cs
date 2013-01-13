using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathFighterXNA.Tweening;

namespace MathFighterXNA.Entity.NumberState
{
    public class DefaultState : INumberState
    {
        public DragableNumber Owner;

        private Tweener defaultMoveTweener;

        public DefaultState(DragableNumber owner)
        {
            Owner = owner;

            defaultMoveTweener = new Tweener(owner.Y, owner.Y + 10, 1f, MathFighterXNA.Tweening.Quadratic.EaseInOut);
            defaultMoveTweener.Ended += delegate() { defaultMoveTweener.Reverse(); };
        }

        void INumberState.onHandCollide(PlayerHand hand)
        {
            if (hand.Player == Owner.Owner)
            {
                Owner.State = Owner.DraggedState;
                Owner.DraggedState.DraggedBy = hand;
            }
        }

        void INumberState.onSlotCollide(NumberSlot slot)
        {
            //throw new NotImplementedException();
        }

        void INumberState.Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            defaultMoveTweener.Update(gameTime);

            Owner.Y = (int)defaultMoveTweener.Position;
        }
    }
}
