using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Alien_Attack
{
    internal class Bunkers
    {
        private Texture2D bunkerAtlas;
        private int bunkerAmount;
        private int screenWidth;
        private int pieceCount;
        private int bunkerNo;
        private const int pieceHeight = 30;
        private const int pieceWidth = 30;
        private SpriteBatch spriteBatch;
        private Rectangle bulletHitbox;
        private List<BunkerPart> bunkerPieces;
        private Vector2 piecePosition;
        private bool isBunkerCollision;
        private int level;
        public Bunkers(Texture2D bunkerTexture, int NoOfBunkers)
        {
            bunkerAtlas = bunkerTexture;
            bunkerAmount = NoOfBunkers;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            pieceCount = 0;
            bunkerNo = 1;
            bunkerPieces = new List<BunkerPart>();
            createBunkers();
        }

        public void updateBunkers() {
            if (pieceCount < bunkerPieces.Count()) {
                if (bunkerPieces[pieceCount].getDestructionCount() == 4) {
                    removeBunkerPiece(pieceCount);
                }
                pieceCount += 1;
                updateBunkers();
            }
            foreach (BunkerPart part in bunkerPieces)
            {
                part.updateBunkerPart();
            }
            pieceCount = 0;
        }

        public void drawBunkers(SpriteBatch _spriteBatch)
        {
            spriteBatch = _spriteBatch;
            foreach (BunkerPart part in bunkerPieces) {
                part.drawBunkerPart(spriteBatch);
            }
            
        }
        private void createBunkers() {
                //Placing parts of the bunker in certain positions
            level = UI.getLevel();
            bunkerAmount -= level/5;
            if (bunkerNo <= bunkerAmount)
            {
                piecePosition = new Vector2(((screenWidth + 50) / (bunkerAmount * 10)) * bunkerNo*2, 700);
                bunkerPieces.Add(new BunkerPart(bunkerAtlas, piecePosition));

                piecePosition = new Vector2(((screenWidth + 50) / (bunkerAmount * 10)) * bunkerNo*2, 700 - pieceHeight);
                bunkerPieces.Add(new BunkerPart(bunkerAtlas, piecePosition));

                piecePosition = new Vector2(((screenWidth + 50) / (bunkerAmount * 10)) * bunkerNo*2 + pieceWidth, 700 - pieceHeight * 2);
                bunkerPieces.Add(new BunkerPart(bunkerAtlas, piecePosition));

                piecePosition = new Vector2(((screenWidth + 50) / (bunkerAmount * 10)) * bunkerNo*2 + pieceWidth * 2, 700 - pieceHeight * 2);
                bunkerPieces.Add(new BunkerPart(bunkerAtlas, piecePosition));

                piecePosition = new Vector2(((screenWidth + 50) / (bunkerAmount * 10)) * bunkerNo*2 + pieceWidth * 3, 700 - pieceHeight);
                bunkerPieces.Add(new BunkerPart(bunkerAtlas, piecePosition));
                
                piecePosition = new Vector2(((screenWidth + 50) / (bunkerAmount * 10)) * bunkerNo*2 + pieceWidth * 3, 700);
                bunkerPieces.Add(new BunkerPart(bunkerAtlas, piecePosition));

                bunkerNo += 1;
                createBunkers();
            }
            bunkerNo = 1;
        }

        private void removeBunkerPiece(int pieceNum) {
            bunkerPieces.RemoveAt(pieceNum);
        }

        public List<BunkerPart> GetBunkerParts() {
            return bunkerPieces;
        }
            
        public bool checkBunkerCollision(Rectangle hitbox) {
                //Check if the bullets hitbox has hit a part of the bunker and returns true if so so that the bullet can be deleted
            bulletHitbox = hitbox;
            foreach (BunkerPart part in bunkerPieces) {
                isBunkerCollision = part.bulletCollision(bulletHitbox);
                if (isBunkerCollision) {
                    return true;
                }
            }
            return false;
        }
    }
}
