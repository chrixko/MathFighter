using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace MathFighterXNA.Entity
{
    public class NumberSlot : BaseEntity
    {
        public Player UsableBy;
        public DragableNumber Number;


        public NumberSlot(int posX, int posY)            
        {
            Position = new Point(posX, posY);
            Size = new Point(32, 32);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.NumberBackgroundSprite, BoundingBox, Color.Black);
        }
    }
}
