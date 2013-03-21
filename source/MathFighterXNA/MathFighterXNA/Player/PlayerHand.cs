using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using ClownSchool.Entity;

namespace ClownSchool {

    public class PlayerHand : BaseEntity {

        public Player Player { get; private set; }
        public JointType Hand { get; private set; }        
        public Balloon DraggingBalloon { get; set; }

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

        public override void Update(GameTime gameTime) {
            if (Player.IsReady) {
                this.Position = Context.SkeletonPointToScreen(Player.Skeleton.Joints[Hand].Position);                 
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
        }

        public override void Delete() {
        }
    }
}