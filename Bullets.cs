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
        SpriteFont testFont;
        SpriteBatch spriteBatch;

        public Bullets(int speed, Texture2D bulletTexture, string moveType, Vector2 startPos, SpriteFont font) {
            bulletSpeed = speed;
            texture = bulletTexture;
            movementType = moveType;
            position = startPos;
            testFont = font;

        }

        public void updateBullets() {
            if (movementType == "up") {
                position.Y += bulletSpeed;
                }
        }

        public void drawBullet(SpriteBatch _spriteBatch) {
            spriteBatch = _spriteBatch;
            destiationRectangle = new Rectangle( (int)position.X, (int)position.Y, 70, 50);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, destiationRectangle, Color.White);
            spriteBatch.End();
            
        }
    }
}
