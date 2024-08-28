using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Alien_Attack
{
    abstract class Enemies
    {
        //Instance variables
        protected Vector2 position = new Vector2(0, 50);
        protected KeyboardState currentKeyboardState;
        protected KeyboardState oldKeyboardState;
        public Enemies() {
        }

        public Vector2 getPosition() {
            return position;
        }

        public abstract void updateEnemy();

        public abstract void drawEnemy();
    }
}
