using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alien_Attack
{
    internal class Controls
    {
        //Instansiation variables
        private Keys fire;
        private Keys left;
        private Keys right;
        private Keys pause;

        KeyboardState keyboard;

        public Controls() {
            //Default controls
            fire = Keys.Space;
            left = Keys.A;
            right = Keys.D;
            pause = Keys.P;
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

        public Keys getPause() {
            return pause;
        }

        private void getKeyboardState() {
            keyboard = Keyboard.GetState();
        }


        //Mutators
        public void setLeft() {
            getKeyboardState();
            left = keyboard.GetPressedKeys()[0];
        }

        public void setRight()
        {
            getKeyboardState();
            right = keyboard.GetPressedKeys()[0];
        }

        public void setFire()
        {
            getKeyboardState();
            fire = keyboard.GetPressedKeys()[0];
        }

        public void setPause() {
            getKeyboardState();
            pause = keyboard.GetPressedKeys()[0];
        }
    }
}
