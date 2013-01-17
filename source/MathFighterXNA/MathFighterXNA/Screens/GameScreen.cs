using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathFighterXNA.Screens {

    public abstract class GameScreen {

        public KinectContext Context { get; private set; }

        public List<BaseEntity> Entities = new List<BaseEntity>();

        public GameScreen(KinectContext context) {
            Context = context;
        }

        public void AddEntity(BaseEntity entity) {
            Entities.Add(entity);
            entity.Screen = this;

            entity.Init();
        }

        public void RemoveEntity(BaseEntity entity) {
            entity.Delete();
            Entities.Remove(entity);
        }

        public abstract void Init();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);        
    }
}
