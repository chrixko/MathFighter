using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ClownSchool.PhysicsBalloon {
    public class VerletStick {
        public VerletPoint PointA;
        public VerletPoint PointB;

        private float length;

        public VerletStick(VerletPoint pointA, VerletPoint pointB, float length) {
            PointA = pointA;
            PointB = pointB;

            if (length == -1) {
                var dx = pointA.X - pointB.X;
                var dy = pointA.X - pointB.Y;

                length = (float)Math.Sqrt(dx * dx + dy * dy);
            } else {
                this.length = length;
            }
        }

        public void Update(GameTime gameTime) {
            var dx = PointB.X - PointA.X;
            var dy = PointB.Y - PointA.Y;

            var dist = (float)Math.Sqrt(dx * dx + dy * dy);
            var diff = length - dist;

            var offsetX = (diff * dx / dist) / 2;
            var offsetY = (diff * dy / dist) / 2;

            PointA.X -= offsetX;
            PointA.Y -= offsetY;

            PointB.X += offsetX;
            PointB.Y += offsetY;
        }
    }
}
