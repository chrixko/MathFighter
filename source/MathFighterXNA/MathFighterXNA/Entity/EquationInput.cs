using System;
using MathFighterXNA.Screens;
using System.Collections.Generic;

namespace MathFighterXNA.Entity {
    public class EquationInput : BaseEntity {

        public GameScreen Screen { get; private set; }

        public Player Current { get; set; }
        public int Product;

        public bool IsReady {
            get {
                return false;
            }
        }

        public List<NumberSlot> Slots;

        public NumberSlot FirstEquationSlot;
        public NumberSlot SecondEquationSlot;

        public NumberSlot FirstProductSlot;
        public NumberSlot SecondProductSlot;

        public EquationInput(GameScreen screen, int posX, int posY) {
            Screen = screen;
            X = posX;
            Y = posY;
        }

        public override void Init() {
            FirstEquationSlot = new NumberSlot(X, Y);
            SecondEquationSlot = new NumberSlot(X + 30, Y);

            FirstProductSlot = new NumberSlot(X, Y + 30);
            SecondProductSlot = new NumberSlot(X + 30, Y + 30);

            Slots.Add(FirstEquationSlot);
            Slots.Add(SecondEquationSlot);

            Slots.Add(FirstProductSlot);
            Slots.Add(SecondProductSlot);

            foreach (NumberSlot slot in Slots) {
                Screen.AddEntity(slot);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            foreach (NumberSlot slot in Slots) {
                slot.Update(gameTime);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            foreach (NumberSlot slot in Slots) {
                slot.Draw(spriteBatch);
            }
        }

        public override void Delete() {
            foreach (NumberSlot slot in Slots) {
                Screen.RemoveEntity(slot);
            }
        }
    }
}
