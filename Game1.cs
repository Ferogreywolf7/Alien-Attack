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

        Player player1;
        Controls controls;
        UI ui;
        Vector2 player1StartPos;
        Texture2D player1Texture;
        Texture2D playerBulletTexture;
        Texture2D textBorder;
        SpriteFont font;
        Bullets playerBullet;
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;
        private Keys shoot;
        private Keys pause;
        private bool gamePaused;
        private bool bulletActive;
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
            bulletActive = false;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            textBorder = Content.Load<Texture2D>("textBorder"); 
            playerBulletTexture = Content.Load<Texture2D>("bulletPlaceholder");
            player1Texture = Content.Load<Texture2D>("playerPlaceholder");
            player1 = new Player(player1Texture, player1StartPos);

            font = Content.Load<SpriteFont>("testFont");
            ui = new UI(_spriteBatch, font, textBorder, controls);

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
                
                player1.playerUpdate(currentKeyState, previousKeyState);

                if (bulletActive)
                {
                    if (playerBullet.getBulletPos().Y <= -60)
                    {
                        bulletActive = false;
                    }
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            player1.DrawPlayer(_spriteBatch);
            //prevents the bullet from moving when the game is paused

            if (!gamePaused)
            {
                if (bulletActive)
                {
                    playerBullet.drawBullet(_spriteBatch);
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
            if (currentKeyState.IsKeyDown(shoot) && !bulletActive)
            {
                extraXCoord = player1.getPLayerWidth()/2;
                playerBullet = new Bullets(5, playerBulletTexture, "up", player1.getPosition(), font, extraXCoord);
                bulletActive = true;
            }
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
