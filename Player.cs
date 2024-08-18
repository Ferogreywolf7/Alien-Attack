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
        int steps;
        private Vector2 position;
        Texture2D texture;
        Rectangle destinationRectangle;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        Keys moveLeft = Keys.A;

        public Player(Texture2D player1Texture, Vector2 player1StartPos) {
            steps = 5;
            position = player1StartPos;
            texture = player1Texture;
        }

        public void playerUpdate(KeyboardState currentKeyState, KeyboardState previousKeyState) {
            currentKeyboardState = currentKeyState;
            previousKeyboardState = previousKeyState;
            movePlayer();
        }

        public void DrawPlayer(SpriteBatch _spriteBatch) {
            spriteBatch = _spriteBatch;
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 100, 100);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, Color.White);
            spriteBatch.End();
        }

        public Vector2 getPosition() {
            return position;
        }

        public void movePlayer() {
            if (currentKeyboardState.IsKeyDown(moveLeft)) {
                movePlayerLeft();
            }

            if (currentKeyboardState.IsKeyDown(Keys.D)) {
                movePlayerRight();
            }
        }

        private void movePlayerLeft() {
            position.X -= steps;
        }

        private void movePlayerRight() {
            position.X += steps;
        }
        

    }
}
