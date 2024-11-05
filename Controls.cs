using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private KeyboardState _keyboard;

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
            _keyboard = Keyboard.GetState();
        }


        //Mutators
        public bool setLeft(KeyboardState currentKeyState) {
            keyAccepted = false;
            //Doesn't work apart from the exact instant it checks yet it get stuck in the while loop
            _keyboard = currentKeyState;
                Debug.WriteLine(string.Join(' ', _keyboard.GetPressedKeys()));
            //When one key is pressed, get that key and put it into the left variable
            if (_keyboard.GetPressedKeyCount() == 1)
            {
                Debug.WriteLine("pressed");
                left = _keyboard.GetPressedKeys()[0];
                Debug.WriteLine("Left set as " + left);
                keyAccepted = true;
            }
            return keyAccepted;
        }

        public bool setRight(KeyboardState currentKeyState)
        {
            keyAccepted = false;
            _keyboard = currentKeyState;
                //Doesn't work apart from the exact instant it checks yet it get stuck in the while loop
                getKeyboardState();
                if (_keyboard.GetPressedKeyCount() == 1)
                {
                    right = _keyboard.GetPressedKeys()[0];
                    keyAccepted = true;
                }
            return keyAccepted;
        }

        public bool setShoot(KeyboardState currentKeyState)
        {
            keyAccepted = false;
            _keyboard = currentKeyState;
                //Doesn't work apart from the exact instant it checks yet it get stuck in the while loop
                if (_keyboard.GetPressedKeyCount() == 1)
                {
                    fire = _keyboard.GetPressedKeys()[0];
                    keyAccepted = true;
                }
            return keyAccepted;
        }

        public bool setPause(KeyboardState currentKeyState)
        {
            keyAccepted = false;
            _keyboard = currentKeyState;
                getKeyboardState();
                if (_keyboard.GetPressedKeyCount() == 1)
                {
                    pause = _keyboard.GetPressedKeys()[0];
                    keyAccepted = true;
                }
            return keyAccepted;
            }
        }
    }