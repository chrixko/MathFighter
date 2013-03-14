using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MathFighterXNA.PhysicsBalloon {
    public class VerletPoint {
        public float X;
        public float Y;
        public float AncX;
        public float AncY;
        public bool Anchored;

        private float oldX;
        private float oldY;
        private static float SPEED_FACT = .92f;

        public Vector2 Velocity {
            get {
                return new Vector2((X - oldX) * SPEED_FACT, (Y - oldY) * SPEED_FACT);
            }
        }

        public VerletPoint(float x, float y) {
            Anchored = false;
            AncX = 0;
            AncY = 0;

            SetPosition(x, y);
        }

        public void Anchor(float x, float y) {
            Anchored = true;
            AncX = x;
            AncY = y;
        }

        public void Constrain(float left, float right, float top, float bottom) {
            X = Math.Max(left, Math.Min(right, X));
            Y = Math.Max(top, Math.Min(bottom, Y));
        }

        public void SetPosition(float x, float y) {
            X = oldX = x;
            Y = oldY = y;
        }

        public void Update(GameTime gameTime) {
            var _x = X;
            var _y = Y;

            if (!Anchored) {
                X += Velocity.X;
                Y += Velocity.Y;
            }

            oldX = _x;
            oldY = _y;
        }
    }
}
