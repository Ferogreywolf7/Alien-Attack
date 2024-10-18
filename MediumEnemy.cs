using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Alien_Attack
{
    internal class mediumEnemy : Enemies
    {
        private Rectangle playerHitbox;
        private Texture2D bulletTexture;
        public mediumEnemy(Texture2D medEnemyTexture, SpriteBatch _spriteBatch, Vector2 startPos)
        {
            texture = medEnemyTexture;
            spriteBatch = _spriteBatch;
            originalPosition = startPos;
            position = originalPosition;
            moveType = "right";
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            bulletTexture = TextureDirectory.getEnemyBulletTexture();
        }

        public override void drawEnemy()
        {
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.End();
        }

        public override void randomBulletFire()
        {
            randomNum = rand.Next(1, 1000-(EnemyController.getNumberOfEnemies()*20));
            if (randomNum == 99) {
                Vector2 bulletSpawnPos = getPosition();
                bullet = new Bullets(5, texture, "down", bulletSpawnPos, 0);
                bulletSpawned = true;
            }
        }

        public override void updateBullet()
        {
            if (bulletSpawned)
            {
                bullet.updateBullets();
            }

            if (bullet.getBulletPos().Y >= 1000)
            {
                bulletSpawned = false;
            }

        }

        public override void drawBullet() {
            if (bulletSpawned)
            {
                bullet.drawBullets(spriteBatch);
            }
        }

        public bool bulletCollision(Rectangle playersHitbox){
            playerHitbox = playersHitbox;
                //Deletes bullet if bulllet collides with player
            if (bulletSpawned && bullet.getHitbox().Intersects(playerHitbox))
            {
                bulletSpawned = false;
                return true;
            }
            return false;
        }
    }
}
