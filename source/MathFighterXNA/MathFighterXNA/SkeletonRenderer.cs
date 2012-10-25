using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MathFighterXNA
{
    public class SkeletonRenderer
    {
        public KinectContext Context { get; private set; }

        public Texture2D CurrentBitmap { get; private set; }

        private float renderHeight { get; set; }
        private float renderWidth { get; set; }

        private const double JointThickness = 3;
        private const double BodyCenterThickness = 10;
        private const double ClipBoundsThickness = 10;
        
        private readonly Color centerPointBrush = Color.Blue;
        private readonly Color trackedJointBrush = Color.Yellow;
        private readonly Color inferredJointBrush = Color.Black;

        private Vector2 jointOrigin;
        public Texture2D jointTexture;
        private Texture2D boneTexture;

        public SkeletonRenderer(KinectContext context, float renderWidth, float renderHeight)
        {
            Context = context;            

            this.renderWidth = renderWidth;
            this.renderHeight = renderHeight;
        }

        public void LoadContent(ContentManager content)
        {
            jointTexture = content.Load<Texture2D>("Joint");
            boneTexture = content.Load<Texture2D>("Bone");
            this.jointOrigin = new Vector2(this.jointTexture.Width / 2, this.jointTexture.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Skeleton skel in Context.Skeletons)
            {
                if (skel.TrackingState == SkeletonTrackingState.Tracked)
                {
                    DrawBonesAndJoints(skel, spriteBatch);
                }
            }
        }

        private void DrawBonesAndJoints(Skeleton skeleton, SpriteBatch spriteBatch)
        {                        
            // Render Torso
            this.DrawBone(skeleton, spriteBatch, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, spriteBatch, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, spriteBatch, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, spriteBatch, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, spriteBatch, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, spriteBatch, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, spriteBatch, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.DrawBone(skeleton, spriteBatch, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, spriteBatch, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, spriteBatch, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, spriteBatch, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, spriteBatch, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, spriteBatch, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.DrawBone(skeleton, spriteBatch, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, spriteBatch, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, spriteBatch, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, spriteBatch, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, spriteBatch, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, spriteBatch, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            foreach (Joint j in skeleton.Joints)
            {
                Color jointColor = Color.Green;
                if (j.TrackingState != JointTrackingState.Tracked)
                {
                    jointColor = Color.Yellow;
                }

                spriteBatch.Draw(this.jointTexture, this.Context.SkeletonPointToScreen(j.Position), null, jointColor, 0.0f, this.jointOrigin, 1.0f, SpriteEffects.None, 0.0f);
                if (j.JointType == JointType.HandRight)
                {
                    var p = this.Context.SkeletonPointToScreen(j.Position);
                    Debug.WriteLine("HandLeft: x=" + p.X + " y=" + p.Y);
                }
            }
        }

        private void DrawBone(Skeleton skeleton, SpriteBatch spriteBatch, JointType jointType0, JointType jointType1)
        {
            //Braucht keiner
        }
    }
}