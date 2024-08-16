using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

/* Problems:
    Need to update bullet but each is its own object which needs to be created before it can be called   
*/
namespace Alien_Attack
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Player player1;
        Vector2 player1StartPos;
        Texture2D player1Texture;
        Texture2D playerBulletTexture;
        Bullets playerBullet;
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;
        bool bulletActive;
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerBulletTexture = Content.Load<Texture2D>("bulletPlaceholder");
            player1Texture = Content.Load<Texture2D>("playerPlaceholder");
            player1 = new Player(player1Texture, player1StartPos);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player1.playerUpdate();
            firePlayerBullet();
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            //test
            if (bulletActive)
            {
                playerBullet.drawBullet(_spriteBatch);
                playerBullet.updateBullets();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            player1.DrawPlayer(_spriteBatch);
            base.Draw(gameTime);
        }

        public void firePlayerBullet()
        {
            if (currentKeyState.IsKeyDown(Keys.Space))
            {
                playerBullet = new Bullets(5, playerBulletTexture, "up", player1.getPosition());
                bulletActive = true;
            }
        }
    }
}
