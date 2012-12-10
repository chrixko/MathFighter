using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA
{
    public class Player : Entity
    {
        public SkeletonPlayerAssignment SkeletonAssignment { get; set; }
        public KinectContext Context { get; private set; }

        public PlayerHand LeftHand { get; private set; }
        public PlayerHand RightHand { get; private set; }

        public Skeleton Skeleton
        {
            get
            {
                switch (SkeletonAssignment)
                {
                    case SkeletonPlayerAssignment.FirstSkeleton:
                        return Context.GetFirstSkeleton();
                    case SkeletonPlayerAssignment.LeftSkeleton:
                        return Context.GetLeftSkeleton();
                    case SkeletonPlayerAssignment.RightSkeleton:
                        return Context.GetRightSkeleton();
                }

                return null;
            }
        }

        public bool IsReady
        {
            get
            {
                return Skeleton != null;
            }
        }     

        public Player(KinectContext context, SkeletonPlayerAssignment assignment)
        {
            this.Context = context;
            this.SkeletonAssignment = assignment;

            LeftHand = new PlayerHand(this, JointType.HandLeft);
            RightHand = new PlayerHand(this, JointType.HandRight);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsReady)
            {
                LeftHand.Update(gameTime);
                RightHand.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsReady)
            {
                LeftHand.Draw(spriteBatch);
                RightHand.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.DrawString(Assets.DebugFont, "No Player-Skeleton found!", new Vector2(0, 0), Color.Red);
            }
        }
    }
}
