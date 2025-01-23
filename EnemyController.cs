using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Alien_Attack
{
    internal class EnemyController
    {
        private const int enemySpacing = 60;
        private Texture2D enemyTexture;
        private SpriteBatch spriteBatch;
        private Vector2 startPos;
        private Vector2 currentPos;
        private static List<mediumEnemy> enemies;
        private List<mediumEnemy> updateOnlyBullets;
        private int numOfEnemies;
        private int rows;
        private int columns;
        private int num;
        private int MoveDownCount;
        private int enemySpeedCount;
        private int BulletUpdateCount;
        private int BulletDrawCount;
        private int collisionCount;
        private Rectangle enemyHitbox;
        private bool isCollision;
        private string gameMode;
        private float currentSpeed;
        //Will only spawn in the regular enemies for now

        public EnemyController(SpriteBatch _spriteBatch, Texture2D texture, Vector2 startPos, string gameMode)
        {
            spriteBatch = _spriteBatch;
            this.startPos = startPos;
            this.gameMode = gameMode;
            currentPos = startPos;
            enemyTexture = texture;
            num = 0;
            MoveDownCount = 0;
            collisionCount = 0;
            enemies = new List<mediumEnemy>();
            updateOnlyBullets = new List<mediumEnemy>();
            currentSpeed = 0.5f;
        }

        public void spawnEnemies(int rows, int columns) {
            this.rows = rows;
            this.columns = columns;
            //To do: Make this into recurrsion instead
            //Spawns in all of the enemies in rows
            for (int row = 0; row <= rows; row++)
            {
                currentPos.Y += enemySpacing;
                for (int column = 0; column <= columns; column++)
                {
                    currentPos.X += enemySpacing;
                    enemies.Add(new mediumEnemy(enemyTexture, spriteBatch, currentPos, currentSpeed));
                    numOfEnemies++;
                }
                currentPos.X = startPos.X;
            }
        }
        
        public void updateAllEnemies()
        {
            //Constantly calls the update method for each enemy (moves them around)
            updateDeadBullets();
            if (num <= getNumberOfEnemies()) {
                enemies[num].updateEnemy();


                if (enemies[num].getPosition().X >= 740)
                {
                    moveAllDown();
                }
                if (enemies[num].getPosition().X <= 10)
                {
                    moveAllDown();
                    //currentPos = startPos;
                    //spawnEnemies(1, columns);
                }

                if (enemies[num].getPosition().Y >= 750) { 
                    //Game over here
                }

                num++;

                updateAllEnemies();
                
            }
            else { num = 0; }
            
        }
        
        public void drawAllEnemies() {
            //Calls drawing methods
            drawDeadBullets();
            foreach(mediumEnemy enemy in enemies)
            {
                enemy.drawEnemy();
                enemy.drawBullet();
            }
            
            
        }
        private void moveAllDown()
        {
            //Loops through all enemys, calling the method to move the enemy down
            if (MoveDownCount <= getNumberOfEnemies())
            {
                enemies[MoveDownCount].moveEnemyDown();
                MoveDownCount++;
                moveAllDown();
            }

            else
            {
                MoveDownCount = 0;
            }
        }

        public void deleteEnemy(int enemyNum)
            //Removes the enemy from the list, stopping them from being drawn and updated
        {
            increaseAllSpeed();
            if (enemies[enemyNum].isBulletAlive()) {
                updateOnlyBullets.Add(enemies[enemyNum]);
            }
            enemies.RemoveAt(enemyNum);
        }

        private void updateDeadBullets()
        {
            if (BulletUpdateCount < updateOnlyBullets.Count)
            {
                updateOnlyBullets[BulletUpdateCount].updateBullet();
                if (!updateOnlyBullets[BulletUpdateCount].isBulletAlive()) {
                    updateOnlyBullets.RemoveAt(BulletUpdateCount);
                }
                BulletUpdateCount++;
                updateDeadBullets();
            }
            
            else
            {
                BulletUpdateCount = 0;
            }
        }

        private void drawDeadBullets()
        {
            if (BulletDrawCount < updateOnlyBullets.Count)
            {
                updateOnlyBullets[BulletDrawCount].drawBullet();
                BulletDrawCount++;
                drawDeadBullets();
            }

            else
            {
                BulletDrawCount = 0;
            }
        }

        private void increaseAllSpeed() {
            if (enemySpeedCount <= getNumberOfEnemies())
            {
                enemies[enemySpeedCount].increaseSpeed();
                enemySpeedCount++;
                increaseAllSpeed();
            }

            else
            {
                currentSpeed += 0.03f;
                enemySpeedCount = 0;
            }
        }
        
        public bool checkCollision(Rectangle bulletHitbox) {
                //Loops through each enemy, getting their hitbox and checking if it is in the bullets hitbox, then deleting the enemy if so
            foreach (mediumEnemy enemy2 in enemies) {
                enemyHitbox = enemy2.getHitbox();
                if (enemyHitbox.Intersects(bulletHitbox))
                {
                    deleteEnemy(collisionCount);
                    return true;
                }
                collisionCount++;
            }
            collisionCount = 0;
            return false;
        }

        public bool checkPlayerCollision(Rectangle playerHitBox) {
                //Checks for the collision between enemy bullets and the player
            foreach (mediumEnemy enemy3 in enemies)
            {
                isCollision = enemy3.bulletCollision(playerHitBox);
                if (isCollision)
                {
                    return true;
                }
            }
            foreach (mediumEnemy enemy in updateOnlyBullets) {
                isCollision = enemy.bulletCollision(playerHitBox);
                if (isCollision)
                {
                    return true;
                }
            }
            return false;
        }

        public static int getNumberOfEnemies() { 
                return enemies.Count - 1;
        }
    }
}
