using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool.Entity {
    public class Clock : BaseEntity {

        public int StartValue { get; private set; }
        public float Value { get; set; }

        public bool Paused { get; set; }

        private float secondTimer = 1f;

        public Clock(int posX, int posY, int seconds) {
            X = posX;
            Y = posY;

            Size = new Point(110, 110);

            StartValue = seconds;
            Value = StartValue;

            Paused = false;

            collidable = false;
        }

        public override void Init() {
            
        }

        public void Switch() {
            Paused = !Paused;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            if (!Paused) {
                Value -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Value < 0) {
                    Value = 0;
                    Paused = true;
                }

                secondTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (secondTimer <= 0) {
                    secondTimer = 1f;

                    if (Value < 10) {
                        Assets.TimeShort.Play();
                    }                    
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {          
            for (int i = 0; i <= 360; i++) {
                if ((360 / (float)StartValue) * (Value - 1) >= i) {
                    var destRect = new Rectangle(this.X + 55, this.Y + 55, 1, 50);
                    spriteBatch.Draw(Assets.ClockFillSprite, destRect, null, Color.White, -MathHelper.ToRadians(i + 180), new Vector2(0, 0), SpriteEffects.None, 0);
                }                
            }            
            
            spriteBatch.Draw(Assets.ClockFrameSprite, BoundingBox, Color.White);

            foreach (var num in FontNumber.FromInteger((int)Value, X + 30, Y + 35, new Point(27, 40))) {
                num.Draw(spriteBatch);
            }
        }

        public override void Delete() {            
        }
    }
}
