using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace MathFighterXNA
{
    public class PlayerHand : BaseEntity
    {
        public Player Player { get; private set; }
        public JointType Hand { get; private set; }

        public KinectContext Context
        {
            get
            {
                return Player.Context;
            }
        }

        public PlayerHand(Player player, JointType hand)
        {            
            this.Offset = new Point(-20, -20);
            this.Size = new Point(40, 40);

            Player = player;
            Hand = hand;

            CollisionType = "hand";
        }

        public override void Update(GameTime gameTime)
        {
            if (Player.IsReady)
            {
                this.Position = Context.SkeletonPointToScreen(Player.Skeleton.Joints[Hand].Position);                 
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.NumberBackgroundSprite, BoundingBox, Color.White);
        }
    }
}
