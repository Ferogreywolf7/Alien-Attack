using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Alien_Attack
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        Player player1;
        controlsMenu controls;
        Vector2 player1StartPos;
        Texture2D player1Texture;
        Texture2D playerBulletTexture;
        SpriteFont font;
        Bullets playerBullet;
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;
        Keys shoot;

        bool bulletActive = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player1StartPos = new Vector2 (50, 300);
            getControls();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerBulletTexture = Content.Load<Texture2D>("bulletPlaceholder");
            player1Texture = Content.Load<Texture2D>("playerPlaceholder");
            player1 = new Player(player1Texture, player1StartPos);

            font = Content.Load<SpriteFont>("testFont");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            // Calls updates  
            firePlayerBullet();
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            player1.playerUpdate( currentKeyState, previousKeyState);

            if (bulletActive)
            {
                if (playerBullet.getBulletPos().Y <= -60) {
                    bulletActive = false;
                }
            }
            //test
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            player1.DrawPlayer(_spriteBatch);
            if (bulletActive)
            {
                playerBullet.drawBullet(_spriteBatch);
                playerBullet.updateBullets();
            }
            base.Draw(gameTime);
        }

        public void getControls()
        {
            controls = new controlsMenu();
            shoot = controls.getFire();
        }

        public void firePlayerBullet()
        {
            //Bullet will only be fired when there is no other bullet on screen and the player has pressed the key for firing
            if (currentKeyState.IsKeyDown(shoot) && !bulletActive)
            {
                
                playerBullet = new Bullets(5, playerBulletTexture, "up", player1.getPosition(), font);
                bulletActive = true;                
            }
        }

    }
}
