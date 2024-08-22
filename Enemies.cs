using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Alien_Attack
{
    abstract class Enemies
    {
        //Instance variables
        private Vector2 position = new Vector2(0, 50);
        public Enemies() {
        }

        public Vector2 getPosition() {
            return position;
        }

        protected abstract void enemyUpdates();  

    }
}
