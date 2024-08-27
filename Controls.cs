using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alien_Attack
{
    public class Controls
    {
        //Instansiation variables
        private Keys fire;
        private Keys left;
        private Keys right;
        private Keys pause;
        private bool keyAccepted;

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
            keyAccepted = false;
            while (!keyAccepted) 
            {

                //Doesn't work apart from the exact instant it checks yet it get stuck in the while loop
                getKeyboardState();
                if (keyboard.GetPressedKeyCount() == 1) {
                    left = keyboard.GetPressedKeys()[0];
                    keyAccepted = true;
                }
            }
        }

        public void setRight()
        {
            keyAccepted = false;
            while (!keyAccepted)
            {

                //Doesn't work apart from the exact instant it checks yet it get stuck in the while loop
                getKeyboardState();
                if (keyboard.GetPressedKeyCount() == 1)
                {
                    right = keyboard.GetPressedKeys()[0];
                    keyAccepted = true;
                }
            }
        }

        public void setShoot()
        {
            keyAccepted = false;
            while (!keyAccepted)
            {

                //Doesn't work apart from the exact instant it checks yet it get stuck in the while loop
                getKeyboardState();
                if (keyboard.GetPressedKeyCount() == 1)
                {
                    fire = keyboard.GetPressedKeys()[0];
                    keyAccepted = true;
                }
            }
        }

        public void setPause()
        {
            keyAccepted = false;
            while (!keyAccepted)
            {

                //Doesn't work apart from the exact instant it checks yet it get stuck in the while loop
                getKeyboardState();
                if (keyboard.GetPressedKeyCount() == 1)
                {
                    pause = keyboard.GetPressedKeys()[0];
                    keyAccepted = true;
                }
            }
        }
    }
}
