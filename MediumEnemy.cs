﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Alien_Attack
{
    internal class mediumEnemy : Enemies
    {
        private Rectangle hitbox;
        private Texture2D bulletTexture;
        private Vector2 bulletSpawnPos;

        public mediumEnemy(Texture2D medEnemyTexture, SpriteBatch _spriteBatch, Vector2 startPos, float speed, Texture2D explosionTexture)
        {
            steps = speed;
            texture = medEnemyTexture;
            spriteBatch = _spriteBatch;
            originalPosition = startPos;
            position = originalPosition;
            moveType = "right";
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            explosionSpriteSheet = explosionTexture;
        }

        public override void drawEnemy()
        {
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, Color.Green);
            spriteBatch.End();
        }

        public override void randomBulletFire()
        {
            randomNum = rand.Next(1, 80+(EnemyController.getNumberOfEnemies()*20) - (UI.getLevel()*5));
            if (randomNum == 9) {
                bulletSpawnPos = getPosition();
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

        public bool bulletCollision(Rectangle Hitbox){
            hitbox = Hitbox;
            if (bulletSpawned && bullet.getHitbox().Intersects(hitbox))
            {
                bulletSpawned = false;
                return true;
            }
            return false;
        }
    }
}
