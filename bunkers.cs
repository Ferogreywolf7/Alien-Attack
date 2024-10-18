using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Implement pixel based collision and delete pixels that are touched + surrounding?

namespace Alien_Attack
{
    internal class Bunkers
    {
        private Texture2D bunkerAtlas;
        private int bunkerAmount;
        private int drawnBunkers;
        private int screenWidth;
        private SpriteBatch spriteBatch;
        private Game1 game1;
        private Rectangle destinationRectangle;
        private List<BunkerPart> bunkerPieces;
        private Vector2 piecePosition;

        public Bunkers(Texture2D bunkerTexture, int amountOfBunkers)
        {
            bunkerAtlas = bunkerTexture;
            bunkerAmount = amountOfBunkers;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            drawnBunkers = 0;
            bunkerPieces = new List<BunkerPart>();
        }

        public void drawBunkers(SpriteBatch _spriteBatch)
        {
            spriteBatch = _spriteBatch;
            if (drawnBunkers != bunkerAmount)
            {
                bunkerPieces.Add(new BunkerPart(bunkerAtlas, piecePosition));
                drawBunkers(spriteBatch);
            }
            drawnBunkers = 0;
        }
    }
}
