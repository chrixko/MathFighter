using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Kinect;

namespace MathFighterXNA.Screens
{
    public class Playground : GameScreen //Single Player Screen for playing / testing
    {
        public Player Player { get; set; }
        
        public Playground(KinectContext context) : base(context)
        {
            Player = new Player(context, SkeletonPlayerAssignment.FirstSkeleton);
            Entities.Add(Player);
            
            Entities.Add(new DragableNumber(Player, 20, 20, 5));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var ent in Entities)
                ent.Update(gameTime);
        }        

        public override void Draw(SpriteBatch spriteBatch)
        {            
            foreach (var ent in Entities)
                ent.Draw(spriteBatch);
        }
    }
}