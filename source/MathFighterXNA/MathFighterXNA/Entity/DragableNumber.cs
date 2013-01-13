using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using MathFighterXNA.Tweening;
using MathFighterXNA.Entity;
using MathFighterXNA.Entity.NumberState;

namespace MathFighterXNA
{
    public class DragableNumber : BaseEntity
    {        
        public Player Owner { get; set; }

        public int Value { get; private set; }

        private Tweener snapToSlotTweenerX;
        private Tweener snapToSlotTweenerY;

        //States
        public INumberState State;

        public DefaultState DefaultState;
        public DraggedState DraggedState;
        public MoveToSlotState MoveToSlotState;
        public InSlotState InSlotState;

        public DragableNumber(Player owner, int posX, int posY, int value)
        {
            Owner = owner;
            Position = new Point(posX, posY);
            Size = new Point(32, 32);
            
            Value = value;

            CollisionType = "number";

            DefaultState = new DefaultState(this);
            DraggedState = new DraggedState(this);
            MoveToSlotState = new MoveToSlotState(this);
            InSlotState = new InSlotState(this);

            State = DefaultState;
        }

        public override void Update(GameTime gameTime)
        {
            var hand = (PlayerHand)GetFirstCollidingEntity(X, Y, "hand");
            if (hand != null)
            {
                State.onHandCollide(hand);
            }

            var slot = (NumberSlot)GetFirstCollidingEntity(X, Y, "slot");
            if (slot != null)
            {
                State.onSlotCollide(slot);
            }

            State.Update(gameTime);
            //if (snapToSlotTweenerX != null && snapToSlotTweenerY != null)
            //{
            //    snapToSlotTweenerX.Update(gameTime);
            //    snapToSlotTweenerY.Update(gameTime);

            //    if (snapToSlotTweenerX != null && snapToSlotTweenerY != null)
            //    {
            //        X = (int)snapToSlotTweenerX.Position;
            //        Y = (int)snapToSlotTweenerY.Position;
            //    }

            //    return;
            //}

            //if (!IsDragged)
            //{
            //    defaultMoveTweener.Update(gameTime);
            //    Y = (int)defaultMoveTweener.Position;

            //    PlayerHand hand = (PlayerHand)GetFirstCollidingEntity(X, Y, "hand");

            //    if (hand != null && hand.Player == Owner)
            //    {
            //        IsDragged = true;
            //        DraggedBy = hand;
            //    }
            //}

            //if (IsDragged)
            //{
            //    this.Position = DraggedBy.BoundingBox.Location;

            //    NumberSlot slot = (NumberSlot)GetFirstCollidingEntity(X, Y, "slot");
            //    if (slot != null && slot.Number == null)
            //    {
            //        IsDragged = false;
            //        DraggedBy = null;

            //        slot.Number = this;

            //        setSnapTweenerTo(slot);
            //    }
            //}
        }

        private void setSnapTweenerTo(NumberSlot slot)
        {
            snapToSlotTweenerX = new Tweener(X, slot.X, 1, MathFighterXNA.Tweening.Elastic.EaseOut);
            snapToSlotTweenerY = new Tweener(Y, slot.Y, 1, MathFighterXNA.Tweening.Elastic.EaseOut);

            snapToSlotTweenerX.Ended += delegate() { snapToSlotTweenerX = null; };
            snapToSlotTweenerY.Ended += delegate() { snapToSlotTweenerY = null; };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.NumberBackgroundSprite, BoundingBox, Color.White);
            spriteBatch.DrawString(Assets.DebugFont, Value.ToString(), new Vector2(BoundingBox.Center.X -8, BoundingBox.Center.Y - 16), Color.Black);
        }
    }
}
