using System;
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

        private BaseEntity attachedEntity;
        private FixedMouseJoint fixedJoint;

        public int JointCount { get; set; }
        public float Force { get; set; }

        public Vector2 BalloonSize { get; set; }

        public int Number { get; set; }

        public Balloon(int posX, int posY, Vector2 balloonSize, int number) {
            X = posX;
            Y = posY;

            BalloonSize = balloonSize;

            Number = number;

            Size = new Point(5, 24);
            JointCount = 10;

            collidable = true;
            CollisionType = "balloon";
        }        

        public override void Init() {
            var balloonRadius = ConvertUnits.ToSimUnits(BalloonSize.X / 2, BalloonSize.Y / 2);
            var jointSize = ConvertUnits.ToSimUnits(5, 12);

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

                Force = -balloonBody.Mass * Screen.World.Gravity.Y * 19f;
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
                }
            }
        }

        public void AttachTo(BaseEntity entity) {
            attachedEntity = entity;
            var grabJoint = joints.Last();

            X = entity.X;
            Y = entity.Y + 6;

            grabJoint.Position = ConvertUnits.ToSimUnits(entity.X, entity.Y);

            fixedJoint = new FixedMouseJoint(grabJoint, ConvertUnits.ToSimUnits(entity.BoundingBox.Center.X, entity.BoundingBox.Center.Y));
            fixedJoint.MaxForce = 1000f * 10000; //I don't really know which numbers are good here lol
            Screen.World.AddJoint(fixedJoint);
            grabJoint.Awake = true;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var grabJoint = joints[joints.Count - 1];

            balloonBody.ApplyForce(new Vector2(0, Force), balloonBody.WorldCenter);            

            if (fixedJoint != null) {                
                fixedJoint.WorldAnchorB = ConvertUnits.ToSimUnits(attachedEntity.BoundingBox.Center.X, attachedEntity.BoundingBox.Center.Y);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            foreach (var joint in joints) {
                var jointPos = ConvertUnits.ToDisplayUnits(joint.Position);
                spriteBatch.Draw(Assets.RopeSection, new Rectangle((int)jointPos.X, (int)jointPos.Y, 5, 16), null, Color.White, joint.Rotation, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            
            var pos = ConvertUnits.ToDisplayUnits(balloonBody.Position);
            spriteBatch.Draw(Assets.BalloonSpritesheet, new Rectangle((int)pos.X, (int)pos.Y, (int)BalloonSize.X, (int)BalloonSize.Y), new Rectangle(62 * (Number - 1), 0, 62, 82), Color.White, balloonBody.Rotation, new Vector2(BalloonSize.X / 2, BalloonSize.Y / 2), SpriteEffects.None, 0);
        }

        public override void Delete() {
            Screen.World.RemoveBody(balloonBody);
            foreach (Body joint in joints) {
                Screen.World.RemoveBody(joint);
            }

            Screen.World.RemoveJoint(fixedJoint);

            //ToDo remove revo joints
        }
    }
}
