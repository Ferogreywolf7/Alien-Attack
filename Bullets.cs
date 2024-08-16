using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alien_Attack
{
    internal class Bullets
    {
        int bulletSpeed;
        Texture2D texture;
        string movementType;
        Vector2 position;
        Rectangle destiationRectangle;

        public Bullets(int speed, Texture2D bulletTexture, string moveType, Vector2 startPos) {
            bulletSpeed = speed;
            texture = bulletTexture;
            movementType = moveType;
            position = startPos;
        }

        public void updateBullets() {
            if (movementType == "up") {
                position.Y += bulletSpeed;
                }
        }

        public void drawBullet(SpriteBatch spriteBatch) {
            destiationRectangle = new Rectangle( (int)position.X, (int)position.Y, 50, 50);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destiationRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
