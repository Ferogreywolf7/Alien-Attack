using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;


namespace Alien_Attack
{
    abstract class Enemies
    {
        //Instance variables
        protected Vector2 position;
        protected Vector2 originalPosition;
        protected KeyboardState currentKeyboardState;
        protected KeyboardState oldKeyboardState;
        protected string moveType;
        protected float steps;
        protected Texture2D texture;
        protected Texture2D explosionSpriteSheet;
        protected SpriteBatch spriteBatch;
        protected Rectangle destinationRectangle;
        protected Random rand;
        protected int randomNum;
        protected Bullets bullet;
        protected int drawCount;
        protected int updateCount;
        protected bool bulletSpawned;
        protected bool isDead;
        protected bool exploded;
        private int counter;
        private int topLeft;
        private Rectangle explosionDestinationRectangle;
        private Rectangle explosionSourceRectangle;
        public Enemies() {
            rand = new Random();
            topLeft = 0;
        }

        public Vector2 getPosition() {
            return position;
        }

        public void updateEnemy() {
            if (!isEnemyDead())
            {
                moveEnemy();
            }
            if (bulletSpawned == true)
            {
                updateBullet();
            }
            else { randomBulletFire(); }
        }

        public abstract void drawEnemy();

        public abstract void updateBullet();

        public abstract void randomBulletFire();

        public bool isBulletAlive() {
            return bulletSpawned;
        }

        public bool isEnemyDead() {
            return isDead;
        }

        public void killEnemy() {
            isDead = true;
        }

        public Rectangle getHitbox()
        {
            return destinationRectangle;
        }
        

        protected void moveEnemy() {
            switch (moveType){
                case "left":
                    position.X -= steps;
                    
                    break;
                case "right":
                    position.X += steps;
                    break;
            }
        }

        public void moveEnemyDown() {
            //Implemented like this to easily move all enemies down at once
            position.Y += 60;
            switch (moveType)
            {
                case "left":
                    moveType = "right";
                    break;
                case "right":
                    moveType = "left";
                    break;
            } 
        }
            //Enemies get slightly faster when you kill them
        public void increaseSpeed() {
            steps += (float) 0.03;
        }

        public abstract void drawBullet();


        public bool checkCollision(Rectangle bulletHitbox) {
            if (destinationRectangle.Intersects(bulletHitbox))
            {
                return true;
            }
            return false;
        }

        //Explosion mechanics for all enemies

        public void explode() {
            if (!exploded)
            {
                if (topLeft >= 158 * 10)
                {
                    topLeft = 0;
                    exploded = true;
                }
                    //runs explosion animation
                else
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(explosionSpriteSheet, explosionDestinationRectangle, explosionSourceRectangle, Color.White);
                    spriteBatch.End();
                    explosionSourceRectangle = new Rectangle(topLeft, 0, 158, 145);
                    explosionDestinationRectangle = new Rectangle((int)position.X - 10, (int)position.Y - 10, 75, 75);
                    counter++;
                    if (counter == 5)
                    {
                        topLeft += 158;
                        counter = 0;
                    }
                }
            }
        }

        public bool getExploded() {
            return exploded;
        }
    }
}
