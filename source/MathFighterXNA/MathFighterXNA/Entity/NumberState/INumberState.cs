using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MathFighterXNA.Entity.NumberState
{
    public interface INumberState
    {
        //void onPlayerDrag(Player player);
        void onHandCollide(PlayerHand hand);
        void onSlotCollide(NumberSlot slot);

        void Update(GameTime gameTime);
    }
}
