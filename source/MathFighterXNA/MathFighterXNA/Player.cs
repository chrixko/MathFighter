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

        public readonly Skeleton Skeleton
        {
            get
            {
                return Context.GetSkeletonById(skeletonId);
            }
        }

        public readonly Rectangle LeftHandBounds
        {
            get
            {
                return getHandBounds(Skeleton.Joints[JointType.HandLeft].Position);
            }
        }

        public readonly Rectangle RightHandBounds
        {
            get
            {
                return getHandBounds(Skeleton.Joints[JointType.HandRight].Position);
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

        private Rectangle getHandBounds(SkeletonPoint point)
        {
            return getHandBounds(this.Context.SkeletonPointToScreen(point));
        }

        private Rectangle getHandBounds(Vector2 position)
        {
            return new Rectangle((int)position.X - 20, (int)position.Y - 20, 40, 40);
        }
    }
}
