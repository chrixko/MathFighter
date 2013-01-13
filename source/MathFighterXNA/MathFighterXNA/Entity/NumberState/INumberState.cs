using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MathFighterXNA.Entity.NumberState
{
    public interface INumberState
    {
        void OnHandCollide(PlayerHand hand);
        void OnSlotCollide(NumberSlot slot);
        void Update(GameTime gameTime);
    }
}
