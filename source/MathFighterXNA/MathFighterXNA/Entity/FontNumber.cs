using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MathFighterXNA.Entity {
    public class FontNumber : BaseEntity {

        public int Value { get; set; }

        public FontNumber(int value, int posX, int posY, Point size) {
            X = posX;
            Y = posY;

            Value = value;

            Size = size;
        }

        public override void Init() {
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.FontNumberSpritesheet, BoundingBox, new Rectangle(Value * 87, 0, 87, 100), Color.White);
        }

        public static List<FontNumber> FromInteger(int number, int posX, int posY, Point size) {
            var numbers = new List<FontNumber>();

            var numString = number.ToString("00");

            for (int i = 0; i < numString.Length; i++) {
                numbers.Add(new FontNumber(int.Parse(numString[i].ToString()), posX + (i * size.X), posY, size));
            }

            return numbers;
        }

        public override void Delete() {
        }
    }
}
