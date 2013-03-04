using System;
using MathFighterXNA.Screens;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MathFighterXNA.Tweening;

namespace MathFighterXNA.Entity {

    public class EquationInput : BaseEntity {
       
        public Player Current { get; set; }

        public List<NumberSlot> Slots;

        public NumberSlot FirstEquationSlot;
        public NumberSlot SecondEquationSlot;

        public NumberSlot FirstProductSlot;
        public NumberSlot SecondProductSlot;

        public bool IsEquationSet {
            get {
                return (FirstEquationSlot.Number != null && SecondEquationSlot.Number != null);
            }
        }

        public bool IsAnswerSet {
            get {
                return (FirstProductSlot.Number != null && SecondProductSlot.Number != null);
            }
        }

        public bool IsAnswerCorrect {
            get {
                if(!IsEquationSet || !IsAnswerSet) return false;

                return (FirstEquationSlot.Number.Value * SecondEquationSlot.Number.Value) == Convert.ToInt32(FirstProductSlot.Number.Value.ToString() + SecondProductSlot.Number.Value.ToString());
            }
        }

        public EquationInput(int posX, int posY) {
            X = posX;
            Y = posY;
           
            Slots = new List<NumberSlot>();
        }

        public override void Init() {
            FirstEquationSlot = new NumberSlot(this, 0, 0, false);
            SecondEquationSlot = new NumberSlot(this, 50, 0, false);

            FirstProductSlot = new NumberSlot(this, 0, 70, true);
            SecondProductSlot = new NumberSlot(this, 50, 70, true);

            Slots.Add(FirstEquationSlot);
            Slots.Add(SecondEquationSlot);

            Slots.Add(FirstProductSlot);
            Slots.Add(SecondProductSlot);

            foreach (NumberSlot slot in Slots) {
                Screen.AddEntity(slot);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);
           
            foreach (NumberSlot slot in Slots) {
                slot.Update(gameTime);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            foreach (NumberSlot slot in Slots) {
                slot.Draw(spriteBatch);
            }

            if (IsAnswerCorrect) {
                spriteBatch.DrawString(Assets.DebugFont, "SOLVED!", new Vector2(this.X, this.Y - 30), Color.Red);
            }
        }

        public override void Delete() {
            foreach (NumberSlot slot in Slots) {
                Screen.RemoveEntity(slot);
            }
        }
    }
}
