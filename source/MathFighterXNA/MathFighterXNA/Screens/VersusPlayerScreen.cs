using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Entity;

namespace MathFighterXNA.Screens {

    public class VersusPlayerScreen : GameScreen {
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }

        public EquationInput Input { get; set; }

        public VersusPlayerScreen(KinectContext context) : base(context) {
        }

        public override void Init() {
            PlayerOne = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            PlayerTwo = new Player(Context, SkeletonPlayerAssignment.RightSkeleton);

            Input = new EquationInput(300, 200);

            AddEntity(PlayerOne);
            AddEntity(PlayerTwo);

            AddEntity(Input);

            for (int i = 1; i <= 10; i++) {
                double dy = System.Math.Pow((60 * i - 30) - 300, 2) * 0.002 + 15;
                AddEntity(new DragableNumber(PlayerOne, System.Convert.ToInt32((60 * i) - 30), System.Convert.ToInt32(dy), i));
            }      
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