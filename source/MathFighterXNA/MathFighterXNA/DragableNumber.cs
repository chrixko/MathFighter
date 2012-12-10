using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace MathFighterXNA
{
    public class DragableNumber : Entity
    {        
        public Player Owner { get; set; }

        public bool IsDragged { get; private set; }
        public Entity DraggedBy { get; private set; }

        public int Value { get; private set; }

        public DragableNumber(Player owner, int posX, int posY, int value)
        {
            Owner = owner;
            Position = new Point(posX, posY);
            Size = new Point(32, 32);
            IsDragged = false;

            Value = value;
        }

        public void Drag(Entity hand)
        {
            IsDragged = true;
            DraggedBy = hand;
        }

        public override void Update(GameTime gameTime)
        {
            if (Owner.IsReady)
            {
                if (!IsDragged)
                {
                    if (this.BoundingBox.Intersects(Owner.LeftHand.BoundingBox))
                    {
                        IsDragged = true;
                        DraggedBy = Owner.LeftHand;
                    }

                    if (this.BoundingBox.Intersects(Owner.RightHand.BoundingBox))
                    {
                        IsDragged = true;
                        DraggedBy = Owner.RightHand;
                    }
                }

                if (IsDragged)
                {
                    this.Position = DraggedBy.BoundingBox.Location;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.NumberBackgroundSprite, BoundingBox, Color.White);            
        }
    }
}
