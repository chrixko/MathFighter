using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using MathFighterXNA.Tweening;


namespace MathFighterXNA
{
    public class DragableNumber : BaseEntity
    {        
        public Player Owner { get; set; }

        public bool IsDragged { get; private set; }
        public BaseEntity DraggedBy { get; private set; }

        public int Value { get; private set; }

        private Tweener tweener;

        public DragableNumber(Player owner, int posX, int posY, int value)
        {
            Owner = owner;
            Position = new Point(posX, posY);
            Size = new Point(32, 32);
            IsDragged = false;

            tweener = new Tweener(posY, posY + 20, 1f, MathFighterXNA.Tweening.Quadratic.EaseInOut);
            tweener.Ended += delegate() { tweener.Reverse(); };
            Value = value;
        }

        public void Drag(BaseEntity hand)
        {
            IsDragged = true;
            DraggedBy = hand;
        }

        public override void Update(GameTime gameTime)
        {           
            //if (Owner.IsReady)
            //{
                if (!IsDragged)
                {
                    tweener.Update(gameTime);
                    Y = (int)tweener.Position;

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
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.NumberBackgroundSprite, BoundingBox, Color.White);
            spriteBatch.DrawString(Assets.DebugFont, Value.ToString(), new Vector2(BoundingBox.Center.X -8, BoundingBox.Center.Y - 16), Color.Black);
        }
    }
}
