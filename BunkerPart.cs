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

        public int getDestructionCount() {
            return destructionCount;
        }

        public Rectangle getBunkerHitbox() {
            return destinationRectangle;
        }

        public void partHit() {
            destructionCount += 1;
            Debug.WriteLine(sourceRectangle);
        }
    }
}
