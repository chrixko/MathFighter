using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathFighterXNA.Bang;
using MathFighterXNA.Bang.Actions;
using MathFighterXNA.Entity;
using MathFighterXNA.Tweening;

namespace MathFighterXNA.Screens {

    public abstract class GameScreen {

        public KinectContext Context { get; private set; }

        public List<BaseEntity> Entities = new List<BaseEntity>();
        public ActionList Actions = new ActionList();
        public Coroutines Coroutines = new Coroutines();

        private SimpleGraphic CurtainLeft;
        private SimpleGraphic CurtainRight;
        private SimpleGraphic BackgroundLeft;
        private SimpleGraphic BackgroundRight;

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

        public virtual void Init() {
            CurtainLeft = new SimpleGraphic(Assets.CurtainTopLeft, 0, 0, 260, MainGame.Height - 50);
            CurtainRight = new SimpleGraphic(Assets.CurtainTopRight, MainGame.Width - 260, 0, 260, MainGame.Height - 50);

            BackgroundLeft = new SimpleGraphic(Assets.CurtainBottomLeft, 0, 0, MainGame.Width / 2, MainGame.Height + 20);
            BackgroundRight = new SimpleGraphic(Assets.CurtainBottomRight, MainGame.Width / 2, 0, MainGame.Width / 2, MainGame.Height + 20);

            AddEntity(BackgroundLeft);
            AddEntity(BackgroundRight);

            AddEntity(CurtainLeft);
            AddEntity(CurtainRight);

            Actions.AddAction(new TweenPositionTo(BackgroundLeft, new Vector2(-(MainGame.Width / 2) + 160, 0), 2f, Sinusoidal.EaseOut), false);
            Actions.AddAction(new TweenPositionTo(BackgroundRight, new Vector2(MainGame.Width - 160, 0), 2f, Sinusoidal.EaseOut), false);

        }

        public virtual void Update(GameTime gameTime) {
            //Dirty? Calling ToArray to make a copy of the entity collection preventing crashing when entities create other entities through an update call
            foreach (var ent in Entities.ToArray()) {
                ent.Update(gameTime);
            }

            Actions.Update(gameTime);
            Coroutines.Update();
        }
        public virtual void Draw(SpriteBatch spriteBatch) {
            foreach (var ent in Entities.ToArray()) {
                ent.Draw(spriteBatch);
            }
        }
    }
}
