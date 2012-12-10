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
        
        public List<Entity> Entities;

        public Playground(KinectContext context) : base(context)
        {
            Player = new Player(context, SkeletonPlayerAssignment.FirstSkeleton);
            Entities = new List<Entity>();
            Entities.Add(Player);

            for (int i = 0; i < 5; i++)
            {
                Entities.Add(new DragableNumber(Player, i * 80, 20, i + 1));
            }
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