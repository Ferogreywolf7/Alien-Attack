using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alien_Attack
{
    internal class classicMedium : Enemies
    {
        public classicMedium()
        {

        }

        public override void updateEnemy()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        public override void drawEnemy()
        {
            
        }
    }
}
