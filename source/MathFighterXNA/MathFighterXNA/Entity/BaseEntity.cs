using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Screens;
using MathFighterXNA.Bang;

namespace MathFighterXNA {

    public abstract class BaseEntity {
        public Point Position {
            get {
                return new Point(X, Y);
            } 
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        public ActionList Actions = new ActionList();
        public Coroutines Coroutines = new Coroutines();

        public int X { get; set; }
        public int Y { get; set; }

        public Point Size { get; set; }
        public Point Offset { get; set; }

        public GameScreen Screen { get; set; }

        public bool collidable = true;
        public string CollisionType { get; set; }

        public IEnumerable<BaseEntity> GetCollidingEntities(int posX, int posY, string type) {
            return from ent in Screen.Entities where ent.collidable && ent.CollisionType == type && ent.BoundingBox.Intersects(new Rectangle(posX, posY, Size.X, Size.Y)) select ent;
        }

        public BaseEntity GetFirstCollidingEntity(int posX, int posY, string type) {
            return GetCollidingEntities(posX, posY, type).FirstOrDefault();
        }

        public Rectangle BoundingBox {
            get {
                return new Rectangle(X + Offset.X, Y + Offset.Y, Size.X, Size.Y);
            }
        }

        public abstract void Init();
        public virtual void Update(GameTime gameTime) {
            Actions.Update(gameTime);
            Coroutines.Update();
        }
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Delete();
    }
}
