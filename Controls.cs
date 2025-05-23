﻿using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Alien_Attack
{
    public class Controls
    {
        //Instansiation variables
        private Keys fire;
        private Keys left;
        private Keys right;
        private Keys pause;
        private bool keyInput;
        private KeyboardState _keyboard;
        private bool keyAccepted;
        private bool isKeyInput;
        private string tempGetNewKeybindOf;
        private string getNewKeybindOf;

        public Controls()
        {
            //Default controls
            fire = Keys.Space;
            left = Keys.A;
            right = Keys.D;
            pause = Keys.P;
        }

        //Accessors
        public Keys getLeft()
        {
            return left;
        }

        public Keys getRight()
        {
            return right;
        }

        public Keys getFire()
        {
            return fire;
        }

        public Keys getPause()
        {
            return pause;
        }

        private void getKeyboardState()
        {
            _keyboard = Keyboard.GetState();
        }


        //Mutators
        public bool setLeft(KeyboardState currentKeyState)
        {
            keyInput = false;
            _keyboard = currentKeyState;
                //When one key is pressed, get that key and put it into the left variable
            if (_keyboard.GetPressedKeyCount() == 1)
            {
                Debug.WriteLine("pressed");
                left = _keyboard.GetPressedKeys()[0];
                Debug.WriteLine("Left set as " + left);
                keyInput = true;
            }
            return keyInput;
        }

        public bool setRight(KeyboardState currentKeyState)
        {
            keyInput = false;
            _keyboard = currentKeyState;
            getKeyboardState();
            if (_keyboard.GetPressedKeyCount() == 1)
            {
                right = _keyboard.GetPressedKeys()[0];
                keyInput = true;
            }
            return keyInput;
        }

        public bool setFire(KeyboardState currentKeyState)
        {
            keyInput = false;
            _keyboard = currentKeyState;
            if (_keyboard.GetPressedKeyCount() == 1)
            {
                fire = _keyboard.GetPressedKeys()[0];
                keyInput = true;
            }
            return keyInput;
        }

        public bool setPause(KeyboardState currentKeyState)
        {
            keyInput = false;
            _keyboard = currentKeyState;
            getKeyboardState();
            if (_keyboard.GetPressedKeyCount() == 1)
            {
                pause = _keyboard.GetPressedKeys()[0];
                keyInput = true;
            }
            return keyInput;
        }

        public void getKeybindToSet(string tempGetNewKeybindOf)
        {
            this.tempGetNewKeybindOf = tempGetNewKeybindOf;
            if (tempGetNewKeybindOf == "")
            {
                keyAccepted = false;
            }
            else
            {
                isKeyInput = false;
            }
        }

        public void setNewKeybind(KeyboardState currentKeyState)
        {
            if (!keyAccepted && !isKeyInput)
            {
                    //There is a temporary variable to make sure the keyboard check is repeated
                if (tempGetNewKeybindOf != "")
                {
                    getNewKeybindOf = tempGetNewKeybindOf;
                }
                switch (getNewKeybindOf)
                {
                    case "Left":
                        isKeyInput = setLeft(currentKeyState);
                        break;
                    case "Right":
                        isKeyInput = setRight(currentKeyState);
                        break;
                    case "Shoot":
                        isKeyInput = setFire(currentKeyState);
                        break;
                    case "Pause":
                        isKeyInput = setPause(currentKeyState);
                        break;
                }
            }
        }
    }
}