using Microsoft.Xna.Framework;

namespace MathFighterXNA.Entity {

    public class NumberSlot : BaseEntity {

        public DragableNumber Number { get; set; }
        public EquationInput Owner;
               
        public bool Reassignable { get; set; }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public Point NumberPosition {
            get {
                return new Point(this.X, this.BoundingBox.Center.Y - 170);
            }
        }

        public NumberSlot(EquationInput owner, int offsetX, int offsetY, bool reassignable) {
            Owner = owner;

            OffsetX = offsetX;
            OffsetY = offsetY;

            Size = new Point(64, 64);

            Reassignable = reassignable;

            CollisionType = "slot";
        }

        public override void Init() {            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            X = Owner.X + OffsetX;
            Y = Owner.Y + OffsetY;            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            //spriteBatch.Draw(Assets.NumberSlotSprite, BoundingBox, Color.White);
        }

        public override void Delete() {
            if (Number != null) {
                Screen.RemoveEntity(Number);            
            }
        }
    }
}