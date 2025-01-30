using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace Alien_Attack
{
    class Player
    {
        private int steps;
        private Vector2 position;
        private Texture2D texture;
        Rectangle destinationRectangle;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        private Keys moveLeft;
        private Keys moveRight;
        private Keys shoot;
        Controls controls;
        private int playerWidth;
        private string gameMode;
            //Bullet related variables
        private Bullets playerBullet;
        private Texture2D bulletTexture;
        private Rectangle bulletHitbox;
        private bool bulletActive;
        private bool bunkerHit;
        private int extraXCoord;
            //Cooldown related variables
        private bool timeElapsed;
        private double startTime;
        private double currentTime;
        private double bulletCooldown;
        private UI ui;

        public Player(Texture2D player1Texture, Texture2D bulletTexture, Vector2 player1StartPos, Controls control, UI ui) {
            this.ui = ui;
            steps = 5;
            position = player1StartPos;
            texture = player1Texture;
            this.bulletTexture = bulletTexture;
            bulletCooldown = 1.50;
            playerWidth = 90;
            controls = control;
            getControls();
            timeElapsed = true;
        }

        public void updatePlayer(KeyboardState currentKeyState, KeyboardState previousKeyState) {
            currentKeyboardState = currentKeyState;
            previousKeyboardState = previousKeyState;
            movePlayer();
            enforceWalls();
            firePlayerBullet();
            if (bulletActive) {
                playerBullet.updateBullets();
            }
            if (!timeElapsed) {
                checkCooldown();
            }
        }

        
        public void drawPlayer(SpriteBatch _spriteBatch) {
            spriteBatch = _spriteBatch;
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 90, playerWidth);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.End();
            if (bulletActive)
            {
                playerBullet.drawBullets(_spriteBatch);
            }
        }

        public Vector2 getPosition() {
            return position;
        }

        public Rectangle getPlayerHitbox() {
            return destinationRectangle;
        }

        public int getPLayerWidth() {
            return playerWidth;
        }

        public void getControls()
        {
            moveLeft = controls.getLeft();
            moveRight = controls.getRight();
            shoot = controls.getFire();
        }

        //checks to see if user pressed the correct key and then calls the method to move the player in the right direction
        private void movePlayer() {
            if (currentKeyboardState.IsKeyDown(moveLeft)    ) {
                movePlayerLeft();
            }

            if (currentKeyboardState.IsKeyDown(moveRight)) {
                movePlayerRight();
            }
        }

        //Changes the X coordinate that the player sprite should be drawn at
        private void movePlayerLeft() {
            position.X -= steps;
        }

        private void movePlayerRight() {
            position.X += steps;
        }

        //Stops player from being able to go off screen
        private void enforceWalls() {
            if (position.X <= 0) {
                position.X = 0;
            }
            else if(position.X >= 730){
                position.X = 730;
            }
        }

        //Player bullet related functions
        private void firePlayerBullet()
        {
            //Bullet will only be fired when there is no other bullet on screen and the player has pressed the key for firing
            if (currentKeyboardState.IsKeyDown(shoot)  && timeElapsed)
            {
                extraXCoord = getPLayerWidth() / 2;
                playerBullet = new Bullets(5, bulletTexture, "up", getPosition(), extraXCoord);
                bulletActive = true;
                timeElapsed = false;
                startCooldown();
            }
        }

        private void checkCooldown() {
            currentTime = Convert.ToDouble(ui.getStopwatchTime().Replace(":",""));
            if ((currentTime - startTime) >= bulletCooldown) {
                Debug.WriteLine("Cooldown finished");
                timeElapsed = true;
            }
        }

        private void startCooldown() {
            startTime = Convert.ToDouble(ui.getStopwatchTime().Replace(":", ""));
        }

        public bool isBulletActive() {
            return bulletActive;
        }

        public void checkplayerBulletCollision(EnemyController enemies, Bunkers bunkers)
        {
            bulletHitbox = playerBullet.getHitbox();
            bulletActive = !enemies.checkCollision(bulletHitbox);
            if (playerBullet.getBulletPos().Y <= -60)
            {
                bulletActive = false;
            }
            //Checks if the payers bullet hits the bunker
            bunkerHit = bunkers.checkBunkerCollision(bulletHitbox);
            if (bunkerHit)
            {
                bulletActive = false;
            }
        }

        //Stops all bullet related functions being called for the player's bullet
        private void deactivateBullet()
        {
            bulletActive = false;
        }


    }
}
