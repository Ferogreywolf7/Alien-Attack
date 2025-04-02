using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alien_Attack
{
    class Player
    {
        private int steps;
        private Vector2 position;
        Rectangle destinationRectangle;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyboardState;

        private Keys moveLeft;
        private Keys moveRight;
        private Keys shoot;
        Controls controls;
        private int playerWidth;

            //Bullet related variables
        private List<Bullets> bullets;
        private Texture2D bulletTexture;
        private Rectangle bulletHitbox;
        private bool bunkerHit;
        private int extraXCoord;

            //Cooldown related variables
        private bool timeElapsed;
        private double startTime;
        private double bulletCooldown;
        private UI ui;

        private static Texture2D playerAnimated;
        private int topLeft;
        private int counter;

        private Vector2 barPosition;
        private bool startReload;
        private Texture2D reloadBarTexture;
        private Texture2D reloadBorderTexture;
        private int barWidth;
        private int counter2;

        public Player(Texture2D playerSpriteSheet, Texture2D bulletTexture, Texture2D reloadBarTexture, Texture2D reloadBorderTexture, Vector2 player1StartPos, Controls control, UI ui) {
            this.ui = ui;
            steps = 5;
            position = player1StartPos;
            this.bulletTexture = bulletTexture;
            bulletCooldown = 1.5;
            bullets = new List<Bullets> { };
            playerWidth = 90;
            controls = control;
            getControls();
            timeElapsed = true;
            topLeft = 0;
            counter = 0;
            counter2 = 0;
            playerAnimated = playerSpriteSheet;
            this.reloadBarTexture = reloadBarTexture;
            this.reloadBorderTexture = reloadBorderTexture;
            barPosition = new Vector2(10, 800);
        }

        public void updatePlayer(KeyboardState currentKeyState, KeyboardState previousKeyState) {
            currentKeyboardState = currentKeyState;
            movePlayer();
            enforceWalls();
            firePlayerBullet();
            foreach(Bullets bullet in bullets){
                bullet.updateBullets();
            }
            if (!timeElapsed) {
                checkCooldown();
            }
        }

        
        public void drawPlayer(SpriteBatch _spriteBatch) {
            
            spriteBatch = _spriteBatch;
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 90, playerWidth);
            Rectangle sourceRectangle = new Rectangle(topLeft, 0, 250, 330);
            spriteBatch.Begin();
            //spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.Draw(playerAnimated, destinationRectangle, sourceRectangle, Color.White);
            playerReloadBar();
            spriteBatch.End();


            //.ToList is used to make a unique and unmodifiable copy of the bullets list to prevent changes during the foreach loop
            foreach (Bullets bullet in bullets.ToList())
            {
                bullet.drawBullets(_spriteBatch);
            }

                //Code for switching animation states for the player by moving the coordinates responsible for grabbing the image off of the sprite sheet
            counter++;
            if (counter == 10)
            {
                topLeft += 330;
                counter = 0;
            }
            if (topLeft >= 330 * 4) {
                topLeft = 0;
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
            else if(position.X >= 710){
                position.X = 710;
            }
        }

        //Player bullet related functions
        private void firePlayerBullet()
        {
            //Bullet will only be fired when there is no other bullet on screen and the player has pressed t\he key for firing
            if (currentKeyboardState.IsKeyDown(shoot)  && timeElapsed)
            {
                extraXCoord = getPLayerWidth() / 2;
                bullets.Add(new Bullets(6, bulletTexture, "up", getPosition(), extraXCoord));
                timeElapsed = false;
                startCooldown();
            }
        }

        private void checkCooldown() {
            timeElapsed = ui.checkCooldown(startTime, bulletCooldown);
        }

        private void startCooldown() {
            startTime = Convert.ToDouble(ui.getStopwatchTime().Replace(":", ""));
        }

        public void checkplayerBulletCollision(EnemyController enemies, Bunkers bunkers)
        {

            foreach (Bullets bullet in bullets.ToList())
            {
                bulletHitbox = bullet.getHitbox();
                if (enemies.checkCollision(bulletHitbox)){
                    bullets.Remove(bullet);
                }
                if (bullet.getBulletPos().Y <= -60)
                {
                    bullets.Remove(bullet);
                }

                //Checks if the payers bullet hits the bunker
                bunkerHit = bunkers.checkBunkerCollision(bulletHitbox);
                if (bunkerHit)
                {
                    bullets.Remove(bullet);
                }
            }
        }

        private void playerReloadBar() {
            barPosition = position;
            if (!timeElapsed && startReload)
            {
                barWidth = 0;
                startReload = false;
            }
            else if (!timeElapsed)
            {
                if (counter2 % 2 == 0)
                {
                    barWidth++;
                }
                counter2++;

                spriteBatch.Draw(reloadBarTexture, new Rectangle((int)barPosition.X + playerWidth / 4, (int)barPosition.Y + 102, barWidth, 15), Color.White);
                spriteBatch.Draw(reloadBorderTexture, new Rectangle((int)barPosition.X + playerWidth / 4, (int)barPosition.Y + 100, 50, 20), Color.White);
            }
            else
            {
                startReload = true;
            }
        }
    }
}
