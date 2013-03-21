using Microsoft.Xna.Framework;
using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool.Entity {

    public class NumberSlot : BaseEntity {

        public Balloon Balloon { get; private set; }
        public EquationInput Owner { get; set; }
        public Player Player { get; set; }
               
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

            Size = new Point(44, 44);

            Reassignable = reassignable;

            CollisionType = "slot";
        }

        public override void Init() {            
        }

        public bool TryAttach(Balloon balloon) {
            PlayerHand hand = (PlayerHand)balloon.AttachedEntity;

            if ((Reassignable || Balloon == null) && (Player == null || hand.Player == Player)) {
                balloon.AttachTo(this);
                Balloon = balloon;

                hand.DraggingBalloon = null;

                Assets.BalloonDrop.Play();

                return true;
            }

            return false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            X = Owner.X + OffsetX;
            Y = Owner.Y + OffsetY;            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (Balloon != null) {
                spriteBatch.Draw(Assets.RopeKnot, new Rectangle(BoundingBox.Center.X, BoundingBox.Center.Y, 11, 12), null, Color.White, 0, new Vector2(5.5f, 6f), SpriteEffects.None, 0);
            } else {
                if (Player.IsDragging) {
                    spriteBatch.Draw(Assets.Indicator, new Rectangle(BoundingBox.Center.X, BoundingBox.Center.Y, 50, 50), null, new Color(255,255,255, 100), 0, new Vector2(25, 25), SpriteEffects.None, 0);
                }
            }
        }

        public override void Delete() {
            if (Balloon != null) {
                Screen.RemoveEntity(Balloon);            
            }
        }
    }
}