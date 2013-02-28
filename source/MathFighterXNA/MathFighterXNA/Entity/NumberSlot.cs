using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MathFighterXNA.Tweening;

namespace MathFighterXNA.Entity {

    public class NumberSlot : BaseEntity {

        //public Player UsableBy;
        public DragableNumber Number { get; set; }
        public bool Reassignable { get; set; }

        private Tweener tweener;

        public NumberSlot(int posX, int posY, bool reassignable) {
            Position = new Point(posX, posY);
            Size = new Point(32, 75);
            tweener = new Tweener(posY, posY + 20, 1f, MathFighterXNA.Tweening.Quadratic.EaseInOut);
            tweener.Ended += delegate() { tweener.Reverse(); };

            Reassignable = reassignable;

            CollisionType = "slot";
        }

        public override void Init() {            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            if (Number != null && Number.State == Number.MoveToSlotState) {
                return;
            } else {
                tweener.Update(gameTime);
                Y = (int)tweener.Position;       
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.NumberSlotSprite, BoundingBox, Color.White);
        }

        public override void Delete() {
            if (Number != null) {
                Screen.RemoveEntity(Number);            
            }
        }
    }
}