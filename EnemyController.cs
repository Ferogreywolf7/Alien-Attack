using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private Texture2D explosion;
        private Texture2D bulletTexture;
        private SpriteBatch spriteBatch;
        private Vector2 startPos;
        private Vector2 currentPos;
        private static List<mediumEnemy> enemies;
        private int numOfEnemies;
        private int rows;
        private int columns;
        private int num;
        private int MoveDownCount;
        private int enemySpeedCount;
        private int collisionCount;
        private int lowestCount;
        private int lowestCoord;
        private int timesSpawnedEnemies;
        private Rectangle enemyHitbox;
        private bool isCollision;
        private bool spawnedOnThisTurn;
        private string gameMode;
        private float currentSpeed;
        private int level;
        private int xPos;

        //Will only spawn in the regular enemies for now

        public EnemyController(SpriteBatch _spriteBatch, Texture2D texture, Vector2 startPos, string gameMode, Texture2D explosionSheet, Texture2D enemyBulletTexture)
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
            currentSpeed = 0.5f + level/5;
            lowestCoord = 0;
            spawnedOnThisTurn = true;
            timesSpawnedEnemies = 1;
            explosion = explosionSheet;
            bulletTexture = enemyBulletTexture;
        }

        public void spawnEnemies(int rows, int columns) {
            level = UI.getLevel();
            this.rows = rows;
            this.columns = (int) columns;
            this.columns += level / 4;
            //To do: Make this into recurrsion instead
            //Spawns in all of the enemies in rows
            for (int row = 0; row <= rows; row++)
            {
                currentPos.Y += enemySpacing;
                for (int column = 0; column <= this.columns; column++)
                {
                    currentPos.X += enemySpacing;
                    enemies.Add(new mediumEnemy(enemyTexture, spriteBatch, currentPos, currentSpeed, explosion, bulletTexture));
                    numOfEnemies++;
                }
                currentPos.X = startPos.X;
            }
        }
        
        public void updateAllEnemies()
        {
            //Constantly calls the update method for each enemy (moves them around)
            //updateDeadBullets();
            checkIfDelete();
            if (num <= getNumberOfEnemies()) {
                enemies[num].updateEnemy();

                xPos = (int) enemies[num].getPosition().X;
                if (xPos >= 740)
                {
                    moveAllDown();
                }
                if (xPos <= 10)
                {
                    spawnedOnThisTurn = false;
                    moveAllDown();
                }
                //When the enemies reach the left side of the screen, more enemies will spawn in the endless gamemode above the coordinates of the top left enemy
                if (xPos >= 60 && gameMode == "Endless" && !spawnedOnThisTurn)
                {
                    currentPos.X = startPos.X - enemySpacing;
                    currentPos.Y = startPos.Y - enemySpacing;//*timesSpawnedEnemies;
                    spawnEnemies(0, columns);
                    spawnedOnThisTurn = true;
                    timesSpawnedEnemies++;
                }
                    num++;

                    updateAllEnemies();
                
            }
            else { num = 0; }
            
        }
        
        public void drawAllEnemies() {
            //Calls drawing methods
            foreach (mediumEnemy enemy in enemies)
            {
                if (!enemy.isEnemyDead())
                {
                    enemy.drawEnemy();
                }
                else {
                    enemy.explode();
                }
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

        public int getLowestEnemy() {

            if (lowestCount <= getNumberOfEnemies())
            {
                if (lowestCoord < (int)enemies[lowestCount].getPosition().Y)
                {
                    lowestCoord = (int)enemies[lowestCount].getPosition().Y;
                }
                lowestCount++;
                getLowestEnemy();
            }
            else {
                lowestCount = 0;
            }
            return lowestCoord;
        }

            //Keeps the enemy alive if the bullet hasn't been destroyed yet or explosion animation cycle hasn't completed
        public void checkIfDelete()
        {
            foreach (mediumEnemy enemy in enemies.ToList())
            {
                if (enemy.isEnemyDead() && !enemy.isBulletAlive())
                {
                    if (enemy.getExploded())
                    {
                        deleteEnemy(collisionCount);
                    }
                }
                
                collisionCount++;
            }
            collisionCount = 0;
        }

        public void deleteEnemy(int enemyNum)
            //Removes the enemy from the list, stopping them from being drawn and updated
        {
            increaseAllSpeed();
            enemies.RemoveAt(enemyNum);
            UI.increaseScore(100);
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
                currentSpeed += 0.04f;
                enemySpeedCount = 0;
            }
        }
        
        public bool checkCollision(Rectangle bulletHitbox) {
            //Loops through each enemy, getting their hitbox and checking if it is in the bullets hitbox, then deleting the enemy if so
            foreach (mediumEnemy enemy2 in enemies)
            {
                
                if (enemy2.checkCollision(bulletHitbox) && !enemy2.isEnemyDead())
                {
                    enemy2.killEnemy();
                    return true;
                }
            }
            return false;
        }

        public bool checkBulletCollision(Rectangle playerHitBox) {
                //Checks for the collision between enemy bullets and the player
            foreach (mediumEnemy enemy3 in enemies)
            {
                isCollision = enemy3.bulletCollision(playerHitBox);
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
