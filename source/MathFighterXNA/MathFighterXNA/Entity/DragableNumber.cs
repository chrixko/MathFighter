using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Entity;
using MathFighterXNA.Entity.NumberState;

namespace MathFighterXNA {

    public class DragableNumber : BaseEntity {        

        public Player Owner { get; set; }
        public int Value { get; private set; }

        //States
        public INumberState State;

        public DefaultState DefaultState;
        public DraggedState DraggedState;
        public MoveToSlotState MoveToSlotState;
        public InSlotState InSlotState;

        public SpriteFont Font;
        public Color Color;        

        public DragableNumber(Player owner, int posX, int posY, int value) {
            Owner = owner;
            Position = new Point(posX, posY);
            Size = new Point(64, 150);            
            Value = value;

            CollisionType = "number";

            DefaultState = new DefaultState(this);
            DraggedState = new DraggedState(this);
            MoveToSlotState = new MoveToSlotState(this);
            InSlotState = new InSlotState(this);

            State = DefaultState;
        }

        public override void Init() {
            Font = Assets.DebugFont;
            Color = Color.White;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var hand = (PlayerHand)GetFirstCollidingEntity(X, Y, "hand");
            if (hand != null) {
                State.OnHandCollide(hand);
            }

            var slot = (NumberSlot)GetFirstCollidingEntity(X, Y, "slot");
            if (slot != null) {
                State.OnSlotCollide(slot);
            }

            State.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.NumberBackgroundSprite, BoundingBox, Color);
            spriteBatch.DrawString(Font, Value.ToString(), new Vector2(BoundingBox.Center.X -16, BoundingBox.Center.Y - 64), Color.Black);

            State.Draw(spriteBatch);            
        }

        public override void Delete() {
        }
    }
}