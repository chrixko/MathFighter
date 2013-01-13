using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathFighterXNA.Entity.NumberState
{
    public class DraggedState : INumberState
    {
        public DragableNumber Owner;
        public PlayerHand DraggedBy;

        public DraggedState(DragableNumber owner)
        {
            Owner = owner;
        }

        void INumberState.OnHandCollide(PlayerHand hand)
        {            
        }

        void INumberState.OnSlotCollide(NumberSlot slot)
        {
            Owner.State = Owner.MoveToSlotState;
            slot.Number = this.Owner;
            Owner.MoveToSlotState.Slot = slot;
        }

        void INumberState.Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (DraggedBy != null)
            {
                Owner.Position = DraggedBy.BoundingBox.Location;
            }
            else
            {
                throw new ArgumentException("DraggedState without Dragger!");
            }
        }
    }
}
