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
        private mediumEnemy enemy1;
        private const int enemySpacing = 60;
        private Texture2D enemyTexture;
        private SpriteBatch spriteBatch;
        private Vector2 startPos;
        private Vector2 currentPos;
        private List<mediumEnemy> enemies;
        private int numOfEnemies;
        private int rows;
        private int columns;
        private int num;
        private int count;
        private int collisionCount;
        private Rectangle enemyHitbox;
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
            
            if (num <= enemies.Count - 1) {
                enemies[num].updateEnemy();


                if (enemies[num].getPosition().X >= 740)
                {
                    moveAllDown();
                }
                if (enemies[num].getPosition().X <= 60)
                {
                    moveAllDown();
                }

                num++;
                updateAllEnemies();
                
            }
            else { num = 0; }
            
        }
        
        public void drawAllEnemies() {
            //Constantly calls the draw method for each enemy (Draws them on the screen)
                //Two enemies dont move for some reason
            //Recursion doesn't work here
            foreach(mediumEnemy enemy in enemies)
            {
                enemy.drawEnemy();
            }
            
        }
        public void deleteEnemy(int enemyNum)
        {
            enemies.RemoveAt(enemyNum);
        }

        public void moveAllDown() {
            
            if (count <= enemies.Count - 1)
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

            foreach (mediumEnemy enemy2 in enemies) {
                //Debug.WriteLine("Checking collision");
                enemyHitbox = enemy2.getHitbox();
                if (enemyHitbox.Intersects(bulletHitbox))
                {
                    //Debug.WriteLine("CollisionDetected");
                    deleteEnemy(collisionCount);
                    return true;
                }
                collisionCount++;
            }
            collisionCount = 0;
            return false;
            //Debug.WriteLine("false");
        }
    }
}
