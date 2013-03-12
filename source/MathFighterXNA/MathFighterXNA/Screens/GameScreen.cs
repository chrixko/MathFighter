using System.Collections;
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
        public virtual void Update(GameTime gameTime) {
            //Dirty? Calling ToArray to make a copy of the entity collection preventing crashing when entities create other entities through an update call
            foreach (var ent in Entities.ToArray()) {
                ent.Update(gameTime);
            }
        }
        public abstract void Draw(SpriteBatch spriteBatch);        
    }
}
