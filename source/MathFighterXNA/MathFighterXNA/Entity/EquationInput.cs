using System;
using ClownSchool.Screens;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ClownSchool.Tweening;

namespace ClownSchool.Entity {

    public class EquationInput : BaseEntity {
       
        public Player Current { get; set; }

        public List<NumberSlot> Slots;

        public NumberSlot FirstEquationSlot;
        public NumberSlot SecondEquationSlot;

        public NumberSlot FirstProductSlot;
        public NumberSlot SecondProductSlot;

        public bool IsEquationSet {
            get {
                return (FirstEquationSlot.Balloon != null && SecondEquationSlot.Balloon != null);
            }
        }

        public bool IsAnswerSet {
            get {
                return (FirstProductSlot.Balloon != null && SecondProductSlot.Balloon != null);
            }
        }

        public bool IsAnswerCorrect {
            get {
                if(!IsEquationSet || !IsAnswerSet) return false;

                return (FirstEquationSlot.Balloon.Number * SecondEquationSlot.Balloon.Number) == Convert.ToInt32(FirstProductSlot.Balloon.Number.ToString() + SecondProductSlot.Balloon.Number.ToString());
            }
        }

        public EquationInput(int posX, int posY) {
            X = posX;
            Y = posY;

            Size = new Point(337, 300);
            collidable = false;

            Slots = new List<NumberSlot>();
        }

        public override void Init() {
            FirstEquationSlot = new NumberSlot(this, 50, 80, false);
            SecondEquationSlot = new NumberSlot(this, 250, 80, false);

            FirstProductSlot = new NumberSlot(this, 50, 234, true);
            SecondProductSlot = new NumberSlot(this, 250, 234, true);

            Slots.Add(FirstEquationSlot);
            Slots.Add(SecondEquationSlot);

            Slots.Add(FirstProductSlot);
            Slots.Add(SecondProductSlot);

            foreach (NumberSlot slot in Slots) {
                Screen.AddEntity(slot);
            }
        }

        public void PopBalloons() {
            FirstEquationSlot.Balloon.Pop();
            SecondEquationSlot.Balloon.Pop();
            FirstProductSlot.Balloon.Pop();
            SecondProductSlot.Balloon.Pop();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);
           
            foreach (NumberSlot slot in Slots) {
                slot.Update(gameTime);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.EquationInputSprite, BoundingBox, Color.White);

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
