﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ClownSchool.Entity.Menu {
    public class MenuButton : BaseEntity {

        public Texture2D Graphic { get; set; }
        public Action OnClick { get; set; }

        public Menu Menu { get; set; }

        private bool selected { get; set; }

        private float hoverTime = 0f;
        private float maxHoverTime = 2f;

        public MenuButton(Texture2D graphic, int posX, int posY, Action onClick) {
            Graphic = graphic;
            OnClick = onClick;

            X = posX;
            Y = posY;

            Size = new Point(130, 130);
        }

        public override void Init() {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
                      
            var _selected = selected;
            selected = GetFirstCollidingEntity("hand") != null;

            if (!_selected && selected) {
                Assets.MenuOver.Play();
            }

            if (selected) {
                hoverTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (hoverTime >= maxHoverTime) {
                    hoverTime = 0;
                    Assets.MenuChoose.Play();
                    
                    if (OnClick != null) {
                        OnClick();
                    }
                }
            } else {
                hoverTime = 0;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (selected) {
                spriteBatch.Draw(Assets.MenuButtonGlow, new Rectangle(X - 30, Y - 30, Size.X + 60, Size.Y + 60), Color.White);
            }
            
            spriteBatch.Draw(Graphic, new Rectangle(X, Y, Size.X, Size.Y), Color.White);

            if (hoverTime > 0 && hoverTime <= maxHoverTime) {
                PlayerHand hand = (PlayerHand)GetFirstCollidingEntity("hand");
                if (hand == null)
                    return;

                for (int i = 0; i <= 360; i++) {
                    var destRect = new Rectangle(hand.X - 50, hand.Y - 50, 1, 20);

                    var asset = Assets.CirclePartFilled;
                    if ((360 / maxHoverTime) * hoverTime <= i) {
                        asset = Assets.CirclePartEmpty;
                    }

                    spriteBatch.Draw(asset, destRect, null, Color.White, MathHelper.ToRadians(i), new Vector2(0, 20), SpriteEffects.None, 0);
                }
            }
        }

        public override void Delete() {

        }
    }
}
