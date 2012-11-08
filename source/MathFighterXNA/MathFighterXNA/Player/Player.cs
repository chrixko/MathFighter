using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA
{
    public class Player
    {

        public SkeletonPlayerAssignment SkeletonAssignment { get; set; }
        public KinectContext Context { get; private set; }

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

        public Rectangle LeftHandBounds
        {
            get
            {
                return GetHandBounds(JointType.HandLeft);
            }
        }

        public Rectangle RightHandBounds
        {
            get
            {
                return GetHandBounds(JointType.HandRight);
            }
        }       

        public Player(KinectContext context, SkeletonPlayerAssignment assignment)
        {
            this.Context = context;
            this.SkeletonAssignment = assignment;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public Rectangle GetHandBounds(JointType jointType)
        {
            return GetHandBounds(Skeleton.Joints[jointType].Position);                       
        }

        public Rectangle GetHandBounds(SkeletonPoint point)
        {
            return GetHandBounds(this.Context.SkeletonPointToScreen(point));
        }

        public Rectangle GetHandBounds(Vector2 position)
        {
            return new Rectangle((int)position.X - 20, (int)position.Y - 20, 40, 40);
        }
    }
}
