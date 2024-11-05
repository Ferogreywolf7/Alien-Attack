using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Net;

namespace Alien_Attack
{
    internal class Bullets
    {
        private int bulletSpeed;
        private Texture2D bulletTexture;
        private string movementType;
        private Vector2 position;
        private Rectangle destinationRectangle;
        private SpriteFont testFont;
        private SpriteBatch spriteBatch;
        private int extraXCoord;

        public Bullets(int speed, Texture2D texture, string moveType, Vector2 startPos, int extraX) {
            bulletSpeed = speed;
            bulletTexture = texture;
            movementType = moveType;
            position = startPos;
            extraXCoord = extraX;
        }

        public void updateBullets() {
            if (movementType == "up") {
                position.Y -= bulletSpeed;
            }
            if (movementType == "down") {
                position.Y += bulletSpeed;
            }
        }

        public void drawBullets(SpriteBatch _spriteBatch) {
            spriteBatch = _spriteBatch;
            destinationRectangle = new Rectangle((int)position.X+extraXCoord-5, (int)position.Y, 10, 30);
            spriteBatch.Begin();
            spriteBatch.Draw(bulletTexture, destinationRectangle, Color.White);
            spriteBatch.End();

        }

        public Rectangle getHitbox() {
            return destinationRectangle;
        }


        public Vector2 getBulletPos() {
            return position;
        }
    }
}
