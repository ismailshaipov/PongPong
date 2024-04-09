using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcanoid
{
    public partial class Form1 : Form
    {

        MapController map;
        Player player;
        Physics2DController physics;


        public Label scoreLabel;
        public Label livesLabel;

        public Form1()
        {

            InitializeComponent();

            scoreLabel = new Label();
            scoreLabel.Location = new Point((MapController.mapWidth) * 20 + 1, 50);

            livesLabel = new Label();
            livesLabel.Location = new Point((MapController.mapWidth) * 20 + 1, 100);


            

            this.Controls.Add(scoreLabel);
            this.Controls.Add(livesLabel);
            

            timer1.Tick += new EventHandler(update);
            timer2.Tick += new EventHandler(animationUpdate);//for animations
            timer2.Interval = 100;

            this.KeyDown += new KeyEventHandler(inputCheck);

            Init();
        }
        

        private void animationUpdate(object sender, EventArgs e)
        {
            if (player.platformAnnimationFrame < 2)
                player.platformAnnimationFrame++;
            else
                player.platformAnnimationFrame = 0;
        }

        private void inputCheck(object sender, KeyEventArgs e)
        {
            map.map[player.platformY / 20, player.platformX / 20] = 0;
            map.map[player.platformY / 20, player.platformX / 20 + 1] = 0;
            map.map[player.platformY / 20, player.platformX / 20 + 2] = 0;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (player.platformX / 20 + 2 < MapController.mapWidth - 1)
                        player.platformX += 20;
                    break;
                case Keys.Left:
                    if (player.platformX > 0)
                        player.platformX -= 20;
                    break;
            }
            map.map[player.platformY / 20, player.platformX / 20] = 9;
            map.map[player.platformY / 20, player.platformX / 20 + 1] = 99;
            map.map[player.platformY / 20, player.platformX / 20 + 2] = 999;
        }


        private void update(object sender, EventArgs e)
        {
            if (player.ballY / 20 + player.dirY > MapController.mapHeight - 1)
            {
                player.lives--;
                if (player.lives == 0)
                {
                    gameOver();
                }
                else Continue();
            }


            map.map[player.ballY / 20, player.ballX / 20] = 0;
            if (!physics.IsCollide(player, map, scoreLabel))
                player.ballX += player.dirX * 10;
            if (!physics.IsCollide(player, map, scoreLabel))
                player.ballY += player.dirY * 10;
            map.map[player.ballY / 20, player.ballX / 20] = 8;

            map.map[player.platformY / 20, player.platformX / 20] = 9;
            map.map[player.platformY / 20, player.platformX / 20 + 1] = 99;
            map.map[player.platformY / 20, player.platformX / 20 + 2] = 999;

            Invalidate();
        }

        private void gameOver()
        {
            timer1.Stop();
            MessageBox.Show(this, "Game Over!");
            Init();
        }

        public void GeneratePlatforms()
        {
            Random r = new Random();
            for (int i = 0; i < MapController.mapHeight / 3; i++)
            {
                for (int j = 0; j < MapController.mapWidth; j += 2)
                {
                    int currPlatform = r.Next(1, 5);
                    map.map[i, j] = currPlatform;
                    map.map[i, j + 1] = currPlatform + currPlatform * 10;
                }
            }
        }


        public void Continue()
        {
            timer1.Interval = 1;
            scoreLabel.Text = "Score: " + player.score;
            livesLabel.Text = "Lives: " + player.lives;
            map.map[player.platformY / 20, player.platformX / 20] = 9;
            map.map[player.platformY / 20, player.platformX / 20 + 1] = 99;
            map.map[player.platformY / 20, player.platformX / 20 + 2] = 999;
            map.map[player.ballY / 20, player.ballX / 20] = 0;

            player.ballY = 300;
            player.ballX = 300;

            map.map[player.ballY / 20, player.ballX / 20] = 8;

            player.dirX = 1;
            player.dirY = -1;


            timer1.Start();
        }

        public void Init()
        {
            map = new MapController();
            player = new Player();
            physics = new Physics2DController();

            this.Width = (MapController.mapWidth + 4) * 20;
            this.Height = (MapController.mapHeight + 2) * 20;


            timer1.Interval = 1;

            player.score = 0;
            player.lives = 1;
            scoreLabel.Text = "Score: " + player.score;
            livesLabel.Text = "Lives: " + player.lives;
            for (int i = 0; i < MapController.mapHeight; i++)
            {
                for (int j = 0; j < MapController.mapWidth; j++)
                {
                    map.map[i, j] = 0;
                }
            }

            player.platformX = (MapController.mapWidth - 1) / 2 * 20;
            player.platformY = (MapController.mapHeight - 1) * 20;

            map.map[player.platformY / 20, player.platformX / 20] = 9;
            map.map[player.platformY / 20, player.platformX / 20 + 1] = 99;
            map.map[player.platformY / 20, player.platformX / 20 + 2] = 999;

            player.ballY = player.platformY - 20 ;
            player.ballX = player.platformX + 20;

            map.map[player.ballY / 20, player.ballX / 20] = 8;

            player.dirX = 1;
            player.dirY = -1;

            GeneratePlatforms();

            timer1.Start();
            timer2.Start();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            map.DrawArea(e.Graphics);
            map.DrawMap(e.Graphics, player);
        }

    }
}