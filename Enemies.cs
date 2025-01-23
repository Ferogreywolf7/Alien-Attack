using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


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
        protected SpriteBatch spriteBatch;
        protected Rectangle destinationRectangle;
        protected Random rand;
        protected int randomNum;
        protected Bullets bullet;
        protected int drawCount;
        protected int updateCount;
        protected bool bulletSpawned;
        public Enemies() {
            rand = new Random();
        }

        public Vector2 getPosition() {
            return position;
        }

        public void updateEnemy() {
            moveEnemy();
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

        public Rectangle getHitbox()
        {
            return destinationRectangle;
        }
        

        public void moveEnemy() {
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

        

    }
}
