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
        private Texture2D bunker;
        private int bunkerAmount;
        private int drawnBunkers;
        private int screenWidth;
        private SpriteBatch spriteBatch;
        private Game1 game1;
        private Rectangle destinationRectangle;

        public Bunkers(Texture2D Bunker, int amountOfBunkers) {
            /*game1 = new Game1();
            spriteBatch = game1.getSpriteBatch();*/
            bunker = Bunker;
            bunkerAmount = amountOfBunkers;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            drawnBunkers = 0;
        }

        public void drawBunkers(SpriteBatch _spriteBatch) {
            spriteBatch = _spriteBatch;
            if (drawnBunkers != bunkerAmount)
            {
                destinationRectangle = new Rectangle(50 * drawnBunkers+1, 500, 100, 100);
                spriteBatch.Begin();
                spriteBatch.Draw(bunker, destinationRectangle, Color.White);
                spriteBatch.End();
                drawnBunkers++;
                drawBunkers(spriteBatch);
            }
        }
    }
}
