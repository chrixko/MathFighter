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
        
        public List<DragableNumber> Numbers;

        public Playground(KinectContext context) : base(context)
        {
            Player = new Player(context, SkeletonPlayerAssignment.FirstSkeleton);
            Numbers = new List<DragableNumber>();

            for (int i = 0; i < 5; i++)
            {
                Numbers.Add(new DragableNumber(Player, i * 80, 20, i + 1));
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Player.Skeleton != null)
            {
                Player.Update(gameTime);
            }

            foreach (var num in Numbers)
                num.Update(gameTime);
        }        

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Player.Skeleton != null)
            {
                Player.Draw(spriteBatch);
            }
            else
            {
                DrawMessage(spriteBatch, "No Playerskeleton found!");
            }

            foreach (var num in Numbers)
                num.Draw(spriteBatch);
        }
    }
}