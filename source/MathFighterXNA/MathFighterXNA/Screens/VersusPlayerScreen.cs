using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Entity;

namespace MathFighterXNA.Screens {

    public class VersusPlayerScreen : GameScreen {
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }

        public Equation CurrentEquation { get; set; }

        double Timer = 60;



        public VersusPlayerScreen(KinectContext context) : base(context) {
        }

        public override void Init() {
            PlayerOne = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            PlayerTwo = new Player(Context, SkeletonPlayerAssignment.RightSkeleton);
            
            AddEntity(PlayerOne);
            AddEntity(PlayerTwo);                                  
        }

        public override void Update(GameTime gameTime) {
            //Dirty? Calling ToArray to make a copy of the entity collection preventing crashing when entities create other entities through an update call
            foreach (var ent in Entities.ToArray<BaseEntity>()) {
                ent.Update(gameTime);
            }

            //if (CurrentEquation.IsSolved()) {
            //    RemoveEntity(CurrentEquation);
            //    Timer += 3f;

            //    CurrentEquation = Equation.CreateWithRandomProduct(Player);
            //    AddEntity(CurrentEquation);                
            //}

            //Timer -= gameTime.ElapsedGameTime.TotalSeconds;
        }        

        public override void Draw(SpriteBatch spriteBatch) {
            foreach (var ent in Entities) {
                ent.Draw(spriteBatch);
            }

            //spriteBatch.DrawString(Assets.DebugFont, string.Concat(((int)Timer).ToString(), "s"), new Vector2(MainGame.Width / 2 - 20, 100), Color.Orange);
        }
    }
}