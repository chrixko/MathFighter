using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA
{
    public class Player : BaseEntity
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

        public Player(KinectContext context, SkeletonPlayerAssignment assignment, Screens.GameScreen screen)
        {
            this.Context = context;
            this.SkeletonAssignment = assignment;

            Screen = screen; // meeeeh :(

            LeftHand = new PlayerHand(this, JointType.HandLeft);
            RightHand = new PlayerHand(this, JointType.HandRight);

            Screen.AddEntity(LeftHand);
            Screen.AddEntity(RightHand);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsReady)
            {
                spriteBatch.DrawString(Assets.DebugFont, "No Player-Skeleton found!", new Vector2(0, 0), Color.Red);
            }
        }
    }
}
