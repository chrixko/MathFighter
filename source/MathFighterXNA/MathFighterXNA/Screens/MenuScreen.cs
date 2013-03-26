using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownSchool.Entity.Menu;
using ClownSchool.Entity;
using ClownSchool.Tweening;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using ClownSchool.Bang.Actions;

namespace ClownSchool.Screens {
    public class MenuScreen : GameScreen {

        public Menu MainMenu;

        public Player Player;

        private Tweener tweenerY;

        private Dictionary<BaseEntity, Vector2> tweenObjects = new Dictionary<BaseEntity, Vector2>();

        public MenuScreen(KinectContext context): base(context) {
            MainMenu = new Menu();
        }

        public override void Init() {
            base.Init();

            MediaPlayer.Volume = 0f;

            Manager.FadeInSong(Assets.MenuSong, true);

            tweenerY = new Tweener(0, 20, 1.5f, Linear.EaseIn);
            tweenerY.Ended += delegate() { tweenerY.Reverse(); };

            Player = new Player(Context, SkeletonPlayerAssignment.FirstSkeleton);
            Player.DrawHands = true;

            AddEntity(Player);

            AddCurtain();            

            int logoWidth = 350;
            int logoHeight = 357;
            
            var logo = new SimpleGraphic(Assets.MenuLogo, (MainGame.Width / 2) - logoWidth / 2, ((MainGame.Height / 2) - logoHeight / 2) - 100, logoWidth, logoHeight);
            AddEntity(logo);

            var balloon1 = new SimpleGraphic(Assets.MenuBalloons, 145, 250 - 190, 150, 195);
            tweenObjects.Add(balloon1, new Vector2(balloon1.X, balloon1.Y));
            AddEntity(balloon1);

            var balloon2 = new SimpleGraphic(Assets.MenuBalloons, 323, 250 - 190, 150, 195);
            tweenObjects.Add(balloon2, new Vector2(balloon2.X, balloon2.Y));
            AddEntity(balloon2);

            var balloon3 = new SimpleGraphic(Assets.MenuBalloons, 868, 250 - 190, 150, 195);
            tweenObjects.Add(balloon3, new Vector2(balloon3.X, balloon3.Y));
            AddEntity(balloon3);

            var balloon4 = new SimpleGraphic(Assets.MenuBalloons, 1048, 250 - 190, 150, 195);
            tweenObjects.Add(balloon4, new Vector2(balloon4.X, balloon4.Y));
            AddEntity(balloon4);

            var singlePlayer = new MenuItem(Assets.MenuSignSinglePlayer, 200, 450, null);
            tweenObjects.Add(singlePlayer, new Vector2(singlePlayer.X, singlePlayer.Y));
            MainMenu.AddItem(singlePlayer);

            var highscore = new MenuItem(Assets.MenuSignHighscore, MainGame.Width - 400, 250, null);
            tweenObjects.Add(highscore, new Vector2(highscore.X, highscore.Y));
            MainMenu.AddItem(highscore);
            
            var multiPlayer = new MenuItem(Assets.MenuSignMultiPlayer, 200, 250, OnClick_Multiplayer);
            tweenObjects.Add(multiPlayer, new Vector2(multiPlayer.X, multiPlayer.Y));
            MainMenu.AddItem(multiPlayer);            
            
            var help = new MenuItem(Assets.MenuSignHelp, MainGame.Width - 400, 450, null);
            tweenObjects.Add(help, new Vector2(help.X, help.Y));
            MainMenu.AddItem(help);

            AddEntity(MainMenu);            
        }

        void OnClick_Multiplayer() {
            Manager.AddScreen(new VersusPlayerScreen(Context));
            Manager.RemoveScreen(this);
            Manager.FadeInSong(Assets.GameSong, true);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            tweenerY.Update(gameTime);

            foreach (var ent in tweenObjects.Keys) {
                ent.Y = (int)tweenObjects[ent].Y + (int)tweenerY.Position;
            }
        }
    }
}
