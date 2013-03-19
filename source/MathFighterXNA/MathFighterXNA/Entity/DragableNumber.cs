using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using ClownSchool.Entity.NumberState;

namespace ClownSchool {

    public class DragableNumber : BaseEntity {        

        public Player Owner { get; set; }
        public int Number { get; private set; }

        //States
        public INumberState State;

        public DefaultState DefaultState;

        public DragableNumber(Player owner, int posX, int posY, int number) {
            Owner = owner;
            Position = new Point(posX, posY);
            Size = new Point(54, 64);
            Offset = new Point(5, 5);

            Number = number;

            CollisionType = "number";

            DefaultState = new DefaultState(this);

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

            State.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.BalloonSpritesheet, new Rectangle(X, Y, 62, 170), new Rectangle(62 * (Number - 1), 0, 62, 170), new Color(255, 255, 255, 255));

            State.Draw(spriteBatch);            
        }

        public override void Delete() {
        }
    }
}