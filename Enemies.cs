using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Alien_Attack
{
    abstract class Enemies
    {
        //Instance variables
        protected Vector2 position;
        protected Vector2 originalPosition;
        protected KeyboardState currentKeyboardState;
        protected KeyboardState oldKeyboardState;
        protected string moveType;
        protected float steps;
        protected Texture2D texture;
        protected SpriteBatch spriteBatch;
        protected Rectangle destinationRectangle;
        public Enemies() {
            steps = 0.5f;
        }

        public Vector2 getPosition() {
            return position;
        }

        public void updateEnemy() {
            moveEnemy();
        }
        public abstract void drawEnemy();

        public void moveEnemy() {
            switch (moveType){
                case "left":
                    position.X -= steps;
                    
                    break;
                case "right":
                    position.X += steps;
                    break;
            }


        }
    }
}
