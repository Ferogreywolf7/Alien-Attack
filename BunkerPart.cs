using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Alien_Attack
{
    internal class BunkerPart
    {
        private int destructionCount;
        private SpriteBatch spriteBatch;
        private Texture2D textureAtlas;
        private const int partWidth = 40;
        private const int partHeight = 40;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;
        private Vector2 position;
        private bool collision;

        public BunkerPart(Texture2D textureAtlas, Vector2 position) {
            this.textureAtlas = textureAtlas;
            this.position = position;
            destructionCount = 0;
                //Makes rectangle for where it will be drawn
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, partWidth, partHeight);
        }

        public void updateBunkerPart() {
                //Rectangle for what part of the texture will be used from the atlas
          sourceRectangle = new Rectangle(200 * destructionCount, 0, 200, 200);
        }

        public void drawBunkerPart(SpriteBatch _spriteBatch) {
            spriteBatch = _spriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(textureAtlas, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
            //Gets the amount of damage the part has taken so it can be destroyed if needed
        public int getDestructionCount() {
            return destructionCount;
        }
            //Gets the hitbox for the bunker part
        public Rectangle getBunkerHitbox() {
            return destinationRectangle;
        }
            //Damages the part of the bunker
        public void partHit() {
            destructionCount += 1;
        }
            //Checks if the hitbox has collided with this specific part of the bunker and if so, damages the part
        public bool bulletCollision(Rectangle hitbox) {
            collision = false;
            if (hitbox.Intersects(getBunkerHitbox())) {
                partHit();
                collision = true;
            }
            return collision;
        }
    }
}
