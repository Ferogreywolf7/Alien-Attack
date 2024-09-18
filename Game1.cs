using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Alien_Attack
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player1;
        public Controls controls;
        private UI ui;
        private Bullets playerBullet;
        private Bunkers bunkers;
        private EnemyController enemies;
        private Vector2 player1StartPos;
        private Texture2D player1Texture;
        private Texture2D playerBulletTexture;
        private Texture2D textBorder;
        private Texture2D bunker;
        private Texture2D enemyTexture;
        private SpriteFont font;
        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;
        private Keys shoot;
        private Keys pause;
        private bool gamePaused;
        private bool playerBulletActive;
        private int extraXCoord;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player1StartPos = new Vector2(50, 800);
            gamePaused = false;
            playerBulletActive = false;

            //Set window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            controls = new Controls();
            getControls();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            textBorder = Content.Load<Texture2D>("textBorder"); 
            playerBulletTexture = Content.Load<Texture2D>("bulletPlaceholder");
            player1Texture = Content.Load<Texture2D>("playerPlaceholder");
            bunker = Content.Load<Texture2D>("bunkerPlaceholder");
            enemyTexture = Content.Load<Texture2D>("enemyPlaceholder");
            font = Content.Load<SpriteFont>("testFont");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player1 = new Player(player1Texture, player1StartPos, controls);          
            ui = new UI(_spriteBatch, font, textBorder, controls);
            bunkers = new Bunkers(bunker, 2);
            enemies = new EnemyController(_spriteBatch, enemyTexture, 2, 5, new Vector2(200, 50));
            enemies.spawnEnemies();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            // Calls updates  
            // pauses the main game when the button is pressed
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            pauseGame();

            if (!gamePaused)
            {
                firePlayerBullet();
                
                player1.updatePlayer(currentKeyState, previousKeyState);
                enemies.updateAllEnemies();
                if (playerBulletActive)
                {
                    if (playerBullet.getBulletPos().Y <= -60)
                    {
                        playerBulletActive = false;
                    }
                }
            }
            if (gamePaused) {
                getControls();
                player1.getControls();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            player1.drawPlayer(_spriteBatch);
            //prevents the bullet from moving when the game is paused

            if (!gamePaused)
            {
                enemies.drawAllEnemies();
                bunkers.drawBunkers(_spriteBatch);
                if (playerBulletActive)
                {
                    playerBullet.drawBullets(_spriteBatch);
                    playerBullet.updateBullets();
                }
            }

            if (gamePaused)
            {
                ui.drawPauseMenu();
                
            }
            base.Draw(gameTime);
        }


        public void getControls()
        {
            
            shoot = controls.getFire();
            pause = controls.getPause();
        }

        public void firePlayerBullet()
        {
            //Bullet will only be fired when there is no other bullet on screen and the player has pressed the key for firing
            if (currentKeyState.IsKeyDown(shoot) && !playerBulletActive)
            {
                extraXCoord = player1.getPLayerWidth()/2;
                playerBullet = new Bullets(5, playerBulletTexture, "up", player1.getPosition(), font, extraXCoord);
                playerBulletActive = true;
            }
        }

        public void deactivateBullet() {
            playerBulletActive = false;
        }

        //pause game when the assigned key is pressed and the game isnt already paused
        private void pauseGame()
        {
            if (currentKeyState.IsKeyDown(pause) && previousKeyState.IsKeyUp(pause))
            {
                if (!gamePaused)
                {
                    gamePaused = true;
                }

                else if (gamePaused)
                {
                    gamePaused = false;
                }
            }
        }
    }
}
