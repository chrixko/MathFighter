using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA.Entity.NumberState {

    public interface INumberState {
        void OnHandCollide(PlayerHand hand);
        void OnSlotCollide(NumberSlot slot);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}