using System;

namespace ClownSchool.Entity.NumberState {

    public class DraggedState : INumberState {
        
        public DragableNumber Owner;
        public PlayerHand DraggedBy;

        public DraggedState(DragableNumber owner) {
            Owner = owner;
        }

        void INumberState.OnHandCollide(PlayerHand hand) {            
        }

        void INumberState.OnSlotCollide(NumberSlot slot) {
            if (!slot.Reassignable && slot.Number != null) return;

            Owner.State = Owner.MoveToSlotState;

            if (slot.Number != null) Owner.Screen.RemoveEntity(slot.Number);
            
            Owner.MoveToSlotState.Slot = slot;
            DraggedBy.IsDragging = false;
        }

        void INumberState.Update(Microsoft.Xna.Framework.GameTime gameTime) {
            if (DraggedBy != null) {
                Owner.Position = DraggedBy.BoundingBox.Location;
            } else {
                throw new ArgumentException("DraggedState without Dragger!");
            }
        }

        void INumberState.Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            
        }
    }
}
