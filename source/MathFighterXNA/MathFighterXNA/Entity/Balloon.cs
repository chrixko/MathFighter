using System.Linq;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using ClownSchool.Physics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ClownSchool.Entity {
    public class Balloon : BaseEntity {

        private Body balloonBody;
        private List<Body> joints = new List<Body>();

        private bool popped;
        private Animation popAnimation;

        public BaseEntity AttachedEntity { get; private set; }
        private FixedMouseJoint fixedJoint;

        public Joint HalfJoint { get; set; }
        public int JointCount { get; set; }
        public float Force { get; set; }

        public Vector2 BalloonSize { get; set; }

        public int Number { get; set; }

        public Balloon(int posX, int posY, int number) {
            X = posX;
            Y = posY;

            Number = number;

            BalloonSize = new Vector2(62, 89);
            Size = new Point(3, 24);
            JointCount = 10;

            collidable = true;
            CollisionType = "balloon";

            popped = false;            
        }        

        public override void Init() {
            popAnimation = new Animation("balloon_pop", Assets.BalloonBoom, 200);

            var balloonRadius = ConvertUnits.ToSimUnits(BalloonSize.X / 2, BalloonSize.Y / 2);
            var jointSize = ConvertUnits.ToSimUnits(3, 12);

            PolygonShape balloonShape = new PolygonShape(PolygonTools.CreateEllipse(balloonRadius.X, balloonRadius.Y, 8), 10);
            PolygonShape jointShape = new PolygonShape(PolygonTools.CreateRectangle(jointSize.X, jointSize.Y), 25);

            var ballAnchor = Vector2.Zero;
            //ball
            {
                Body body = BodyFactory.CreateBody(Screen.World);
                body.BodyType = BodyType.Dynamic;
                body.Position = ConvertUnits.ToSimUnits(X, Y);

                Fixture fixture = body.CreateFixture(balloonShape);
                fixture.Friction = 0.2f;
                fixture.CollisionCategories = Category.Cat2;
                fixture.CollidesWith = Category.All & ~Category.Cat1;

                body.AngularDamping = 0.4f;

                ballAnchor = ConvertUnits.ToSimUnits(new Vector2(X - 2, Y + 27)); //TODO: relative to size

                balloonBody = body;

                Force = -balloonBody.Mass * Screen.World.Gravity.Y * 15;
            }

            for (int i = 0; i < JointCount; i++) {
                Body body = BodyFactory.CreateBody(Screen.World);
                body.BodyType = BodyType.Dynamic;

                Fixture fixture = body.CreateFixture(jointShape);
                fixture.Friction = 0.2f;
                fixture.CollisionCategories = Category.Cat1;
                fixture.CollidesWith = Category.All & ~Category.Cat2;

                joints.Add(body);

                if (i == 0) {
                    body.Position = new Vector2(ballAnchor.X, ballAnchor.Y);

                    RevoluteJoint rj = JointFactory.CreateRevoluteJoint(balloonBody, body, body.GetLocalPoint(ballAnchor));
                    rj.CollideConnected = false;

                    Screen.World.AddJoint(rj);
                } else {
                    var prevBody = joints[i - 1];
                    var anchor = new Vector2(prevBody.Position.X, prevBody.Position.Y + jointSize.Y);
                    body.Position = new Vector2(anchor.X, anchor.Y);

                    RevoluteJoint rj = JointFactory.CreateRevoluteJoint(prevBody, body, body.GetLocalPoint(anchor));
                    rj.CollideConnected = false;
                    Screen.World.AddJoint(rj);

                    if ((int)(JointCount / 2) == i) {
                        HalfJoint = rj;
                    }
                }
            }

            joints.Last().FixedRotation = true;
        }

        public void AttachTo(BaseEntity entity) {
            if (fixedJoint != null) {
                Screen.World.RemoveJoint(fixedJoint);
            }

            AttachedEntity = entity;
            var grabJoint = joints.Last();            

            var pos = grabJoint.WorldCenter;
            pos.Y += ConvertUnits.ToSimUnits(10);            

            fixedJoint = new FixedMouseJoint(grabJoint, pos);
            fixedJoint.MaxForce = 1000f * 10000; //I don't really know which numbers are good here lol
            
            fixedJoint.Frequency = 10;

            Screen.World.AddJoint(fixedJoint);

            fixedJoint.WorldAnchorB = ConvertUnits.ToSimUnits(entity.BoundingBox.Center.X, entity.BoundingBox.Center.Y);
            grabJoint.Awake = true;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var grabJoint = joints[joints.Count - 1];

            balloonBody.ApplyForce(new Vector2(0, Force), balloonBody.WorldCenter);            

            if (fixedJoint != null) {
                X = AttachedEntity.BoundingBox.Center.X;
                Y = AttachedEntity.BoundingBox.Center.Y;

                fixedJoint.WorldAnchorB = ConvertUnits.ToSimUnits(X, Y);
            }

            var slot = GetFirstCollidingEntity("slot");
            if (slot != null && fixedJoint != null && AttachedEntity.GetType() == typeof(PlayerHand)) {                
                (slot as NumberSlot).TryAttach(this);
            }
            
            if (popped) {
                popAnimation.Update(gameTime);
            }


            if (ConvertUnits.ToDisplayUnits(balloonBody.Position.Y) < -200) {
                Screen.RemoveEntity(this);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            foreach (var joint in joints) {
                var jointPos = ConvertUnits.ToDisplayUnits(joint.Position);
                spriteBatch.Draw(Assets.RopeSection, new Rectangle((int)jointPos.X, (int)jointPos.Y, 3, 16), null, Color.White, joint.Rotation, new Vector2(0, 0), SpriteEffects.None, 0);
            }

            if (Screen.World.BodyList.Contains(balloonBody)) {
                var pos = ConvertUnits.ToDisplayUnits(balloonBody.Position);
                spriteBatch.Draw(Assets.BalloonSpritesheet, new Rectangle((int)pos.X, (int)pos.Y, (int)BalloonSize.X, (int)BalloonSize.Y), new Rectangle(62 * (Number - 1), 0, 62, 89), Color.White, balloonBody.Rotation, new Vector2(BalloonSize.X / 2, BalloonSize.Y / 2), SpriteEffects.None, 0);
            }

            if (popped && !popAnimation.Finished) {
                var pos = ConvertUnits.ToDisplayUnits(balloonBody.Position);
                
                spriteBatch.Draw(popAnimation.SpriteSheet, new Rectangle((int)pos.X, (int)pos.Y, popAnimation.FrameWidth, popAnimation.FrameHeight), popAnimation.FrameRectangle, Color.White, 0, new Vector2(popAnimation.FrameWidth / 2, popAnimation.FrameHeight / 2), SpriteEffects.None, 0);
            }
        }

        public void Pop() {
            if (popped)
                return;

            Assets.BalloonPop.Play();
            popAnimation.Play(false);
            popped = true;
            Screen.World.RemoveBody(balloonBody);
        }

        public void Cut() {
            Screen.World.RemoveJoint(HalfJoint);
            Screen.World.RemoveJoint(fixedJoint);
            AttachedEntity = null;
            fixedJoint = null;
        }

        public override void Delete() {
            if (Screen.World.BodyList.Contains(balloonBody))
                Screen.World.RemoveBody(balloonBody);
            
            foreach (Body joint in joints) {
                Screen.World.RemoveBody(joint);
            }
            if (Screen.World.JointList.Contains(fixedJoint))
                Screen.World.RemoveJoint(fixedJoint);
            
            //ToDo remove revo joints
        }
    }
}
