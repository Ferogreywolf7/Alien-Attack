using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alien_Attack
{
    internal class controlsMenu
    {
        //Instansiation variables
        private Keys fire;
        private Keys left;
        private Keys right;

        public controlsMenu() {
            //Default controls
            fire = Keys.Space;
            left = Keys.A;
            right = Keys.D;
        }

        //Accessors
        public Keys getLeft() {
            return left;
        }

        public Keys getRight() {
            return right;
        }

        public Keys getFire() {
            return fire;
        }

        
    }
}
