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
        public KinectContext Context { get; private set; }

        public Skeleton Skeleton
        {
            get
            {
                return Context.GetFirstSkeleton();
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

        private int skeletonId { get; set; }        

        public Player(KinectContext context, int skeletonId)
        {
            this.Context = context;
            this.skeletonId = skeletonId;
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
