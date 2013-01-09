using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Screens;

namespace MathFighterXNA
{
    public abstract class BaseEntity
    {
        public Point Position 
        {
            get
            {
                return new Point(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public int X { get; set; }
        public int Y { get; set; }

        public Point Size { get; set; }
        public Point Offset { get; set; }

        public GameScreen Screen { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(X + Offset.X, Y + Offset.Y, Size.X, Size.Y);
            }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
