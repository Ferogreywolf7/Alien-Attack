using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Alien_Attack
{
    internal class mediumEnemy : Enemies
    {
        public mediumEnemy(Texture2D medEnemyTexture, SpriteBatch _spriteBatch, Vector2 startPos)
        {
            texture = medEnemyTexture;
            spriteBatch = _spriteBatch;
            originalPosition = startPos;
            position = originalPosition;
            moveType = "right";
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
        }

        public override void drawEnemy()
        {
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
