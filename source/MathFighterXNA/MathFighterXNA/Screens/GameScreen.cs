using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MathFighterXNA.Screens
{
    public abstract class GameScreen
    {
        public KinectContext Context { get; private set; }

        public List<Entity> Entities = new List<Entity>();

        public GameScreen(KinectContext context)
        {
            Context = context;
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            entity.Screen = this;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        //public abstract bool AllPlayersReady();
    }
}
