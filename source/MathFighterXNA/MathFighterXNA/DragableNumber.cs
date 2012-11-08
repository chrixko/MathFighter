using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace MathFighterXNA
{
    public class DragableNumber
    {
        public Point Position { get; set; }        
        public Player Owner { get; set; }

        public bool IsDragged { get; private set; }
        public JointType DraggedBy { get; private set; }

        public int Value { get; private set; }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(Position.X, Position.Y, 32, 32);
            }
        }

        public DragableNumber(Player owner, int posX, int posY, int value)
        {
            Owner = owner;
            Position = new Point(posX, posY);
            IsDragged = false;

            Value = value;
        }

        public void Drag(JointType hand)
        {
            IsDragged = true;
            DraggedBy = hand;
        }

        public void Update(GameTime gameTime)
        {
            if (Owner.IsReady)
            {
                if (!IsDragged)
                {
                    if (this.BoundingBox.Intersects(Owner.LeftHandBounds))
                    {
                        IsDragged = true;
                        DraggedBy = JointType.HandLeft;
                    }

                    if (this.BoundingBox.Intersects(Owner.RightHandBounds))
                    {
                        IsDragged = true;
                        DraggedBy = JointType.HandRight;
                    }
                }

                if (IsDragged)
                {
                    this.Position = Owner.GetHandBounds(DraggedBy).Location;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.NumberBackgroundSprite, BoundingBox, Color.White);            
        }
    }
}
