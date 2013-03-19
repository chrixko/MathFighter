using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using ClownSchool.Entity.NumberState;

namespace ClownSchool {

    public class DragableNumber : BaseEntity {        

        public Player Owner { get; set; }
        public int Value { get; private set; }

        //States
        public INumberState State;

        public DefaultState DefaultState;
        public DraggedState DraggedState;
        public MoveToSlotState MoveToSlotState;
        public InSlotState InSlotState;

        public DragableNumber(Player owner, int posX, int posY, int value) {
            Owner = owner;
            Position = new Point(posX, posY);
            Size = new Point(54, 64);
            Offset = new Point(5, 5);

            Value = value;

            CollisionType = "number";

            DefaultState = new DefaultState(this);
            DraggedState = new DraggedState(this);
            MoveToSlotState = new MoveToSlotState(this);
            InSlotState = new InSlotState(this);

            State = DefaultState;
        }

        public override void Init() {

        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var hand = (PlayerHand)GetFirstCollidingEntity(X, Y, "hand");
            if (hand != null) {
                State.OnHandCollide(hand);
            }

            //var slot = (NumberSlot)GetFirstCollidingEntity(X, Y, "slot");
            //if (slot != null) {
            //    State.OnSlotCollide(slot);
            //}

            State.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.BalloonSpritesheet, new Rectangle(X, Y, 62, 170), new Rectangle(62 * (Value - 1), 0, 62, 170), new Color(255, 255, 255, 255));

            State.Draw(spriteBatch);            
        }

        public override void Delete() {
            if (DraggedState.DraggedBy != null)
                DraggedState.DraggedBy.IsDragging = false;
        }
    }
}