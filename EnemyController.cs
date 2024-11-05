using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int numOfEnemies;
        private int rows;
        private int columns;
        private int num;
        private int count;
        private int collisionCount;
        private Rectangle enemyHitbox;
        private bool isPlayerCollision;
        //Will only spawn in the regular enemies for now

        public EnemyController(SpriteBatch _spriteBatch, Texture2D texture, int rows, int columns, Vector2 startPos)
        {
            spriteBatch = _spriteBatch;
            this.rows = rows;
            this.columns = columns;
            this.startPos = startPos;
            currentPos = startPos;
            enemyTexture = texture;
            num = 0;
            count = 0;
            collisionCount = 0;
            enemies = new List<mediumEnemy>();
        }

        public void spawnEnemies() {
            //To do: Make this into recurrsion instead
                //Spawns in all of the enemies in rows
            for (int row = 0; row <= rows; row++)
            {
                currentPos.Y += enemySpacing;
                for (int column = 0; column <= columns; column++)
                {
                    currentPos.X += enemySpacing;
                    enemies.Add(new mediumEnemy(enemyTexture, spriteBatch, currentPos));
                    numOfEnemies++;
                }
                currentPos.X = startPos.X;
            }
        }
        
        public void updateAllEnemies()
        {
            //Constantly calls the update method for each enemy (moves them around)
            
            if (num <= getNumberOfEnemies()) {
                enemies[num].updateEnemy();


                if (enemies[num].getPosition().X >= 740)
                {
                    moveAllDown();
                }
                if (enemies[num].getPosition().X <= 10)
                {
                    moveAllDown();
                }

                if (enemies[num].getPosition().Y >= 750) { 
                    //Game over here
                }

                num++;

                if (getNumberOfEnemies() == -1) { 
                    //Victory
                }
                updateAllEnemies();
                
            }
            else { num = 0; }
            
        }
        
        public void drawAllEnemies() {
                //Calls drawing methods
            foreach(mediumEnemy enemy in enemies)
            {
                enemy.drawEnemy();
                enemy.drawBullet();
            }
            
            
        }

        public void deleteEnemy(int enemyNum)
            //Removes the enemy from the list, stopping them from being drawn and updated
        {
            enemies.RemoveAt(enemyNum);
        }

        public void moveAllDown() {
                //Loops through all enemys, calling the method to move the enemy down
            if (count <= getNumberOfEnemies())
            {
                enemies[count].moveEnemyDown();
                count++;
                moveAllDown();
            }
            
            else { 
                count = 0;
            }
            
        }
        
        public bool checkCollision(Rectangle bulletHitbox) {
                //Recursion attempt
            /*if (collisionCount <= enemies.Count - 1)
            {
                Debug.WriteLine("Checking collision");
                enemyHitbox = enemies[collisionCount].getHitbox();
                if (enemyHitbox.Intersects(bulletHitbox)) {
                    Debug.WriteLine("CollisionDetected");
                    deleteEnemy(collisionCount);
                    return true;
                }
                collisionCount++;
                checkCollision(bulletHitbox);

            }
            return false;*/
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
                isPlayerCollision = enemy3.bulletCollision(playerHitBox);
                if (isPlayerCollision) {
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
