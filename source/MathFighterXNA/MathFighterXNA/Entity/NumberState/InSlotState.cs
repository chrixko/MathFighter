namespace MathFighterXNA.Entity.NumberState {

    public class InSlotState : INumberState {

        public DragableNumber Owner;
        public NumberSlot Slot;

        public InSlotState(DragableNumber owner) {
            Owner = owner;
        }

        void INumberState.OnHandCollide(PlayerHand hand) {            
        }

        void INumberState.OnSlotCollide(NumberSlot slot) {            
        }

        void INumberState.Update(Microsoft.Xna.Framework.GameTime gameTime) {
            if (Slot != null) {
                Owner.Position = Slot.NumberPosition;
            }
        }

        void INumberState.Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            
        }
    }
}