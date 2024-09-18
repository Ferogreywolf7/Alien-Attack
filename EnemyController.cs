using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Alien_Attack.Game1;

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
        private List<mediumEnemy> enemies = new List<mediumEnemy>();
        private int numOfEnemies;
        private int rows;
        private int columns;
        private int num = 0;
        //Will only spawn in the regular enemies for now

        public EnemyController(SpriteBatch _spriteBatch, Texture2D texture, int rows, int columns, Vector2 startPos)
        {
            spriteBatch = _spriteBatch;
            this.rows = rows;
            this.columns = columns;
            this.startPos = startPos;
            enemyTexture = texture;
            
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
            num++;
            if (num <= enemies.Count - 1) {
                enemies[num].updateEnemy();
                updateAllEnemies();
            }
            else{ num = 0; }
        }

        public void drawAllEnemies() {
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
    }
}
