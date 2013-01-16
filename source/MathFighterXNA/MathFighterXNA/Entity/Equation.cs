using System;
using Microsoft.Xna.Framework;
using MathFighterXNA.Entity;
using MathFighterXNA.Screens;

namespace MathFighterXNA.Entity {
    
    public class Equation : BaseEntity {
        public Player Solver { get; set; }
        public NumberSlot FirstSlot { get; set; }
        public NumberSlot SecondSlot { get; set; }
        public int Product { get; set; }

        public static Random Random = new Random();

        public Equation(int product, Player solver, GameScreen screen) {
            //TODO: Having to pass the GameScreen is horrible. After initialization entity object already nows which GameScreen it belongs to.. The same in Player.cs

            Product = product;
            Solver = solver;

            FirstSlot = new NumberSlot((MainGame.Width / 2) - 64, (MainGame.Height / 2) + 100);
            SecondSlot = new NumberSlot((MainGame.Width / 2) + 32, (MainGame.Height / 2) + 100);

            screen.AddEntity(FirstSlot);
            screen.AddEntity(SecondSlot);
        }

        public static Equation CreateWithRandomProduct(Player solver, GameScreen screen) {
            return new Equation(Random.Next(1, 101), solver, screen);        
        }

        public bool IsSolved() {
            if (FirstSlot.Number == null || SecondSlot.Number == null) {
                return false;
            }

            return (FirstSlot.Number.Value * SecondSlot.Number.Value) == Product;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (!IsSolved()) {
                spriteBatch.DrawString(Assets.DebugFont, "Product: " + Product.ToString(), new Vector2((MainGame.Width / 2) - 75, 50.0f), Color.Red);
            } else {
                spriteBatch.DrawString(Assets.DebugFont, "Solved!", new Vector2((MainGame.Width / 2) - 75, 50.0f), Color.Red);
            }
            
        }

        public override void Delete() {
            Screen.RemoveEntity(FirstSlot);
            Screen.RemoveEntity(SecondSlot);
        }
    }
}