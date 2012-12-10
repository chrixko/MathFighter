using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA
{
    public abstract class Entity
    {
        public Point Position { get; set; }
        public Point Size { get; set; }
        public Point Offset { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(Position.X + Offset.X, Position.Y + Offset.Y, Size.X, Size.Y);
            }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
