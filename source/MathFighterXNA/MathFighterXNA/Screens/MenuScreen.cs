using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownSchool.Entity.Menu;
using ClownSchool.Entity;

namespace ClownSchool.Screens {
    public class MenuScreen : GameScreen {

        public Menu MainMenu;

        public Player Player;

        public MenuScreen(KinectContext context): base(context) {
            MainMenu = new Menu();
        }

        public override void Init() {
            base.Init();

            Player = new Player(Context, SkeletonPlayerAssignment.FirstSkeleton);
            Player.DrawHands = true;

            AddEntity(Player);

            AddCurtain();            

            int logoWidth = 350;
            int logoHeight = 357;
            
            var logo = new SimpleGraphic(Assets.MenuLogo, (MainGame.Width / 2) - logoWidth / 2, ((MainGame.Height / 2) - logoHeight / 2) - 100, logoWidth, logoHeight);
            AddEntity(logo);

            var balloon1 = new SimpleGraphic(Assets.MenuBalloons, 145, 250 - 190, 150, 195);
            AddEntity(balloon1);

            var balloon2 = new SimpleGraphic(Assets.MenuBalloons, 323, 250 - 190, 150, 195);
            AddEntity(balloon2);

            var balloon3 = new SimpleGraphic(Assets.MenuBalloons, 868, 250 - 190, 150, 195);
            AddEntity(balloon3);

            var balloon4 = new SimpleGraphic(Assets.MenuBalloons, 1048, 250 - 190, 150, 195);
            AddEntity(balloon4);

            var singlePlayer = new MenuItem(Assets.MenuSignSinglePlayer, 200, 250, null);
            MainMenu.AddItem(singlePlayer);

            var highscore = new MenuItem(Assets.MenuSignHighscore, 200, 450, null);
            MainMenu.AddItem(highscore);
            
            var multiPlayer = new MenuItem(Assets.MenuSignMultiPlayer, MainGame.Width - 400, 250, OnClick_Multiplayer);
            MainMenu.AddItem(multiPlayer);

            var help = new MenuItem(Assets.MenuSignHelp, MainGame.Width - 400, 450, null);
            MainMenu.AddItem(help);

            AddEntity(MainMenu);            
        }

        void OnClick_Multiplayer() {
            Manager.AddScreen(new VersusPlayerScreen(Context));
            Manager.RemoveScreen(this);
        }
    }
}
