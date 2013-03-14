using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA.PhysicsBalloon {
    public class Balloon {
        private List<VerletPoint> Points;
        private List<VerletStick> Sticks;

        private float X;
        private float Y;

        private float rotationTarget;

        private static int stepCount = 20;
        private static int stepCountFact = 1 / stepCount;

        public Balloon() {
            rotationTarget = 0f;

            Points = new List<VerletPoint>();
            for (int i = 0; i < 30; i++) {
                Points.Add(new VerletPoint(i % 2 == 0 ? 400 : 405, 500 - (i * 5)));
            }

            Sticks = new List<VerletStick>();
            for (int i = 0; i < Points.Count - 1; i++) {
                Sticks.Add(new VerletStick(Points[i], Points[i + 1], -1));
            }

            Y = 500;
        }

        public void Update(GameTime gameTime) {
            foreach (VerletStick stick in Sticks) {
                stick.PointA.Y += .4f;
                stick.PointA.Update(gameTime);

                stick.PointB.Update(gameTime);
                stick.Update(gameTime);
            }

            Sticks.Last().PointB.Y += .4f;
            if (Y > 80) {
                Sticks.Last().PointB.Y -= 10;
            } else {
                Sticks.Last().PointB.Y -= 12;
            }

            var loops = 11;

            do {
                for (int i = 0; i < Sticks.Count; i++) {
                    var stick = Sticks[i];
                    stick.Update(gameTime);
                }
            } while (--loops > 0);

            X = Points[Points.Count - 3].X;
            Y = Points[Points.Count - 3].Y;
        }

        public void Draw(SpriteBatch spriteBatch) {
            var dx = X - Points[Points.Count - 10].X;
            var dy = Y - Points[Points.Count - 10].Y;

            var dist = Math.Sqrt(dx * dx + dy * dy);

            spriteBatch.Draw(Assets.NumberSlotSprite, new Rectangle((int)X, (int)Y, 50, 50), null, Color.White, (float)Math.Asin(dx / dist) * .5f, new Vector2(0, 0), SpriteEffects.None, 0f);

            var n = Points.Count - 2;

            var prevPoint = new Point();
            var nextPoint = new Point();
            var startPoint = new Point();
            var endPoint = new Point();

            for (int i = 2; i < n; i++) {
                if (i - 1 >= 0) {
                    prevPoint.X = (int)Points[i - 1].X;
                    prevPoint.Y = (int)Points[i - 1].Y;
                }

                startPoint.X = (int)Points[i].X;
                startPoint.Y = (int)Points[i].Y;

                if (i + 1 < Points.Count) {
                    endPoint.X = (int)Points[i + 1].X;
                    endPoint.Y = (int)Points[i + 1].Y;
                }
                if (i + 2 < Points.Count) {
                    nextPoint.X = (int)Points[i + 2].X;
                    nextPoint.Y = (int)Points[i + 2].Y;
                }

                for (int j = 0; j <= stepCount; j++) {
                    var xloc = getCardinalSplinePoint(prevPoint.X, startPoint.X, endPoint.X, nextPoint.X, stepCountFact, j, .5f);
                    var yloc = getCardinalSplinePoint(prevPoint.Y, startPoint.Y, endPoint.Y, nextPoint.Y, stepCountFact, j, .5f);

                    spriteBatch.Draw(Assets.NumberSlotSprite, new Rectangle((int)xloc, (int)yloc, 5, 5), Color.White);
                }
            }
        }

        float getCardinalSplinePoint(float prevVal, float startVal, float endVal, float nextVal, float numSteps, float curStep, float tension) {
            var t1 = (endVal - prevVal)*tension; 
            var t2 = (nextVal - startVal)*tension;						
            var s = curStep * numSteps; 
            var h1 = (2 * Math.Pow(s,3)) - (3 * Math.Pow(s,2)) + 1; 
            var h2 = -(2 * Math.Pow(s,3)) + (3 * Math.Pow(s,2)); 
            var h3 = Math.Pow(s,3) - (2 * Math.Pow(s,2)) + s; 
            var h4 = Math.Pow(s,3) - Math.Pow(s,2);			
            var value = (h1 * startVal) + (h2 * endVal) + (h3 * t1) + (h4 * t2);			
            return (float)value; 
        }
    }
}
