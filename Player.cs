using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace Alien_Attack
{
    class Player
    {
        private int steps;
        private Vector2 position;
        private Texture2D texture;
        Rectangle destinationRectangle;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        private Keys moveLeft;
        private Keys moveRight;
        Controls controls;
        private int playerWidth;

        public Player(Texture2D player1Texture, Vector2 player1StartPos, Controls control) {
            steps = 5;
            position = player1StartPos;
            texture = player1Texture;
            playerWidth = 70;
            controls = control;
            getControls();
        }

        public void updatePlayer(KeyboardState currentKeyState, KeyboardState previousKeyState) {
            currentKeyboardState = Game1.getKayboardState();
            previousKeyboardState = previousKeyState;
            movePlayer();
        }

        
        public void drawPlayer(SpriteBatch _spriteBatch) {
            spriteBatch = _spriteBatch;
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 70, playerWidth);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.End();
        }

        public Vector2 getPosition() {
            return position;
        }

        public Rectangle getPlayerHitbox() {
            return destinationRectangle;
        }

        public int getPLayerWidth() {
            return playerWidth;
        }

        public void getControls()
        {
            moveLeft = controls.getLeft();
            moveRight = controls.getRight();
        }

        //checks to see if user pressed the correct key and then calls the method to move the player in the right direction
        private void movePlayer() {
            if (currentKeyboardState.IsKeyDown(moveLeft)) {
                movePlayerLeft();
            }

            if (currentKeyboardState.IsKeyDown(moveRight)) {
                movePlayerRight();
            }
        }

        //Changes the X coordinate that the player sprite should be drawn at
        private void movePlayerLeft() {
            position.X -= steps;
        }

        private void movePlayerRight() {
            position.X += steps;
        }
        


    }
}
