using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

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
        controlsMenu controls;

        public Player(Texture2D player1Texture, Vector2 player1StartPos) {
            steps = 5;
            position = player1StartPos;
            texture = player1Texture;
            getControls();
        }

        public void playerUpdate(KeyboardState currentKeyState, KeyboardState previousKeyState) {
            currentKeyboardState = currentKeyState;
            previousKeyboardState = previousKeyState;
            movePlayer();
        }

        
        public void DrawPlayer(SpriteBatch _spriteBatch) {
            //
            spriteBatch = _spriteBatch;
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 100, 100);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.End();
        }

        public Vector2 getPosition() {
            return position;
        }

        public void getControls()
        {
            controls = new controlsMenu();
            moveLeft = controls.getLeft();
            moveRight = controls.getRight();
        }

        //checks to see if user pressed the correct key and then calls the method to move the player in the right direction
        public void movePlayer() {
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
