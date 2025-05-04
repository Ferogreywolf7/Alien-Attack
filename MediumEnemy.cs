using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Alien_Attack
{
    internal class mediumEnemy : Enemies
    {
        private Rectangle hitBox;
        private Texture2D bulletTexture;
        private Vector2 bulletSpawnPos;
        private Rectangle sourceRectangle;
        private int counter;
        private int topLeft;

        public mediumEnemy(Texture2D medEnemyTexture, SpriteBatch spriteBatch, Vector2 startPos, float speed, Texture2D explosionTexture, Texture2D bulletTexture)
        {
            steps = speed;
            texture = medEnemyTexture;
            this.spriteBatch = spriteBatch;
            originalPosition = startPos;
            position = originalPosition;
            moveType = "right";
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 60, 60);
            explosionSpriteSheet = explosionTexture;
            topLeft = 0;
            this.bulletTexture = bulletTexture;
        }

            //Method for drawing an animated version of the enemy sprite    
        public override void drawEnemy()
        {
            if (counter % 10 == 0) {
                topLeft += 188;
            }
            if (topLeft > 721) {
                topLeft = 0;
            }
            sourceRectangle = new Rectangle(topLeft, 0, 160, 160);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
            counter++;
        }

          //Method for having a random chance to fire a bullet which changes based on the level and number of enemies
        public override void randomBulletFire()
        {
            randomNum = rand.Next(1, 80+(EnemyController.getNumberOfEnemies()*20) - (UI.getLevel()*5));
            if (randomNum == 9) {
                bulletSpawnPos = getPosition();
                bullet = new Bullets(5, bulletTexture, "down", bulletSpawnPos, 0);
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

        public bool bulletCollision(Rectangle hitbox){
            hitBox = hitbox;
            if (bulletSpawned && bullet.getHitbox().Intersects(hitBox))
            {
                bulletSpawned = false;
                return true;
            }
            return false;
        }
    }
}