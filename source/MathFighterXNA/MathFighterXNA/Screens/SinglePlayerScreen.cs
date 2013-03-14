﻿using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Entity;
using System.Collections.Generic;
using System;


namespace MathFighterXNA.Screens {

    public class SinglePlayerScreen : GameScreen {
        public Player Player { get; set; }
        public Equation CurrentEquation { get; set; }

        double Timer = 60;

        public SinglePlayerScreen(KinectContext context) : base(context) {            
        }

        public override void Init() {
            base.Init();

            Player = new Player(Context, SkeletonPlayerAssignment.FirstSkeleton);
            AddEntity(Player);

            //CurrentEquation = Equation.CreateWithRandomProduct(Player);

            //AddEntity(CurrentEquation);

            for (int i = 1; i <= 10; i++)
            {

                double dy = System.Math.Pow((60 * i - 30)-300, 2) * 0.002 +15;
                AddEntity(new DragableNumber(Player, System.Convert.ToInt32((60 * i)-30), System.Convert.ToInt32(dy), i));
            }                       

        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            //if (CurrentEquation.IsSolved()) {
            //    RemoveEntity(CurrentEquation);
            //    Timer += 3f;

            //    CurrentEquation = Equation.CreateWithRandomProduct(Player);
            //    AddEntity(CurrentEquation);                
            //}

            Timer -= gameTime.ElapsedGameTime.TotalSeconds;

        }        

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Assets.DebugFont, string.Concat(((int)Timer).ToString(), "s"), new Vector2(MainGame.Width / 2 - 20, 100), Color.Orange);
        }
    }
}