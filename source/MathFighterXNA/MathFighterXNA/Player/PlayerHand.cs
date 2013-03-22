using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using ClownSchool.Entity;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Linq;

namespace ClownSchool {

    public class PlayerHand : BaseEntity {

        public Player Player { get; private set; }
        public JointType Hand { get; private set; }        
        public Balloon DraggingBalloon { get; set; }

        public bool IsGrabbing { get; private set; }

        public KinectContext Context {
            get {
                return Player.Context;
            }
        }

        public PlayerHand(Player player, JointType hand) {            
            this.Offset = new Point(-20, -20);
            this.Size = new Point(40, 40);

            Player = player;
            Hand = hand;

            CollisionType = "hand";
        }

        public override void Init() {
        }

        private InteractionHandPointer getHandPointer() {   
            UserInfo userInfo;

            if (Context.UserInfos.TryGetValue(Player.Skeleton.TrackingId, out userInfo)) {
                return (from InteractionHandPointer hp in userInfo.HandPointers where hp.HandType == (Hand == JointType.HandLeft ? InteractionHandType.Left : InteractionHandType.Right) select hp).FirstOrDefault();
            }

            return null;         
        }

        public override void Update(GameTime gameTime) {
            if (Player.IsReady) {

                #region dragging
                if (Configuration.GRABBING_ENABLED) {
                    var handPointer = getHandPointer();

                    if (handPointer != null) {
                        if (handPointer.HandEventType == InteractionHandEventType.Grip) {
                            IsGrabbing = true;
                        } else if (handPointer.HandEventType == InteractionHandEventType.GripRelease) {
                            IsGrabbing = false;

                            if (this.DraggingBalloon != null) {
                                DraggingBalloon.Cut();
                                DraggingBalloon = null;
                            }
                        }
                    }
                }
                #endregion
                           
                this.Position = Context.SkeletonPointToScreen(Player.Skeleton.Joints[Hand].Position);                 
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Configuration.GRABBING_ENABLED) {
                if (IsGrabbing) {
                    spriteBatch.Draw(Assets.CactusSprite, BoundingBox, new Color(100, 100, 100, 50));
                }
            }
                        
        }

        public void Grab(Balloon balloon) {
            balloon.AttachTo(this);
            this.DraggingBalloon = balloon;

            Assets.BalloonGrab.Play();  
        }

        public override void Delete() {
        }
    }
}