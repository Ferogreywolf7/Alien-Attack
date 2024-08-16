using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Alien_Attack
{
    abstract class Enemies
    {
        //Instance variables
        private Vector2 position = new Vector2(0, 0);
        public Enemies() {
        }

        public Vector2 getPosition() {
            return position;
        }

        protected abstract void movement();  

    }
}
