using System;
using Microsoft.Xna.Framework;
using MathFighterXNA.Entity;
using MathFighterXNA.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA.Entity {
    
    public class Equation : BaseEntity {
        public Player Solver { get; set; }
        public NumberSlot FirstSlot { get; set; }
        public NumberSlot SecondSlot { get; set; }
        public int Product { get; set; }

        public static Random Random = new Random();

        public Equation(int product, Player solver) {
            Product = product;
            Solver = solver;

            //FirstSlot = new NumberSlot((MainGame.Width / 2) - 64, (MainGame.Height / 2) + 100, true);
            //SecondSlot = new NumberSlot((MainGame.Width / 2) + 32, (MainGame.Height / 2) + 100, true);
        }

        public static Equation CreateWithRandomProduct(Player solver) {
            return new Equation(Random.Next(1, 11) * Random.Next(1, 11), solver);        
        }

        public bool IsSolved() {
            if (FirstSlot.Number == null || SecondSlot.Number == null) {
                return false;
            }

            return (FirstSlot.Number.Value * SecondSlot.Number.Value) == Product;
        }

        public override void Init() {
            Screen.AddEntity(FirstSlot);
            Screen.AddEntity(SecondSlot);
        }

        public override void Update(GameTime gameTime) {
            FirstSlot.Update(gameTime);
            SecondSlot.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!IsSolved()) {
                spriteBatch.DrawString(Assets.DebugFont, "Product: " + Product.ToString(), new Vector2((MainGame.Width / 2) - 75, (MainGame.Height / 2) + 50), Color.Red);
            } else {
                spriteBatch.DrawString(Assets.DebugFont, "Solved!", new Vector2((MainGame.Width / 2) - 75, (MainGame.Height / 2) + 50), Color.Red);
            }

            FirstSlot.Draw(spriteBatch);
            SecondSlot.Draw(spriteBatch);
        }

        public override void Delete() {
            Screen.RemoveEntity(FirstSlot);
            Screen.RemoveEntity(SecondSlot);
        }
    }
}