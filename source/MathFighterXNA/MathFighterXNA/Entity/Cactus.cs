using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathFighterXNA.Entity {
    public class Cactus : BaseEntity {

        public float Rotation;

        public Cactus(int posX, int posY, float rotation) {
            X = posX;
            Y = posY;

            
        }

        public override void Init() {
            throw new NotImplementedException();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            
        }

        public override void Delete() {            
        }
    }
}
