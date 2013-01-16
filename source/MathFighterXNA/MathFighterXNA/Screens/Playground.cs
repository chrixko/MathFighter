using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Entity;

namespace MathFighterXNA.Screens
{
    public class Playground : GameScreen //Single Player Screen for playing / testing
    {
        public Player Player { get; set; }
        public Equation CurrentEquation { get; set; }
        
        public Playground(KinectContext context) : base(context)
        {
        }

        public override void Init()
        {
            Player = new Player(Context, SkeletonPlayerAssignment.FirstSkeleton, this);
            AddEntity(Player);

            AddEntity(new Equation(25, Player, this));
            
            for (int i = 1; i <= 10; i++) {
                AddEntity(new DragableNumber(Player, (60 * i) - 30, 20, i));
            }                       
        }

        public override void Update(GameTime gameTime) {
            //Dirty? Calling ToArray to make a copy of the entity collection preventing crashing when entities create other entities through an update call
            foreach (var ent in Entities.ToArray<BaseEntity>()) {
                ent.Update(gameTime);
            }                
        }        

        public override void Draw(SpriteBatch spriteBatch) {
            foreach (var ent in Entities) {
                ent.Draw(spriteBatch);
            }                
        }
    }
}